using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerGameContext : BaseContext
{
    public AnswerGameContext() : base(UIType.AnswerGameView) { }
}
public class Paperdetail
{
    public int id;
    public string title;
    public string answerA;
    public string answerB;
    public string answerC;
    public string answerD;
    public string rightAnswer;
    public Paperdetail(string Id,string Title, string AnswerA, string AnswerB, string AnswerC, string AnswerD,string RightAnswer)
    {
        int.TryParse(Id, out id);
        title = Title;
        answerA = AnswerA;
        answerB = AnswerB;
        answerC = AnswerC;
        answerD = AnswerD;
        rightAnswer = RightAnswer;
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
    public List<GameObject> stars;
    public GameObject resultObj;
    public GameObject completeObj;

    private string rightAnswer;
    private string chooseAnswer;
    private static string localPath;
    private WWW www;
    private string xmlName ;
    private List<Paperdetail> questionList = new List<Paperdetail>();
    private int index ;
    private int rightCount;
    private Timer time;
    public override void OnEnter(BaseContext context, object[] obj = null)
    {
        base.OnEnter(context);
        if (questionList.Count > 0)
        {
            questionList.Clear();
        }
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
            PaperInfo info = (PaperInfo)obj[0];
            xmlName =info.paperName;
            InitPaperList();
            time.Start();
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
        JudgeAnswer();
    }
    public void bBtClick(GameObject sender)
    {
        chooseAnswer = "B";
        JudgeAnswer();
    }
    public void cBtClick(GameObject sender)
    {
        chooseAnswer = "C";
        JudgeAnswer();
    }
    public void dBtClick(GameObject sender)
    {
        chooseAnswer = "D";
        JudgeAnswer();
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
            gradetx.text = JudgeGrade().ToString();
            usetimetx.text = ((int)time.f_CurTime).ToString() + "S";
            for (int i = 0; i < CalculateStars(JudgeGrade()); i++)
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
    public void nextBtClick(GameObject sender)
    {
        loadQuestion();
        resultObj.SetActive(false);
    }
    public void homepageBtClick(GameObject sender)
    {
        Singleton<ContextManager>.Instance.Pop();
    }
    public void backBtClick(GameObject sender)
    {
        Singleton<ContextManager>.Instance.Pop();
    }

    void InitPaperList()
    {
        List<string[]> strList = GetXML.getInstance().LoadData(xmlName);
        foreach (string[] str in strList)
        {
            Paperdetail one = new Paperdetail(str[0], str[1], str[2], str[3], str[4], str[5],str[6]);
            questionList.Add(one);
        }
        loadQuestion();
    }
}
