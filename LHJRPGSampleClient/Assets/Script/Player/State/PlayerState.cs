using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum PlayerState
{
    PlayerStateNone,
    PlayerStateTryConnect,
    PlayerStateConnected,
}

public abstract class PlayerStateBase
{
    protected Player player;

    public PlayerStateBase(Player player)
    {
        this.player = player;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

    //protected 
}