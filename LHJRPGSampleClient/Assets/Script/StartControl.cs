using LHJSampleClientCS;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartControl : MonoBehaviour
{
    public TextMeshProUGUI ID;
    public TextMeshProUGUI password;
    public TextMeshProUGUI loadingText;
    public GameObject loading;
    public static StartControl Instance;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            loading.SetActive(true);
            ServerConnect();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoginButton()
    {
        var loginReq = new REQ_LOGIN_PACKET();
        loginReq.SetValue(ID.text, password.text);

        GameControl.instance.player.networkMain.PostSendPacket(PACKET_ID.REQ_LOGIN, loginReq.ToBytes());
        Debug.Log($"로그인 요청:  {ID.text}, {password.text}");
        
        loading.SetActive(true);
        loadingText.text = "로그인 요청 중";
    }

    public void ExitButton()
    {
        GameControl.instance.player.ChangeState(PlayerState.PlayerStateNone);
        Application.Quit();
    }

    public void ServerConnect()
    {
        GameControl.instance.player.ChangeState(PlayerState.PlayerStateTryConnect);
    }

    public void ConnectSuccess()
    {
        loadingText.text = "서버 접속 성공";
        loading.SetActive(false);
        GameControl.instance.player.ChangeState(PlayerState.PlayerStateConnected);
    }

    public void ConnectFail()
    {
        loadingText.text = "서버 접속 실패";
        StartCoroutine(IEConnectFail());

        IEnumerator IEConnectFail()
        {
            yield return new WaitForSeconds(2f);
            loadingText.text = "서버 확인후 재실행 필요";
        }
    }

    public void LoginSuccess()
    {
        loadingText.text = "로그인 성공";
        StartCoroutine(IELoginSuccess()); 

        IEnumerator IELoginSuccess()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("Ingame");
        }
    }

    public void LoginFail()
    {
        loadingText.text = "로그인 실패";
        StartCoroutine(IELoginFail());

        IEnumerator IELoginFail()
        {
            yield return new WaitForSeconds(2f);
            loading.SetActive(false);
        }
    }
}