using System.Net.Sockets;
using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using ProtoBuf;
[ProtoContract]
public class DefaultRequest
{

}
[ProtoContract]
public class DefaultResp
{

}
public class HttpClient
{
    private static HttpClient instance;
    //玩家数据
    public UserInfo userInfo;
    public PapersScoreResp scorelist;
    public string currentAcount;
    public UserInfo UserInfo
    {
        get{return userInfo;}
        set
        {
            userInfo = value;
        }
    }

    public static HttpClient getInstnce()
    {
        if (instance == null)
        {
            instance = new HttpClient();
        }
        return instance;
    }

    public void initData(Action fnish)
    {
        DefaultRequest req = new DefaultRequest();
        ReadSend<DefaultRequest, InitDataResp>(req, "UserController,initData", resp =>
          {
              userInfo = resp.userinfo;
              scorelist = resp.screlist;
              fnish();
          }, null);
    }
    public void UpdateUserInfo<reqT>(reqT req,string url,Action onFnish=null)
    {
        ReadSend<reqT, UserInfo>(req, url, resp =>
        {
            HttpClient.getInstnce().UserInfo = resp;
            if (onFnish != null)
            {
                onFnish();
            }
        }, null);
    }

    public void ReadSend<ReqT, RespT>(ReqT t, string url, Action<RespT> onFnish, Action error)
    {
        HttpCore.instance.ReadSend<ReqT, RespT>(t, url, onFnish, error);
    }
   
}