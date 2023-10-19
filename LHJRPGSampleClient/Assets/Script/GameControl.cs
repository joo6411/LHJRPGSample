using LHJSampleClientCS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public string serverIP;
    public int serverPort;
    public Player player;

    public static GameControl instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            player = new Player();
            player.Init(serverIP, serverPort);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}