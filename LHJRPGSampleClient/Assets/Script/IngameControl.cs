using LHJSampleClientCS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameControl : MonoBehaviour
{
    public static IngameControl Instance;
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //GameControl.instance.player.networkMain.PostSendPacket(PACKET_ID.req, loginReq.ToBytes());
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
