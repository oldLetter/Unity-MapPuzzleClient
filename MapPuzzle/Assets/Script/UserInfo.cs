using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

[ProtoContract]
public class UserInfo {
    [ProtoMember(1)]
    public string acount;
    [ProtoMember(2)]
    public string id;
    [ProtoMember(3)]
    public int currentLevel;
    [ProtoMember(4)]
    public int maxLevel;
    [ProtoMember(5)]
    public int hintCount;
}
[ProtoContract]
public class UserInfoReq
{
    [ProtoMember(1)]
    public string acount;
}
