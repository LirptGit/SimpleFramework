using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace EditorExpand.HierarchyExpand
{
    [CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Rainbow Hierarchy Config")]
    public class RainbowHierarchyConfig : ScriptableObject
    {
        [Header("GUID")]
        public string scriptableObjectGUID;

        [Header("参考颜色")]
        public List<Color> NormalColor = new List<Color>()
        {
            Color.black,
            Color.blue,
            Color.red,
            Color.white,
            Color.yellow,
            Color.green,
            Color.grey,
            Color.gray
        };

        [Header("配置清单")]
        public List<HierarchyConfig> RainbowList;
    }



    [System.Serializable]
    public class HierarchyConfig
    {
        [Header("Dont Change Anything!")]
        [Tooltip("Dont Change this Value!")]
        public ulong ID;
        public string GameObjectName;

        [Header("Config")]
        public Color FontColor = Color.white;
        public int FontSize = 15;

        public Color BackgroundColor = Color.black;

        public Rect InstanceRect;
    }
}
