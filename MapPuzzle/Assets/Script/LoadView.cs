using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadView :MonoBehaviour {
    public GameObject imgObj;
    public void Update()
    {
        imgObj.GetComponent<RectTransform>().Rotate(Vector3.forward * 3);
    }
}
