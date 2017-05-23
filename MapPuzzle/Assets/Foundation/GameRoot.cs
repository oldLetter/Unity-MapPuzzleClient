using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameRoot : MonoBehaviour {
    private float currenttime;
    private bool backDown = false;
       public void Start()
       {
        HttpClient.getInstnce().startConnct();
        Begin();
        currenttime = Time.time;        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (backDown && Time.time - currenttime < 1.2f)
            {
                Debug.Log("dsdfad");
                Application.Quit();
            }
            backDown = true;
            currenttime = Time.time;
        }
    }

    public void Begin()
    {
        Singleton<Localization>.Create();
        Singleton<UIManager>.Create();
        Singleton<ContextManager>.Create();
    }
}

