using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProtoBuf;

public class AnswerMainContext : BaseContext
{
    public AnswerMainContext()
        : base(UIType.AnswerMainView)
    {

    }
}
[ProtoContract]
public class PaperScoreReq
{
    [ProtoMember(1)]
    public string id;
    [ProtoMember(2)]
    public int score;
    [ProtoMember(3)]
    public bool type;
}
[ProtoContract]
public class PaperScoreResp
{
    [ProtoMember(1)]
    public int score;
}
[ProtoContract]
public class PapersScoreResp
{
    [ProtoMember(1)]
    public List<int> scoreList;
    public PapersScoreResp()
    {
        scoreList=new List<int>();
    }
}
[ProtoContract]
public class PaperInfo
{
    [ProtoMember(1)]
    public string id;
    [ProtoMember(2)]
    public string name;
    [ProtoMember(3)]
    public string paperName;
    [ProtoMember(4)]
    public string count;
    public PaperInfo()
    {

    }
    public PaperInfo(string Id,string PaperName,string Name,string Count)
    {
        id = Id;
        paperName = PaperName;
        name = Name;
        count = Count;
    }
}
[ProtoContract]
public class PaperListResp
{
    [ProtoMember(1)]
    public List<PaperInfo> paperlist;
    public PaperListResp()
    {
         paperlist = new List<PaperInfo>();
    }

}
public class AnswerMainView : AnimateView
{
    public GridScroller _gridScroller;
    public Text papertx;

    private static string localPath;
    private string xmlName = "PaperList";
    private bool paperType =true;  //为真表示本地试卷  为假表示网络试卷
    List<PaperInfo> paperList = null;

    public override void OnEnter(BaseContext context, object[] obj = null)
    {

        base.OnEnter(context);
        if (paperType)
        {
            papertx.text= Singleton<Localization>.Instance.GetText("location_paper");
            InitLocationPaperList();
        }
        else
        {
            papertx.text = Singleton<Localization>.Instance.GetText("net_paper");
            InitNetPaperList();
        }
    }

    public override void OnExit(BaseContext context)
    {
        base.OnExit(context);
    }
    void InitLocationPaperList()
    {
        paperList= new List<PaperInfo>();
        List<string[]> strList = GetXML.getInstance().LoadData(xmlName);
        foreach (string[] str in strList)
        {
            PaperInfo info = new PaperInfo(str[0], str[1],str[2], str[3]);
            paperList.Add(info);
        }
        _gridScroller.Init(OnChange, paperList.Count, new Vector2(0.12f, 1f));
    }
    void InitNetPaperList()
    {
        paperList = new List<PaperInfo>();
        DefaultRequest req = new DefaultRequest();
        HttpClient.getInstnce().ReadSend<DefaultRequest, PaperListResp>(req, "AnswerController,GetPaperList", resp =>
         {
             paperList = resp.paperlist;
             _gridScroller.Init(OnChange, paperList.Count, new Vector2(0.12f, 1f));
         }, null);
    }
    public void backBtClick(GameObject sender)
    {
        Singleton<ContextManager>.Instance.Pop();
    }
    public void chooseTypeBtClick(GameObject sender)
    {
        if (paperType)
        {
            paperType = false;
            InitNetPaperList();
            papertx.text = papertx.text = Singleton<Localization>.Instance.GetText("net_paper");
        }
        else
        {
            paperType = true;
            InitLocationPaperList();
            papertx.text = papertx.text = Singleton<Localization>.Instance.GetText("location_paper");
        }
    }
    public void OnChange(Transform trans, int index)
    {
        trans.GetComponent<paperItem>().Init(paperList[index], paperType);
    }

}

