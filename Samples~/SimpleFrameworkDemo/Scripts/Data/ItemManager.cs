using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ExItem
{
    public uint itemId;  // uintΪ�޷�������
                         // �����Զ����������ݵ�����
                         // public int itemID;
                         // public string itemName;
    public List<string> itemData;
}

public class ItemManager : ScriptableObject
{
    public string Desc = "���������Զ����ɵģ������ֶ��޸ģ�";
    public ExItem[] dataArray;
}

public class ExCell
{
    public string name;
    public string info;
}