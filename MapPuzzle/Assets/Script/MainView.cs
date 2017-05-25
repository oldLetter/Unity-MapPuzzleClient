using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProtoBuf;
public class MainContext : BaseContext
{
    public MainContext() : base(UIType.MainView)
    {

    }
}
[ProtoContract]
public class LogoutResp
{
    [ProtoMember(1)]
    public bool state;
}
public class MainView : AnimateView
{
    [SerializeField]
    private List<Button> bts;
    [SerializeField]
    private GameObject diffcultObj;
    [SerializeField]
    private GameObject setObj;
    public GameObject levelDetailObj;
    public Toggle msopen;
    public Toggle msclose;
    public Toggle seopen;
    public Toggle seclose;
    public GameObject musicVolumObj;
    public Text titletx;
    public Text infotx;

    private List<LevelBtItem> btItems = new List<LevelBtItem>();
    private int chooseLevel;
    private GameObject earth;
    private string leveldetailxml = "LevelDetail";
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
            btitem.Initialize((i + 1).ToString(),i);
            btItems.Add(btitem);
        }
        for (int i = 0; i < HttpClient.getInstnce().UserInfo.currentLevel; i++)
        {
            btItems[i].onShortPress = LevelShortPress;
            btItems[i].onLongPress = LevelLongPress;
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

    public void answerBtClick(GameObject sender)
    {
        Singleton<ContextManager>.Instance.Push(new AnswerMainContext());
    }
    public void logoutBtClick(GameObject sender)
    {
        DefaultRequest req = new DefaultRequest();
        HttpClient.getInstnce().ReadSend<DefaultRequest, LogoutResp>(req, "UserController,Logout", resp =>
          {
              if (resp.state)
              {
                  HttpClient.getInstnce().UserInfo = null;
                  HttpCore.instance.isLogin = true;
                  Singleton<ContextManager>.Instance.Pop();
              }
          }, null);
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
    public void LevelLongPress(object obj)
    {
        int id = (int)obj;
        List<string[]> strList = GetXML.getInstance().LoadData(leveldetailxml);
        levelDetailObj.SetActive(true);
        titletx.text = strList[id][1].ToString();
        infotx.text = strList[id][2].ToString();
    }
    public void LevelShortPress(object obj)
    {
        int id = (int)obj;
        chooseLevel = id + 1;
        diffcultObj.SetActive(true);
    }
    public void levelinfocloseBtClick(GameObject sender)
    {
        levelDetailObj.SetActive(false);
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
