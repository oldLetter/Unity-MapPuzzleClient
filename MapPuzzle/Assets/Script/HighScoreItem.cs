﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  
 *
 *	by Xuanyi
 *
 */
	public class HighScoreItem : MonoBehaviour {

	    public void Init(int index)
        {
            transform.Find("Text").GetComponent<Text>().text = index.ToString();
        }

	}