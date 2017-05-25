using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBtItem :RepeatPressEventTrigger {

    [SerializeField]
    private GameObject lockObj;
    [SerializeField]
    private Text btTitle;

    public void Initialize(string index,object obj)
    {
        onLongPress = null;
        onShortPress = null;
        this.obj = obj;
        this.gameObject.GetComponent<Button>().enabled = false;
        lockObj.SetActive(true);
        btTitle.text = string.Format(Singleton<Localization>.Instance.GetText("level_index"), index);
    }
    public void ChangeState()
    {
        this.gameObject.GetComponent<Button>().enabled = true;
        lockObj.SetActive(false);
    }
}
