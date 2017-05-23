using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Coder  {

    public static byte[] IntToBytes(int num)
    {
        byte[] bytes = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            bytes[i] = (byte)(num >> (24 - i * 8));
        }
        return bytes;
    }
    public static int BytesToInt(byte[] data, int offset)
    {
        int num = 0;
        for (int i = offset; i < offset + 4; i++)
        {
            num <<= 8;
            num |= (data[i] & 0xff);
        }
        return num;
    }
    public static byte[] Serialize<T>(T t,string url)
    {
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //使用ProtoBuf工具的序列化方法
                Serializer.Serialize<T>(ms, t);
                //定义二级制数组，保存序列化后的结果
                byte[] result = new byte[ms.Length];
                //将流的位置设为0，起始点
                ms.Position = 0;
                //将流中的内容读取到二进制数组中
                ms.Read(result, 0, result.Length);
                using (var fs = File.Create(Application.dataPath + "/userinfo.bin"))
                {
                    //使用Protobuf序列化文件  
                    Serializer.Serialize<T>(fs, t);
                }
                byte[] urlByt = System.Text.Encoding.Default.GetBytes(url);
                byte[] req = new byte[8 +urlByt.Length+ result.Length];
                Coder.IntToBytes(result.Length + urlByt.Length+4).CopyTo(req, 0);
                Coder.IntToBytes(urlByt.Length).CopyTo(req, 4);
                urlByt.CopyTo(req, 8);
                result.CopyTo(req, 8+urlByt.Length);

                return req;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("序列化失败: " + ex.ToString());
            return null;
        }
    }
    public static T DeSerialize<T>(byte[] msg)
    {
        T instance = default(T);
        try
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(msg, 0, msg.Length);
                ms.Position = 0;
                instance = Serializer.Deserialize<T>(ms);
                return instance;
            }
        }
        catch(Exception ex)
        {
            Debug.Log("反序列化失败: " + ex.ToString());
            return default(T);
        }

    }
}
