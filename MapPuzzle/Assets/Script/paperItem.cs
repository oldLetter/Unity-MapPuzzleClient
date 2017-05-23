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

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate ()
        {
            OnClick(this.gameObject);
        });
    }
    public void Init(PaperInfo info)
    {
        this.info = info;
        idtx.text = info.id;
        nametx.text = info.name;
        counttx.text = info.count;
    }
    public void OnClick(GameObject sender)
    {
        Singleton<ContextManager>.Instance.Push(new AnswerGameContext(),new object[1] {info});
    }
}
