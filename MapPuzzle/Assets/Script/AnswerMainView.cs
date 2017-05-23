using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerMainContext : BaseContext
{
    public AnswerMainContext()
        : base(UIType.AnswerMainView)
    {

    }
}

public class PaperInfo
{
    public string id;
    public string name;
    public string paperName;
    public string count;
    public PaperInfo(string Id,string PaperName,string Name,string Count)
    {
        id = Id;
        paperName = PaperName;
        name = Name;
        count = Count;
    }
}
public class AnswerMainView : AnimateView
{
    public GridScroller _gridScroller;
    private static string localPath;
    private WWW www;
    private string xmlName = "PaperList";
    private List<PaperInfo> paperList = new List<PaperInfo>();

    public override void OnEnter(BaseContext context, object[] obj = null)
    {
        base.OnEnter(context);
        if (paperList.Count > 0)
        {
            paperList.Clear();
        }
        InitPaperList();
    }

    public override void OnExit(BaseContext context)
    {
        base.OnExit(context);
    }
    void InitPaperList()
    {
        List<string[]> strList = GetXML.getInstance().LoadData(xmlName);
        foreach (string[] str in strList)
        {
            PaperInfo info = new PaperInfo(str[0], str[1],str[2], str[3]);
            paperList.Add(info);
        }
        _gridScroller.Init(OnChange, paperList.Count, new Vector2(0.12f, 1f));
    }

    public void backBtClick(GameObject sender)
    {
        Singleton<ContextManager>.Instance.Pop();
    }

    public void OnChange(Transform trans, int index)
    {
        trans.GetComponent<paperItem>().Init(paperList[index]);
    }

}

