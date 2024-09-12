using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Text;
using System.Reflection;

namespace EditorExpand.HierarchyExpand
{
    /// <summary>
    /// 彩虹 Hierarchy
    /// </summary>
    [InitializeOnLoad]
    public class RainbowHierarchy : MonoBehaviour
    {
        static RainbowHierarchy()
        {

        }



        [Tooltip("配置文件夹路径")]
        public static string fileDataPath = "Assets/RPTMain/Res/ScriptableObject";

        [InitializeOnLoadMethod]
        private static void RainbowView()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnRainbowGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnRainbowButton;
        }

        private static void OnRainbowGUI(int instanceID, Rect selectionRect)
        {
            // Debug.Log($"{instanceID} {selectionRect}");

            UnityEngine.Object target = EditorUtility.InstanceIDToObject(instanceID);
            if (target == null) return;


#if UNITY_2020_1_OR_NEWER
            var globalID = GlobalObjectId.GetGlobalObjectIdSlow(target).targetObjectId;
#elif UNITY_2018_1
            var globalID =  GetLocalIdentifierInFile(target);
#endif

            // 获取配置数据表
            var fileName = EditorSceneManager.GetActiveScene().name;
            RainbowHierarchyConfig config = GetScriptableObject(fileName);

            if (config == null) return;

            List<HierarchyConfig> rainbowList = config.RainbowList;
            var unitConfig = rainbowList.Find(e => e.ID == globalID);

            if (unitConfig == null) return;

            #region 应用配置
            Texture2D _Bg = CreateDefaultTexture2D(unitConfig);
            GUIStyle FontStyle = CreateGUIStyle(_Bg, unitConfig);
            #endregion

            EditorGUI.LabelField(selectionRect, unitConfig.GameObjectName, FontStyle);
        }

        private static void OnRainbowButton(int instanceID, Rect selectionRect)
        {
            // 清除Scene
            UnityEngine.Object target = EditorUtility.InstanceIDToObject(instanceID);
            if (target == null) return;
     
        }



        #region Menu - GameObject
        [MenuItem("GameObject/彩虹 Hierarchy/添加彩虹", priority = 1)]
        private static void AddRainbowGUI()
        {
            // 获取 ScriptableObejct
            var fileName = GetFilePath(EditorSceneManager.GetActiveScene().name);
            var config = AssetDatabase.LoadAssetAtPath<RainbowHierarchyConfig>(fileName);

            if (config == null)
            {
                config = CreateScriptableObejct(EditorSceneManager.GetActiveScene().name);
            }

            var globalID = GlobalObjectId.GetGlobalObjectIdSlow(Selection.activeObject).targetObjectId;

            var rainbowList = config.RainbowList;
            var unitConfig = rainbowList.Find(e => e.ID == globalID);

            if (unitConfig == null)
            {
                HierarchyConfig newConfig = new()
                {
                    GameObjectName = Selection.activeGameObject.name,
                    ID = globalID,
                };

                rainbowList.Add(newConfig);
            }
            else
            {
                unitConfig.GameObjectName = Selection.activeGameObject.name;
            }

            EditorUtility.SetDirty(config);
        }

        [MenuItem("GameObject/彩虹 Hierarchy/移除彩虹", priority = 2)]
        private static void RemoveRainbowGUI()
        {
            //var localID = GetLocalIdentifierInFile(Selection.activeGameObject);
            var globalID = GlobalObjectId.GetGlobalObjectIdSlow(Selection.activeObject).targetObjectId;


            // 获取ScriptableObject配置文件
            RainbowHierarchyConfig config = GetScriptableObject(Selection.activeGameObject.scene.name);

            var rainbowList = config.RainbowList;
            var unitConfig = rainbowList.Find(e => e.ID == globalID);

            if (unitConfig != null)
            {
                rainbowList.Remove(unitConfig);
            }

            EditorUtility.SetDirty(config);
        }

        [MenuItem("GameObject/彩虹 Hierarchy/清除彩虹", priority = 3)]
        private static void ClearAll()
        {
            RainbowHierarchyConfig config = GetScriptableObject(Selection.activeGameObject.scene.name);
            if (config == null) return;

            var rainbowList = config.RainbowList;
            rainbowList.Clear();

            EditorUtility.SetDirty(config);
        }
        #endregion

