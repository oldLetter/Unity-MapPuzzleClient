using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtItem :MonoBehaviour {

    [SerializeField]
    private GameObject lockObj;
    [SerializeField]
    private Text btTitle;

    public void Initialize(string index)
    {
        this.gameObject.GetComponent<Button>().enabled = false;
        btTitle.text = string.Format(Singleton<Localization>.Instance.GetText("level_index"), index);
    }
    public void ChangeState()
    {
        this.gameObject.GetComponent<Button>().enabled = true;
        lockObj.SetActive(false);
    }
}
