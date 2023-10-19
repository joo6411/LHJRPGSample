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
        Debug.Log($"�α��� ��û:  {ID.text}, {password.text}");
        
        loading.SetActive(true);
        loadingText.text = "�α��� ��û ��";
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
        loadingText.text = "���� ���� ����";
        loading.SetActive(false);
        GameControl.instance.player.ChangeState(PlayerState.PlayerStateConnected);
    }

    public void ConnectFail()
    {
        loadingText.text = "���� ���� ����";
        StartCoroutine(IEConnectFail());

        IEnumerator IEConnectFail()
        {
            yield return new WaitForSeconds(2f);
            loadingText.text = "���� Ȯ���� ����� �ʿ�";
        }
    }

    public void LoginSuccess()
    {
        loadingText.text = "�α��� ����";
        StartCoroutine(IELoginSuccess()); 

        IEnumerator IELoginSuccess()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("Ingame");
        }
    }

    public void LoginFail()
    {
        loadingText.text = "�α��� ����";
        StartCoroutine(IELoginFail());

        IEnumerator IELoginFail()
        {
            yield return new WaitForSeconds(2f);
            loading.SetActive(false);
        }
    }
}