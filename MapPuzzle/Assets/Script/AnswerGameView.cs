using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProtoBuf;

public class AnswerGameContext : BaseContext
{
    public AnswerGameContext() : base(UIType.AnswerGameView) { }
}

[ProtoContract]
public class Questiondetail
{
    [ProtoMember(1)]
    public string id;
    [ProtoMember(2)]
    public string title;
    [ProtoMember(3)]
    public string answerA;
    [ProtoMember(4)]
    public string answerB;
    [ProtoMember(5)]
    public string answerC;
    [ProtoMember(6)]
    public string answerD;
    [ProtoMember(7)]
    public string rightAnswer;
    public Questiondetail()
    {

    }
    public Questiondetail(string Id,string Title, string AnswerA, string AnswerB, string AnswerC, string AnswerD,string RightAnswer)
    {
        id = Id;
        title = Title;
        answerA = AnswerA;
        answerB = AnswerB;
        answerC = AnswerC;
        answerD = AnswerD;
        rightAnswer = RightAnswer;
    }
}
[ProtoContract]
public class QuestionListReq
{
    [ProtoMember(1)]
    public string paperName;
    [ProtoMember(2)]
    public string id;
}
[ProtoContract]
public class QuestionListResp
{
    [ProtoMember(1)]
    public List<Questiondetail> questionList;
    [ProtoMember(2)]
    public int maxscore;

    public QuestionListResp()
    {
        questionList = new List<Questiondetail>();
    }
}
public class AnswerGameView : AnimateView
{
    public Text detailtx;
    public Text atx;
    public Text btx;
    public Text ctx;
    public Text dtx;
    public Text righttx;
    public Text showtimetx;
    public Text usetimetx;
    public Text gradetx;
    public Text maxgradetx;
    public List<GameObject> stars;
    public List<Button> answerBts;
    public GameObject resultObj;
    public GameObject completeObj;

