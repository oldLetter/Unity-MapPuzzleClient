using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ProtoContract]
public class InitDataResp
{
    [ProtoMember(1)]
    public UserInfo userinfo;
    [ProtoMember(2)]
    public PapersScoreResp screlist;
    public InitDataResp()
    {
        userinfo = new UserInfo();
        screlist = new PapersScoreResp();
    }
}
[ProtoContract]
public class LoginReq
{
    [ProtoMember(1)]
    public string usr;
    [ProtoMember(2)]
    public string pwd;

    public LoginReq()
    {
    }
}
[ProtoContract]
public class LoginResp
{
    public static int ACOUNT_INEXISTENCE = 0;
    public static int PWD_ERROR = 1;
    public static int LOGIN_SUCCESS = 2;

    [ProtoMember(1)]
    public int result;

    public LoginResp()
    {
    }
}

[ProtoContract]
public class RegistReq
{
    [ProtoMember(1)]
    public string acount;
    [ProtoMember(2)]
    public string pwd;

    public RegistReq()
    {
    }
}
[ProtoContract]
public class RegistResp
{
    public static int ACOUNT_EXIST = 0;
    public static int REGIST_SUCCESS = 1;

    [ProtoMember(1)]
    public int result;

    public RegistResp()
    {
    }
}


public class LoginContext : BaseContext
{
    public LoginContext() : base(UIType.LoginView) { }
}
public class LoginView:AnimateView
{
    [SerializeField]
    private InputField acountText;
    [SerializeField]
    private InputField pwdText;
    [SerializeField]
    private Text hintText;

    string acount;
    public void Awake()
    {
        InitMusic();
        if (!PlayerPrefs.HasKey("acount"))
        {
            PlayerPrefs.SetString("acount", "");
        }
    }
    public override void OnEnter(BaseContext context, object[] obj = null)
    {
        base.OnEnter(context, obj);
        acountText.text = PlayerPrefs.GetString("acount");
    }
    public void loginBtClick(GameObject sender)
    {
        LoginReq req = new LoginReq();
        if (acountText.text != "")
        {
            if (pwdText.text != "")
            {
                acount = acountText.text;
                req.usr = acountText.text;
                req.pwd = pwdText.text;
                HttpClient.getInstnce().ReadSend<LoginReq, LoginResp>(req, "UserController,Login", resp =>
                {
                    if (resp.result == LoginResp.LOGIN_SUCCESS)
                    {
                        //getUserInfo();
                        initData();
                        PlayerPrefs.SetString("acount", acount);
                    }
                    else if (resp.result == LoginResp.ACOUNT_INEXISTENCE)
                    {
                        hintText.text = Singleton<Localization>.Instance.GetText("acount_inexistence");
                    }
                    else if (resp.result == LoginResp.PWD_ERROR)
                    {
                        hintText.text = Singleton<Localization>.Instance.GetText("pwd_error");
                    }
                }, null);
            }
            else
            {
                hintText.text= Singleton<Localization>.Instance.GetText("pwd_null");
            }
        }
        else
        {
            hintText.text = Singleton<Localization>.Instance.GetText("acount_null");
        }
      
    }

    public void registBtClick(GameObject sender)
    {
        RegistReq req = new RegistReq();
        if (acountText.text.Length>=2)
        {
            if (pwdText.text != "")
            {
                acount = acountText.text;
                req.acount = acountText.text;
                req.pwd = pwdText.text;
                HttpClient.getInstnce().ReadSend<RegistReq, RegistResp>(req, "UserController,Regist", resp =>
                {
                    if (resp.result == RegistResp.ACOUNT_EXIST)
                    {
                        hintText.text = Singleton<Localization>.Instance.GetText("acount_exist");
                    }
                    else if (resp.result == RegistResp.REGIST_SUCCESS)
                    {
                        PlayerPrefs.SetString("acount", acount);
                        getUserInfo();
                    }
                }, null);
            }
            else
            {
                hintText.text = Singleton<Localization>.Instance.GetText("pwd_null");
            }
        }
        else
        {
            hintText.text = Singleton<Localization>.Instance.GetText("acount_short");
        }
    }

    public void OnInputChange()
    {
        hintText.text = "";
    }

    public void getUserInfo()
    {
        UserInfoReq req = new UserInfoReq();
        req.acount = acount;
        HttpClient.getInstnce().UpdateUserInfo<UserInfoReq>(req, "UserController,returnUsreInfo", () =>
        {
            Singleton<ContextManager>.Instance.Push(new MainContext());
        });
    }
    public void initData()
    {
        HttpClient.getInstnce().initData(() =>
        {
            Singleton<ContextManager>.Instance.Push(new MainContext());
        });
    }
    public void InitMusic()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            AudioController.Instance.BGMPlay("bgm_game");
        }
    }
}
