using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProtoBuf;

public class GameContext : BaseContext
{
    public GameContext() : base(UIType.GameView)
    {
       
    }
}
[ProtoContract]
public class HintReq
{
    [ProtoMember(1)]
    public string acount;
    [ProtoMember(2)]
    public int hintcount;
}
public class GameView : AnimateView {
    [SerializeField]
    private GameObject complePanel;
    [SerializeField]
    private GameObject nextBtObj;
    [SerializeField]
    private List<GameObject> stars;
    [SerializeField]
    private Text gradetext;
    [SerializeField]
    private Text usetimetext;
    [SerializeField]
    private Text showtimetx;
    [SerializeField]
    private Text piecenametx;
    [SerializeField]
    private Text hintcounttx;
    [SerializeField]
    private GameObject pauseObj;

    private int grade;
    private int star;
    private int usetime;
    private Timer time;
    private int usehintCount;
    private int hintcount;

    public override void OnEnter(BaseContext context, object[] obj = null)
    {
        DataAssign.getInstance().LoadOnePiece = setPieceName;
        hintcount = HttpClient.getInstnce().UserInfo.hintCount;
        hintcounttx.text = hintcount.ToString();
        time = new Timer(0);
        time.Start();
        DataAssign.getInstance().LevelCompletecall = LevelComplete;
        base.OnEnter(context);
        pauseObj.SetActive(false);
        if (complePanel.activeInHierarchy == true)
        {
            complePanel.SetActive(false);
        }
        for(int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive(false);
        }
    }
    void Update()
    {
        if (time != null)
        {
            time.Update(Time.deltaTime);
            showtimetx.text = ((int)time.f_CurTime)+"S";
        }
    }

    public void LevelComplete()
    {
//        updateHintCount();
        int usetime = (int)time.f_CurTime;
        time.Stop();
        time = null;
        complePanel.SetActive(true);
        if (!nextBtObj.activeInHierarchy)
        {
            nextBtObj.SetActive(true);
        }
        CalculateGrade(usehintCount,usetime);
        for(int i = 0; i < star; i++)
        {
            stars[i].SetActive(true);
        }
        if (GameManager.getInstance().currentlevel == HttpClient.getInstnce().UserInfo.maxLevel)
        {
            nextBtObj.SetActive(false);
        }
        gradetext.text = grade.ToString();
        usetimetext.text = usetime.ToString()+"s";
        usehintCount = 0;
    }
    public void setPieceName(string piecename)
    {
        piecenametx.text = piecename;
    }
    private void CalculateGrade(int hintcount,int time)
    {
        if (time < 100)
        {
            grade = 100 - hintcount * 5;
        }
        else if (time < 200)
        {
            grade = 80 - hintcount * 5;
        }
        else
        {
            grade = 60 - hintcount * 5;
        }
        if (grade < 30)
        {
            grade = 30;
        }
        CalculateStars();
    }
    private void CalculateStars()
    {
        if (grade >= 80)
        {
            star = 3;
        }
        else if (grade >= 60)
        {
            star = 2;
        }
        else
        {
            star = 1;
        }
    }
    public void updateHintCount()
    {
        HintReq req = new HintReq();
        req.acount = HttpClient.getInstnce().UserInfo.acount;
        req.hintcount = hintcount;
        HttpClient.getInstnce().UpdateUserInfo<HintReq>(req, "GameController,updateHintCount");
    }
    public void backBtClick(GameObject sender)
    {
        GameObject pieceObj = GameObject.Find("MainPiece");
        Destroy(pieceObj);
        Singleton<ContextManager>.Instance.Pop();
    }

    public void pauseBtClick(GameObject sender)
    {
        if (time!= null){
            pauseObj.SetActive(true);
            time.Stop();
        }
    }
    public void homepageBtClick(GameObject sender)
    {
        Singleton<ContextManager>.Instance.Pop();
        GameObject pieceObj = GameObject.Find("MainPiece");
        Destroy(pieceObj);
    }
    public void againBtClick(GameObject sender)
    {
        if (pauseObj.activeInHierarchy)
        {
            pauseObj.SetActive(false);
        }
        complePanel.SetActive(false);
        GameManager.getInstance().LoadLevel();
        time = new Timer(0);
        time.Restart();
    }
    public void startBtClick(GameObject sender)
    {
        if (time != null)
        {
            pauseObj.SetActive(false);
            time.Start();
        }
    }
    public void nextLevelBtClick(GameObject sender)
    {
        if(GameManager.getInstance().currentlevel < HttpClient.getInstnce().UserInfo.currentLevel)
        {
            complePanel.SetActive(false);
            GameManager.getInstance().currentlevel++;
            GameManager.getInstance().LoadLevel();
            time = new Timer(0);
            time.Restart();
        }
    }
    public void hintBtClick(GameObject sender)
    {
        if(hintcount > 0)
        {
            if (DataAssign.getInstance().HintCall())
                {
                usehintCount++;
                hintcount--;
                updateHintCount();
                hintcounttx.text = hintcount.ToString();
            }
        }
        else
        {
            sender.GetComponent<Button>().enabled = false;
        }
    }

}