    private string rightAnswer;
    private string chooseAnswer;
    private static string localPath;
    private WWW www;
    private string xmlName ;
    private List<Questiondetail> questionList;
    private int index ;
    private int rightCount;
    private Timer time;
    private bool paperType;
    private int maxscore;
    private PaperInfo info;
    private Color choosecolor = new Color(0.678f, 0.678f, 0.678f, 0.785f);
    private Color initcolor = new Color(1, 1, 1, 0.785f);
    public override void OnEnter(BaseContext context, object[] obj = null)
    {
        base.OnEnter(context);
        index = 0;
        rightCount = 0;
        time = new Timer(0);
        resultObj.SetActive(false);
        completeObj.SetActive(false);
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive(false);
        }
        if (obj != null)
        {
            paperType = (bool)obj[1];
            info = (PaperInfo)obj[0];
            xmlName =info.paperName;
            if (obj.Length == 3)
            {
                maxscore = (int)obj[2];
            }
            time.Start();
        }
        if (paperType)
        {
            InitLocalPaperList();
        }
        else
        {
            InitNetPaperList();
        }
    }
    void Update()
    {
        if (time != null)
        {
            time.Update(Time.deltaTime);
            showtimetx.text = ((int)time.f_CurTime) + "S";
        }
    }
    public void loadQuestion()
    {
        detailtx.text = questionList[index].id+"."+ questionList[index].title;
        atx.text = "A : " + questionList[index].answerA;
        btx.text = "B : " + questionList[index].answerB;
        ctx.text = "C : " + questionList[index].answerC;
        dtx.text = "D : " + questionList[index].answerD;
        rightAnswer = questionList[index].rightAnswer;
    }
    public void aBtClick(GameObject sender)
    {
        chooseAnswer = "A";
        changeBtState(false, sender);
        JudgeAnswer();
    }
    public void bBtClick(GameObject sender)
    {
        chooseAnswer = "B";
        changeBtState(false, sender);
        JudgeAnswer();
    }
    public void cBtClick(GameObject sender)
    {
        chooseAnswer = "C";
        changeBtState(false, sender);
        JudgeAnswer();
    }
    public void dBtClick(GameObject sender)
    {
        chooseAnswer = "D";
        changeBtState(false, sender);
        JudgeAnswer();
    }
    public void changeBtState(bool state, GameObject sender = null)
    {
        if (state)
        {
            foreach(Button bt in answerBts)
            {
                bt.enabled = true;
                bt.gameObject.GetComponent<Image>().color = initcolor;
            }
        }
        else
        {
            foreach(Button bt in answerBts)
            {
                bt.enabled = false;
            }
            sender.GetComponent<Image>().color = choosecolor;
        }
    }
    public void JudgeAnswer()
    {
        index++;
        resultObj.SetActive(true);
        righttx.text = string.Format(Singleton<Localization>.Instance.GetText("right_answer"), rightAnswer);
        if (chooseAnswer.Equals(rightAnswer))
        {
            rightCount++;
            resultObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/other/right");
        }
        else
        {
            resultObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/other/error");
        }
        if (index == questionList.Count)
        {
            time.Stop();
            completeObj.SetActive(true);
            int currentscore = JudgeGrade();
            int id;
            int.TryParse(info.id, out id);
            gradetx.text = currentscore.ToString();
            usetimetx.text = ((int)time.f_CurTime).ToString() + "S";
            if (currentscore> maxscore)
            {
                maxgradetx.text = currentscore.ToString();
                updateScore(currentscore);
            }
            else
            {
                maxgradetx.text =maxscore.ToString();
            }
            for (int i = 0; i < CalculateStars(currentscore); i++)
            {
                stars[i].SetActive(true);
            }
        }

    }
    public int JudgeGrade()
    {
        int result = (int)(((float)rightCount / (float)questionList.Count )*100);
        return result;
    }
    private int CalculateStars(int grade)
    {
        int star;
        if (grade >= 90)
        {
            star = 3;
        }
        else if (grade >= 70)
        {
            star = 2;
        }
        else if (grade >= 50)
        {
            star = 1;
        }
        else
        {
            star = 0;
        }
        return star;
    }

    public void updateScore(int score)
    {
        PaperScoreReq req = new PaperScoreReq();
        req.id = info.id;
        req.score = score;
        req.type = paperType;
        HttpClient.getInstnce().ReadSend<PaperScoreReq, PaperScoreResp>(req, "AnswerController,updateScore", resp =>
         {
             int id;
             int.TryParse(info.id, out id);
             HttpClient.getInstnce().scorelist.scoreList[id - 1] = resp.score;
         }, null);
    }
    public void nextBtClick(GameObject sender)
    {
        loadQuestion();
        changeBtState(true);
        resultObj.SetActive(false);
    }
    public void homepageBtClick(GameObject sender)
    {
        changeBtState(true);
        Singleton<ContextManager>.Instance.Pop();
    }
    public void backBtClick(GameObject sender)
    {
        changeBtState(true);
        Singleton<ContextManager>.Instance.Pop();
    }

    void InitLocalPaperList()
    {
        questionList = new List<Questiondetail>();
        List<string[]> strList = GetXML.getInstance().LoadData(xmlName);
        foreach (string[] str in strList)
        {
            Questiondetail one = new Questiondetail(str[0], str[1], str[2], str[3], str[4], str[5],str[6]);
            questionList.Add(one);
        }
        loadQuestion();
    }
    void InitNetPaperList()
    {
        QuestionListReq req = new QuestionListReq();
        req.paperName = xmlName;
        req.id = info.id;
        HttpClient.getInstnce().ReadSend<QuestionListReq, QuestionListResp>(req, "AnswerController,GetQuestionList", resp =>
        {
            questionList = resp.questionList;
            maxscore = resp.maxscore;
            loadQuestion();
        }, null);
        
    }
}
