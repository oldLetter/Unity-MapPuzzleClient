using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

[ProtoContract]
public class LevelUpReq
{
    [ProtoMember(1)]
    public int currentLevel;
    [ProtoMember(2)]
    public string acount;
}
public class GameManager {
    private static GameManager instance;

    public int currentlevel;
    public int currentDifficult;
    private string chinaXmlname = "ChinaPieceLocation";
    //private string chinaXmlname = "/china.xml";
    private string chinaMappath = "Image/china";
    private string australiaMappath = "Image/australia";
    private string australiaXml = "Australia";
    private string southAmerica1Mappath = "Image/southAmerica";
    private string southAmerica1Xml = "SouthAmerica";

    public static GameManager getInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }

    public void LoadLevel()
    {
        DataAssign.getInstance().UpdateLevel = LevelUp;
        GameObject obj = GameObject.Find("MainPiece");
        if (obj!= null)
        {
            GameObject.DestroyImmediate(obj);
        }
        GameObject chinaObj = GameObject.Instantiate(Resources.Load<GameObject>("GamePiecePF/MainPiece")) as GameObject;
        chinaObj.name = "MainPiece";
        chinaObj.transform.position=new Vector3(-1, 0, 0);
        if (currentlevel == 1)
        {
            chinaObj.GetComponent<MainPlay>().setData(australiaXml, australiaMappath + currentDifficult);
        }
        else if (currentlevel == 2)
        {
            chinaObj.GetComponent<MainPlay>().setData(chinaXmlname, chinaMappath + currentDifficult);
        }
        else if (currentlevel == 3)
        {
            chinaObj.GetComponent<MainPlay>().setData(southAmerica1Xml, southAmerica1Mappath + currentDifficult);
        }
    }
    public void LevelUp()
    {
        if (currentlevel == HttpClient.getInstnce().UserInfo.currentLevel&&currentlevel<HttpClient.getInstnce().UserInfo.maxLevel)
        {
            Debug.Log("level up");
            LevelUpReq req = new LevelUpReq();
            req.currentLevel = currentlevel;
            req.acount = HttpClient.getInstnce().UserInfo.acount;
            HttpClient.getInstnce().UpdateUserInfo<LevelUpReq>(req, "GameController,LevelUp");
        }
    }
}
