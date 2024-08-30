using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ExItem
{
    public uint itemId;  // uint为无符号整型
                         // 可以自定义所需数据的类型
                         // public int itemID;
                         // public string itemName;
    public List<string> itemData;
}

public class ItemManager : ScriptableObject
{
    public string Desc = "该配置是自动生成的，请勿手动修改！";
    public ExItem[] dataArray;
}

public class ExCell
{
    public string name;
    public string info;
}