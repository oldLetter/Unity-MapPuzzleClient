using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RepeatPressEventTrigger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float interval = 0.5f; //回调触发间隔时间;  
    public float shorttime = 0.2f;

    public Action<object> onLongPress;
    public Action<object> onShortPress;
    public object obj;
    private bool isPointDown = false;
    private float lastInvokeTime;
    private float currentTime;

    // Use this for initialization  
    void Start()
    {
    }

    // Update is called once per frame  
    void Update()
    {
        if (isPointDown)
        {
            if (Time.time - lastInvokeTime > interval)
            {
                if (onLongPress != null)
                {
                    onLongPress(obj);
                }
                lastInvokeTime = Time.time;
                isPointDown = false;
            }
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointDown = true;
        lastInvokeTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointDown = false;
        currentTime = Time.time;
        if (currentTime - lastInvokeTime <shorttime)
        {
            if (PlayerPrefs.GetInt("soundEffect") == 1&&this.GetComponent<Button>().enabled)
            {
                AudioController.Instance.SoundPlay("sfx_click");
            }
            if (onShortPress != null)
            {
                onShortPress(obj);   
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointDown = false;
    }
}
