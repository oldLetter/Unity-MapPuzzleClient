using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataAssign  {

    private Action levelCompletecall;
    private Action updateLevel;
    private Action<string> loadOnePiece;
    private Func<bool> hintCall;
    private  static DataAssign instance = null;

    public Action LevelCompletecall
    {
        get
        {
            return levelCompletecall;
        }

        set
        {
            levelCompletecall = value;
        }
    }

    public Action UpdateLevel
    {
        get
        {
            return updateLevel;
        }

        set
        {
            updateLevel = value;
        }
    }

    public Action<string> LoadOnePiece
    {
        get
        {
            return loadOnePiece;
        }

        set
        {
            loadOnePiece = value;
        }
    }

    public Func<bool> HintCall
    {
        get
        {
            return hintCall;
        }

        set
        {
            hintCall = value;
        }
    }

    private DataAssign() { }

    public static DataAssign getInstance()
    {
        if (instance == null)
        {
            instance = new DataAssign();
        }
        return instance;
    }

}
