using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceInfo
{
    public string Id;
    public string X;
    public string Y;
    public string Name;
    public string Piecename;
    public PieceInfo(string id, string x, string y, string name, string piecename)
    {
        Id = id;
        X = x;
        Y = y;
        Name = name;
        Piecename = piecename;
    }
}

public class MainPlay : MonoBehaviour
{
    [SerializeField]
    private GameObject hintObj;
    List<PieceInfo> pieceInfos = new List<PieceInfo>();
    List<PieceInfo> newList;
    private static string localPath;
    private static string all;
    private string xmlName;
    private string mapPath;
    private SpriteRenderer spriteMap;
    public WWW www;
    Action call;
    int index;
    // Use this for initialization
    void Awake()
    {
        call = OnComplete;
        DataAssign.getInstance().HintCall = showHint;
        spriteMap = GetComponent<SpriteRenderer>();
    }

    public void setData(string xmlname,string mappath)
    {
        mapPath = mappath;
        xmlName = xmlname;
        //StartCoroutine(WWWLoad()); 
        Begin();
        spriteMap.sprite = Resources.Load<Sprite>(mapPath);
    }
    void Begin()
    {
        List<string[]> strList = GetXML.getInstance().LoadData(xmlName);
        foreach (string[] str in strList)
        {
            PieceInfo info = new PieceInfo(str[0], str[1], str[2], str[3], str[4]);
            pieceInfos.Add(info);
        }
        newList = ListRandom.GetRandomList<PieceInfo>(pieceInfos);
        OnComplete();
    }
    public void OnComplete()
    {

        if (index < newList.Count)
        {
            GameObject aloneObj = GameObject.Instantiate(Resources.Load<GameObject>("GamePiecePF/alonePiece")) as GameObject;
            aloneObj.transform.SetParent(this.transform);
            aloneObj.GetComponent<AlonePieceControl>().Initialize(newList[index].Piecename, newList[index].X, newList[index].Y, call);
            DataAssign.getInstance().LoadOnePiece(newList[index].Name);
            index++;
        }
        else
        {
            DataAssign.getInstance().UpdateLevel();
            DataAssign.getInstance().LevelCompletecall();
        }
    }
    public bool showHint()
    {
        if (!hintObj.activeInHierarchy)
        {
            StartCoroutine("showHintIenum");
            return true;
        }
        else
        {
            return false;
        }
        
    }
    public IEnumerator showHintIenum()
    {
        if (!hintObj.activeInHierarchy)
        {
            hintObj.SetActive(true);
            float posx;
            float posy;
            float.TryParse(newList[index - 1].X, out posx);
            float.TryParse(newList[index - 1].Y, out posy);
            hintObj.transform.localPosition = new Vector3(posx,posy, 0);
        }
        yield return new WaitForSeconds(2.0f);
        hintObj.SetActive(false);

    }

}
