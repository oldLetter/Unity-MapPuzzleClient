using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Define View's Path And Name
 *
 *	by Xuanyi
 *
 */


public class UIType
{

    public string Path { get; private set; }

    public string Name { get; private set; }

    public UIType(string path)
    {
        Path = path;
        Name = path.Substring(path.LastIndexOf('/') + 1);
    }

    public override string ToString()
    {
        return string.Format("path : {0} name : {1}", Path, Name);
    }

    public static readonly UIType StartView = new UIType("View/StartView");
    public static readonly UIType LoginView = new UIType("View/LoginView");
    public static readonly UIType MainView = new UIType("View/MainView");
    public static readonly UIType GameView = new UIType("View/GameView");
    public static readonly UIType AnswerMainView = new UIType("View/AnswerMainView");
    public static readonly UIType AnswerGameView = new UIType("View/AnswerGameView");
}

