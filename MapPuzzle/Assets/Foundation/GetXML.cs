using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GetXML  {
    //List<T> t;
    private static GetXML instance;
    private static WWW www;
                                                                                                                        
    public static GetXML getInstance()
    {
        if (instance == null)
        {
            instance = new GetXML();
        }
        return instance;
    }

    public List<string[]> LoadData(string filepath)
    {
        string path = "XMLData/";
        List<string[]> list = new List<string[]>();
        path += filepath;
        string _result = Resources.Load(path).ToString();
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(_result);
        //得到objects节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("Table").ChildNodes;
        foreach(XmlNode xml1 in xmlNodeList)
        {
            int i = 0;
            XmlNodeList xml1List = xml1.ChildNodes;
            string[] str = { };
            str = new string[xml1List.Count];
            foreach (XmlNode el in xml1List)//读元素值
            {
                str[i] = el.FirstChild.Value;
                    i++;
            }
            list.Add(str);
        }
        return list;
    }
}
