using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlonePieceControl : MonoBehaviour {
    private SpriteRenderer sprite;
    private float tarX;
    private float tarY;
    private Vector3 tarPosi;
    private Vector3 initialPosi;
    private float mindistance = 1;

    Action fnishcall;

	void Awake () {
        initialPosi = new Vector3(7, -3, 0);
        sprite = GetComponent<SpriteRenderer>();
        
    }

    public void Initialize(string path,string x,string y,Action call)
    {
        fnishcall = call;
        this.gameObject.transform.localPosition = initialPosi;
        string spritePath = "Image/" + path;
        sprite.sprite = Resources.Load<Sprite>(spritePath);
        float.TryParse(x,out tarX);
        float.TryParse(y, out tarY);
        tarPosi = new Vector3(tarX, tarY);
        //StartCoroutine("OnMouseDown");

    }
    IEnumerator OnMouseDown()
    {
        Vector3 ScreenSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenSpace.z));

        while (Input.GetMouseButton(0))
        {
            //得到现在鼠标的2维坐标系位置  
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenSpace.z);
            //将当前鼠标的2维位置转化成三维的位置，再加上鼠标的移动量  
            Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            //CurPosition就是物体应该的移动向量赋给transform的position属性        
            transform.position = CurPosition;
            //这个很主要  
            //yield return new WaitForFixedUpdate();
            yield return null;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float distance = Vector3.Distance(tarPosi, transform.localPosition);
            if (distance >=mindistance )
            {
                transform.localPosition = initialPosi;
            }
            else
            {
                transform.localPosition = tarPosi;
                if (PlayerPrefs.GetInt("soundEffect") == 1)
                {
                    AudioController.Instance.SoundPlay("sfx_clr");
                }
                fnishcall();
                StopAllCoroutines();
                Destroy(this.gameObject.GetComponent<AlonePieceControl>());
                //yield return null;
            }
        }
    }
}