        #region Menu - AssetDatabase
        private static void CreateFolder()
        {
            string[] path = fileDataPath.Split('/');

            if (!path[0].Equals("Assets"))
            {
                Debug.Log("[Error Create] You must use \"Assets\" to start filePath");
                return;
            }

            string parentPath = path[0];
            string folderName;

            for (int i = 1; i < path.Length; i++)
            {
                folderName = path[i];

                if (!AssetDatabase.IsValidFolder($"{parentPath}/{folderName}"))
                {
                    AssetDatabase.CreateFolder(parentPath, folderName);
                }

                parentPath = $"{parentPath}/{folderName}";
            }

            AssetDatabase.Refresh();
        }

        private static void InitScriptObject()
        {
            string dataPath = GetFilePath(Selection.activeGameObject.scene.name);

            string[] assets = AssetDatabase.FindAssets(dataPath);

            if (assets.Length.Equals(0))
            {
                var config = ScriptableObject.CreateInstance<RainbowHierarchyConfig>();

                AssetDatabase.CreateAsset(config, dataPath);
                //AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region others

        private static Texture2D CreateDefaultTexture2D(HierarchyConfig config)
        {
            Texture2D _Bg = new(1, 1, TextureFormat.RGBAFloat, false);
            _Bg.SetPixel(0, 0, config.BackgroundColor);
            _Bg.Apply();

            return _Bg;
        }

        private static GUIStyle CreateGUIStyle(Texture2D backgorund, HierarchyConfig config)
        {
            GUIStyle FontStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter
            };

            FontStyle.normal.background = backgorund;
            FontStyle.normal.textColor = config.FontColor;
            FontStyle.fontSize = config.FontSize;
            FontStyle.fontStyle = UnityEngine.FontStyle.Bold;

            return FontStyle;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        /// <param name="name">文件名</param>
        /// <returns></returns>
        private static string GetFilePath(string name)
        {
            // 备注：未作 name 是否为空的判断，因引用不确定性，应在调用此方法前进行判断分析。
            return $"{fileDataPath}/Rainbow_{name}";
        }

        /// <summary>
        /// ScriptObject RainbowHierarchy数据获取
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件路径（默认为NULL，若期望使用其他路径，传入新文件夹路径地址即可）</param>
        /// <returns></returns>
        private static RainbowHierarchyConfig GetScriptableObject(string fileName, string filePath = null)
        {
            // 资源文件路径地址
            filePath ??= GetFilePath(fileName + ".asset");

            // 检查资源文件夹目录是否存在
            if (!AssetDatabase.IsValidFolder(fileDataPath))
            {
                CreateFolder();
            }

            // 加载路径文件
            RainbowHierarchyConfig config = AssetDatabase.LoadAssetAtPath<RainbowHierarchyConfig>(filePath);

            // 有则返回，无则创建
            config = config != null ? config : CreateScriptableObejct(fileName, filePath);

            return config;
        }

        /// <summary>
        /// ScriptableObject RainbowHeirarchyConfig 创建
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件路径（默认为NULL，若期望使用其他路径，传入新文件夹路径地址即可）</param>
        /// <returns></returns>
        private static RainbowHierarchyConfig CreateScriptableObejct(string fileName, string filePath = null)
        {
            // 资源路径地址
            filePath ??= GetFilePath(fileName + ".asset");

            var config = AssetDatabase.LoadAssetAtPath<RainbowHierarchyConfig>(filePath);

            if (config == null)
            {
                var instance = ScriptableObject.CreateInstance<RainbowHierarchyConfig>();

                AssetDatabase.CreateAsset(instance, filePath);
            }

            AssetDatabase.Refresh();

            return config;
        }

        /// <summary>
        /// 获取本地文件GUID
        /// </summary>
        private static int GetLocalIdentifierInFile(GameObject target)
        {
            PropertyInfo insepectorInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            SerializedObject serializedObject = new SerializedObject(target);
            insepectorInfo.SetValue(serializedObject, InspectorMode.Debug, null);
            SerializedProperty localIdentifierInFile = serializedObject.FindProperty("m_LocalIdentfierInFile");

            return localIdentifierInFile.intValue;
        }
        #endregion
    }
}
