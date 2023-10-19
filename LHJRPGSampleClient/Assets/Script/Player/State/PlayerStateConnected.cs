using LHJSampleClientCS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateConnected : PlayerStateBase
{
    public PlayerStateConnected(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        player.networkMain.PacketFuncDic.Add(PACKET_ID.ACK_LOGIN, PacketProcess_AckLogin);
    }

    public override void Exit() { }
    public override void Execute()   {  }

    private void PacketProcess_AckLogin(byte[] bodyData)
    {
        var responsePkt = new ACK_LOGIN_PACKET();
        responsePkt.FromBytes(bodyData);

        Debug.Log($"로그인 결과:  {(RESULT_CODE)responsePkt.Result}");

        if ((RESULT_CODE)responsePkt.Result == RESULT_CODE.LOGIN_SUCCESS)
        {
            StartControl.Instance.LoginSuccess();
            //var lobbyReq = new REQ_LOBBY_INFO_PACKET();
            //LoginBtn.Enabled = false;

            //player.networkMain.PostSendPacket(PACKET_ID.REQ_LOBBY_INFO, lobbyReq.ToBytes());
            //Debug.Log($"로비정보 요청");
        }
        else
        {

        }
    }
}
