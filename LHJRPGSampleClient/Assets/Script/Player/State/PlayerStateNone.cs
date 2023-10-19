using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateNone : PlayerStateBase
{
    public PlayerStateNone(Player player) : base(player)
    {
    }

    public override void Enter()
    {
    }

    public override void Exit() { }
    public override void Execute() { }
}
