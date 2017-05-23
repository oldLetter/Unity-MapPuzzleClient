﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainContext : BaseContext
{
    public MainContext() : base(UIType.MainView)
    {

    }
}
public class MainView : AnimateView
{
    [SerializeField]
    private List<Button> bts;
    [SerializeField]
    private GameObject diffcultObj;
    [SerializeField]
    private GameObject setObj;
    public Toggle msopen;
    public Toggle msclose;
    public Toggle seopen;
    public Toggle seclose;
    public GameObject musicVolumObj;

    private List<LevelBtItem> btItems = new List<LevelBtItem>();
    private int chooseLevel;
    private GameObject earth;
    public void Awake()
    {
        earth = GameObject.Instantiate(Resources.Load<GameObject>("Model/Earth")) as GameObject;
        earth.layer = 11;
        foreach (Transform tran in earth.GetComponentsInChildren<Transform>())
        {//遍历当前物体及其所有子物体
            tran.gameObject.layer = 11;
        }
        earth.AddComponent<EarthRota>().rotaSpeed = 0.4f;
    }
    public override void OnEnter(BaseContext context, object[] obj = null)
    {
        base.OnEnter(context);
        earth.SetActive(true);
        InitSetTog();
        if (diffcultObj.activeInHierarchy)
        {
            diffcultObj.SetActive(false);
        }
        if (setObj.activeInHierarchy)
        {
            setObj.SetActive(false);
        }
        btItems.Clear();
        for (int i = 0; i < bts.Count; i++)
        {
            LevelBtItem btitem = bts[i].gameObject.GetComponent<LevelBtItem>();
            btitem.Initialize((i + 1).ToString());
            btItems.Add(btitem);
        }
        for (int i = 0; i < HttpClient.getInstnce().UserInfo.currentLevel; i++)
        {
            btItems[i].ChangeState();
        }
    }

    public override void OnPause(BaseContext context)
    {
        base.OnPause(context);
        earth.SetActive(false);
    }
    public override void OnExit(BaseContext context)
    {
        base.OnExit(context);
        earth.SetActive(false);
    }
    public void firstLevelBtClick(GameObject sender)
    {
        chooseLevel = 1;
        diffcultObj.SetActive(true);
    }
    public void secondLevelBtClick(GameObject sender)
    {
        chooseLevel = 2;
        diffcultObj.SetActive(true);
    }
    public void thirdLevelBtClick(GameObject sender)
    {
        chooseLevel = 3;
        diffcultObj.SetActive(true);
    }
    public void answerBtClick(GameObject sender)
    {
        Singleton<ContextManager>.Instance.Push(new AnswerMainContext());
    }
    public void logoutBtClick(GameObject sender)
    {
        HttpClient.getInstnce().UserInfo = null;
        Singleton<ContextManager>.Instance.Pop();
    }
    public void closeBtClick(GameObject sender)
    {
        diffcultObj.SetActive(false);
    }

    public void simpBtClick(GameObject sender)
    {
        startLoad(1);

    }
    public void diffcultyBtClick(GameObject sender)
    {
        startLoad(2);
    }
    public void setBtClick(GameObject sender)
    {
        setObj.SetActive(true);
    }
    public void setcloseBtClick(GameObject sender)
    {
        setObj.SetActive(false);
    }
    public void msopenTgClick(bool ison)
    {
        if (ison)
        {
            AudioController.Instance.BGMPlay("bgm_game");
            musicVolumObj.SetActive(true);
            PlayerPrefs.SetInt("music", 1);
        }
    }
    public void mscloseTgClick(bool ison)
    {
        if (ison)
        {
            AudioController.Instance.BGMPause();
            musicVolumObj.SetActive(false);
            PlayerPrefs.SetInt("music", 0);
        }
    }
    public void MusicVolum(Slider slid)
    {
        AudioController.Instance.BGMSetVolume(slid.value);
    }
    public void seopenTgClick(bool ison)
    {
        if (ison)
        {
            PlayerPrefs.SetInt("soundEffect", 1);
        }
    }
    public void secloseTgClick(bool ison)
    {
        if (ison)
        {
            PlayerPrefs.SetInt("soundEffect", 0);
        }
    }
    public void startLoad(int difficult)
    {
        diffcultObj.SetActive(false);
        Singleton<ContextManager>.Instance.Push(new GameContext());
        GameManager.getInstance().currentlevel = chooseLevel;
        GameManager.getInstance().currentDifficult = difficult;
        GameManager.getInstance().LoadLevel();
    }
    public void InitSetTog()
    {
        if (PlayerPrefs.GetInt("music") == 1)
        {
            msopen.isOn = true;
            msclose.isOn = false;
            musicVolumObj.SetActive(true);
            musicVolumObj.GetComponent<Slider>().value = 0.3f;
        }
        else
        {
            msopen.isOn = false;
            msclose.isOn = true;
            musicVolumObj.SetActive(false);
        }
        if (PlayerPrefs.GetInt("soundEffect") == 1)
        {
            seopen.isOn = true;
            seclose.isOn = false;
        }
        else
        {
            seopen.isOn = false;
            seclose.isOn = true;
        }
    }
}