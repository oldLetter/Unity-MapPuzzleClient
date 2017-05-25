using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/*
 *	
 *  Base View
 *
 *	by Xuanyi
 *
 */


public abstract class BaseView : MonoBehaviour
{
    protected void Start()
    {
        BindClick();
    }
    protected void BindClick()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (var item in buttons)
        {
            Button btn = item;
            if (btn.tag == "Untagged")
            {
                btn.onClick.AddListener(delegate ()
                {
                    this.OnClickButtons(btn.gameObject);
                });
            }
        }
        Toggle[] toggles = GetComponentsInChildren<Toggle>(true);
        if (toggles != null)
        {
            foreach (var item in toggles)
            {
                Toggle toggle = item;
                toggle.onValueChanged.AddListener(delegate (bool ison)
                {
                    OnToggleChange(toggle.gameObject, ison);
                });
            }
        }
    }
    protected void OnToggleChange(GameObject sender, bool ischange)
    {
        string className = this.GetType().FullName;
        Type type = Type.GetType(className);
        System.Object obj = Activator.CreateInstance(type, true);
        type.GetMethod(sender.name + "Click").Invoke(this, new object[1] { ischange });
    }
    protected void OnClickButtons(GameObject sender)
    {
        if (GetComponent<CanvasGroup>().alpha > 0.99)
        {
            if (PlayerPrefs.GetInt("soundEffect") == 1)
            {
                AudioController.Instance.SoundPlay("sfx_click");
            }
            string className = this.GetType().FullName;
            Type type = Type.GetType(className);
            System.Object obj = Activator.CreateInstance(type, true);
            type.GetMethod(sender.name + "Click").Invoke(this, new object[1] { sender });
        }   
    }
    public virtual void OnEnter(BaseContext context, object[] obj=null)
    {

    }

    public virtual void OnExit(BaseContext context)
    {

    }

    public virtual void OnPause(BaseContext context)
    {

    }

    public virtual void OnResume(BaseContext context)
    {

    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

