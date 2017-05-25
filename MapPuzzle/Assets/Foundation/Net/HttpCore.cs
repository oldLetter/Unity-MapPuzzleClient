using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class HttpCore :MonoBehaviour {
    public static HttpCore instance;
    public GameObject loadingObj;
    [HideInInspector]
    public string currentAcount;
    [HideInInspector]
    public bool isLogin = true;
    private const string IP = "115.159.62.67";
    //private const string IP = "192.168.1.132";
    private const int PORT = 8844;

    static private TcpClient client = new TcpClient();
    private string ip;
    private int len;
   // private int id;
    private bool isHead;
    Thread thread;
    AutoResetEvent threadevent = new AutoResetEvent(false);
    AutoResetEvent datathread = new AutoResetEvent(false);
    static bool islinkError = true;
    private bool isReconnect = false;

    private HttpCore() { }
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
        loadingObj.SetActive(false);
    }
    public void startConnct()
    {
        begion();
        thread = new Thread(() => {
            while (true)
            {
                if (islinkError)
                {
                    Loom.QueueOnMainThread(() => {
                        if (!loadingObj.activeInHierarchy)
                        {
                            loadingObj.SetActive(true);
                        }
                    });

                    client = null;
                    client = new TcpClient();
                    Thread.Sleep(3000);
                    begion();
                }
                else
                {
                    threadevent.WaitOne();
                }
                Thread.Sleep(1);
            }
        });
        thread.Start();
    }
    public void begion()
    {
        islinkError = false;
        try
        {
            client.Connect(IP, PORT);
            Debug.Log("连接服务器成功\r\n");
            isHead = true;
            Loom.QueueOnMainThread(() => {
                if (isReconnect)
                {
                    ReconnectJoinUser();
                    isReconnect = false;
                }
                if (loadingObj.activeInHierarchy)
                {
                    loadingObj.SetActive(false);
                }
            });

        }
        catch (Exception e)
        {
            Debug.Log("连接服务器失败:" + e.Message);
            islinkError = true;
        }

    }
    public void ReconnectJoinUser()
    {
        UserInfoReq req = new UserInfoReq();
        req.acount = currentAcount;
        ReadSend<UserInfoReq, DefaultResp>(req, "UserController,ReconnectJoinUser", null, null);
    }

    public void ReadSend<ReqT, RespT>(ReqT t, string url, Action<RespT> onFnish, Action error)
    {
        byte[] req = Coder.Serialize(t, url);
        loadingObj.SetActive(true);
        Thread oThread = new Thread(new ParameterizedThreadStart(SendData<RespT>));
        oThread.IsBackground = false;
        object[] objs = new object[] { req, onFnish };
        oThread.Start(objs);
    }

    public void SendData<RespT>(object obj)
    {
        object[] objs = (object[])obj;
        byte[] req = (byte[])objs[0];
        Action<RespT> onFnish = (Action<RespT>)objs[1];
        try
        {
            client.GetStream().Write(req, 0, req.Length);
            int numberOfBytesRead = 0;
            if (client.GetStream().CanRead)
            {
                byte[] recieveData = new byte[1024 * 3];
                do
                {
                    numberOfBytesRead = client.GetStream().Read(recieveData, 0, recieveData.Length);
                    decoder(recieveData, onFnish);
                }
                while (client.GetStream().DataAvailable);
            }
            Loom.QueueOnMainThread(() =>
            {
                loadingObj.SetActive(false);
            });
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            islinkError = true;
            if (!isLogin)
            {
                isReconnect = true;
            }
            if (thread != null)
            {
                threadevent.Set();
            }
        }
    }

    public void decoder<RespT>(byte[] recieveData, Action<RespT> onFnish)
    {
        if (isHead)
        {
            byte[] lenByte = new byte[4];
            System.Array.Copy(recieveData, lenByte, 4);
            len = Coder.BytesToInt(lenByte, 0);
            isHead = false;
        }
        //读取消息体内容
        if (!isHead)
        {
            byte[] msgByte = new byte[len];
            System.Array.ConstrainedCopy(recieveData, 4, msgByte, 0, len);
            RespT resp = Coder.DeSerialize<RespT>(msgByte);
            Loom.QueueOnMainThread(() =>
            {
                if (onFnish != null)
                {
                    onFnish(resp);
                }
            });
            isHead = true;
            len = 0;
        }
    }
    void OnApplicationQuit()
    {
        client.Close();
    }

}
