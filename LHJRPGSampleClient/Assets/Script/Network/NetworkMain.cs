using LHJSampleClientCS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class NetworkMain
{
    ClientNetwork Network = new ClientNetwork();

    bool IsNetworkThreadRunning = false;
    bool IsBackGroundProcessRunning = false;

    System.Threading.Thread NetworkReadThread = null;
    System.Threading.Thread NetworkSendThread = null;
    System.Threading.Thread dispatcherUITimer = null;

    PacketBufferManager PacketBuffer = new PacketBufferManager();
    Queue<PacketData> RecvPacketQueue = new Queue<PacketData>();
    Queue<byte[]> SendPacketQueue = new Queue<byte[]>();

    Dictionary<PACKET_ID, Action<byte[]>> PacketFuncDic = new Dictionary<PACKET_ID, Action<byte[]>>();

    void SetPacketHandler()
    {
        /*
        PacketFuncDic.Add(PACKET_ID.ACK_LOGIN, PacketProcess_AckLogin);
        PacketFuncDic.Add(PACKET_ID.ACK_ROOM_ENTER, PacketProcess_AckRoomEnter);
        PacketFuncDic.Add(PACKET_ID.ACK_LOBBY_INFO, PacketProcess_AckLobbyInfo);
        PacketFuncDic.Add(PACKET_ID.NOTIFY_LOBBY_INFO, PacketProcess_NotifyLobbyInfo);
        PacketFuncDic.Add(PACKET_ID.ACK_ROOM_CHAT, PacketProcess_AckRoomChat);
        PacketFuncDic.Add(PACKET_ID.NOTIFY_ROOM_CHAT, PacketProcess_NotifyRoomChat);
        PacketFuncDic.Add(PACKET_ID.ACK_ROOM_INFO, PacketProcess_AckRoomInfo);
        PacketFuncDic.Add(PACKET_ID.NOTIFY_ROOM_INFO, PacketProcess_NotifyRoomInfo);
        PacketFuncDic.Add(PACKET_ID.ACK_ROOM_LEAVE, PacketProcess_AckRoomLeave);
        PacketFuncDic.Add(PACKET_ID.NOTIFY_ROOM_LEAVE, PacketProcess_NotifyRoomLeave);
        */
    }

    void Init()
    {
        PacketBuffer.Init((8096 * 10), PacketDef.PACKET_HEADER_SIZE, 1024);

        IsNetworkThreadRunning = true;
        NetworkReadThread = new System.Threading.Thread(this.NetworkReadProcess);
        NetworkReadThread.Start();
        NetworkSendThread = new System.Threading.Thread(this.NetworkSendProcess);
        NetworkSendThread.Start();

        IsBackGroundProcessRunning = true;

        dispatcherUITimer = new System.Threading.Thread(this.BackGroundProcess);
        dispatcherUITimer.Start();

        IsBackGroundProcessRunning = true;

        /*
        dispatcherUITimer = new System.Windows.Threading.DispatcherTimer();
        dispatcherUITimer.Tick += new EventHandler(BackGroundProcess);
        dispatcherUITimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        dispatcherUITimer.Start();
        */

        SetPacketHandler();
    }

    void PacketProcess(PacketData packet)
    {
        var packetType = (PACKET_ID)packet.PacketID;
        //DevLog.Write("Packet Error:  PacketID:{packet.PacketID.ToString()},  Error: {(RESULT_CODE)packet.Result}");
        //DevLog.Write("RawPacket: " + packet.PacketID.ToString() + ", " + PacketDump.Bytes(packet.BodyData));

        if (PacketFuncDic.ContainsKey(packetType))
        {
            PacketFuncDic[packetType](packet.BodyData);
        }
        else
        {
            //DevLog.Write("Unknown Packet Id: " + packet.PacketID.ToString());
        }
    }

    void NetworkReadProcess()
    {
        const Int16 PacketHeaderSize = PacketDef.PACKET_HEADER_SIZE;

        while (IsNetworkThreadRunning)
        {
            if (Network.IsConnected() == false)
            {
                System.Threading.Thread.Sleep(1);
                continue;
            }

            var recvData = Network.Receive();

            if (recvData != null)
            {
                PacketBuffer.Write(recvData.Item2, 0, recvData.Item1);

                while (true)
                {
                    var data = PacketBuffer.Read();
                    if (data.Count < 1)
                    {
                        break;
                    }

                    var packet = new PacketData();
                    packet.DataSize = (short)(data.Count - PacketHeaderSize);
                    packet.PacketID = BitConverter.ToInt16(data.Array, data.Offset + 2);
                    packet.Type = (SByte)data.Array[(data.Offset + 4)];
                    packet.BodyData = new byte[packet.DataSize];
                    Buffer.BlockCopy(data.Array, (data.Offset + PacketHeaderSize), packet.BodyData, 0, (data.Count - PacketHeaderSize));
                    lock (((System.Collections.ICollection)RecvPacketQueue).SyncRoot)
                    {
                        RecvPacketQueue.Enqueue(packet);
                    }
                }

                //DevLog.Write($"받은 데이터 크기: {recvData.Item1}", LOG_LEVEL.INFO);
            }
            else
            {
                Network.Close();
                //DevLog.Write("서버와 접속 종료 !!!", LOG_LEVEL.INFO);
            }
        }
    }

    void NetworkSendProcess()
    {
        while (IsNetworkThreadRunning)
        {
            System.Threading.Thread.Sleep(1);

            if (Network.IsConnected() == false)
            {
                continue;
            }

            lock (((System.Collections.ICollection)SendPacketQueue).SyncRoot)
            {
                if (SendPacketQueue.Count > 0)
                {
                    var packet = SendPacketQueue.Dequeue();
                    Network.Send(packet);
                }
            }
        }
    }

    void BackGroundProcess()
    {
        while (IsNetworkThreadRunning)
        {
            System.Threading.Thread.Sleep(100);

            var packet = new PacketData();

            lock (((System.Collections.ICollection)RecvPacketQueue).SyncRoot)
            {
                if (RecvPacketQueue.Count > 0)
                {
                    packet = RecvPacketQueue.Dequeue();
                }
            }

            if (packet.PacketID != 0)
            {
                PacketProcess(packet);
            }
        }
    }

    public void PostSendPacket(PACKET_ID packetID, byte[] bodyData)
    {
        if (Network.IsConnected() == false)
        {
            //DevLog.Write("서버 연결이 되어 있지 않습니다", LOG_LEVEL.ERROR);
            return;
        }

        Int16 bodyDataSize = 0;
        if (bodyData != null)
        {
            bodyDataSize = (Int16)bodyData.Length;
        }
        var packetSize = bodyDataSize + PacketDef.PACKET_HEADER_SIZE;

        List<byte> dataSource = new List<byte>();
        dataSource.AddRange(BitConverter.GetBytes((UInt16)packetSize));
        dataSource.AddRange(BitConverter.GetBytes((UInt16)packetID));
        dataSource.AddRange(new byte[] { (byte)0 });

        if (bodyData != null)
        {
            dataSource.AddRange(bodyData);
        }

        SendPacketQueue.Enqueue(dataSource.ToArray());
    }
}
