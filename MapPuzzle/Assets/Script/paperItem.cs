using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class paperItem : MonoBehaviour
{
    public Text idtx;
    public Text nametx;
    public Text counttx;

    private PaperInfo info;
    private bool paperType;   //为真表示本地试卷  为假表示网络试卷

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate ()
        {
            OnClick(this.gameObject);
        });
    }
    public void Init(PaperInfo info,bool type)
    {
        this.info = info;
        paperType = type;
        idtx.text = info.id;
        nametx.text = info.name;
        counttx.text = info.count;
        if (info.id.Equals("0"))
        {
            idtx.text = "";
            this.GetComponent<Button>().enabled = false;
        }
        else
        {
            this.GetComponent<Button>().enabled = true;
        }
    }
    public void OnClick(GameObject sender)
    {
        int id;
        int.TryParse(info.id, out id);
        if (paperType)
        {
            Singleton<ContextManager>.Instance.Push(new AnswerGameContext(),new object[3] {info,paperType,HttpClient.getInstnce().scorelist.scoreList[id-1]});
        }
        else
        {
            Singleton<ContextManager>.Instance.Push(new AnswerGameContext(), new object[2] { info, paperType });
        }
    }
}
