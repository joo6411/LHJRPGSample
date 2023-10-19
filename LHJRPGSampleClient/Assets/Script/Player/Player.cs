using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Player
{
    public NetworkMain networkMain;
    private PlayerState mState;
    private Dictionary<PlayerState, PlayerStateBase> mStates;

    public void Init(string ip, int port)
    {
        networkMain = new NetworkMain();
        networkMain.Init(ip, port);
        mState = PlayerState.PlayerStateNone;
        mStates[PlayerState.PlayerStateNone] = new PlayerStateNone(this);
        mStates[PlayerState.PlayerStateTryConnect] = new PlayerStateTryConnect(this);
        mStates[PlayerState.PlayerStateConnected] = new PlayerStateConnected(this);
    }

    public void ChangeState(PlayerState newState)
    {
        mStates[mState].Exit();
        mState = newState;
        mStates[mState].Enter();
    }
}