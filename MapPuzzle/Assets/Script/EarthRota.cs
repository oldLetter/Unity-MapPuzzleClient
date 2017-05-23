using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRota : MonoBehaviour {
    public float rotaSpeed;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(Vector3.up * rotaSpeed);
    }
}
