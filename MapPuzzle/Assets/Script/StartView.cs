using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartContext : BaseContext
{
    public StartContext() : base(UIType.StartView)
    {

    }      
}

public class StartView : AnimateView
{
    private static bool haserun = true;
    private Action call;
    public void Awake()
    {
        if (haserun)
        {
            Invoke("startLogin", 2.0f);
            haserun = false;
        }
    }

    public override void OnEnter(BaseContext context, object[] obj = null)
    {
        base.OnEnter(context);
    }

    public override void OnExit(BaseContext context)
    {
        base.OnExit(context);
    }
    public void startLogin()
    {
        Singleton<ContextManager>.Instance.Push(new LoginContext());
    }

}

