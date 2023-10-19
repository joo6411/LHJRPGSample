using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateTryConnect : PlayerStateBase
{
    public PlayerStateTryConnect(Player player) : base(player)
    {
    }

    public override void Enter()
    {
        if(player.networkMain.Connect())
        {
            StartControl.Instance.ConnectSuccess();
        }
        else
        {
            StartControl.Instance.ConnectFail();
        }
    }

    public override void Exit() { }
    public override void Execute() { }
}
