using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework
{
    public class UIComponent : SimpleFrameworkComponent 
    {
        [Header("需要使用AB包加载, 请填写ab包文件名字全称")]
        [SerializeField] private string abName;

        [SerializeField] private Transform m_InstanceRoot = null;

        [SerializeField] private ResourceLoaderComponent m_Loader = null;
        [SerializeField] private LoaderType loaderType;

        [SerializeField] private string[] uiGroups = null;

        private readonly List<string> m_isLoading = new();
        private readonly Dictionary<string, UIGroup> m_UIGroupDict = new();

        /// <summary>
        /// 游戏框架组件初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = new GameObject("UI Instances").transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < uiGroups.Length; i++)
            {
                AddUIGroup(uiGroups[i]);
            }
        }

        /// <summary>
        /// 添加界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        private void AddUIGroup(string uiGroupName)
        {
            GameObject uiGroupHelperObj = new GameObject("UI Group - " + uiGroupName);
            uiGroupHelperObj.transform.SetParent(m_InstanceRoot);
            uiGroupHelperObj.transform.localScale = Vector3.one;

            UIGroup uiGroup = uiGroupHelperObj.GetOrAddComponent<UIGroup>();
            uiGroup.Initialize(uiGroupName);

            m_UIGroupDict.Add(uiGroupName, uiGroup);
        }

        /// <summary>
        /// 是否存在界面组
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <returns>是否存在界面组</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            return m_UIGroupDict.ContainsKey(uiGroupName);
        }

        /// <summary>
        /// 获取界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称</param>
        /// <returns>要获取的界面组</returns>
        public UIGroup GetUIGroup(string uiGroupName)
        {
            return m_UIGroupDict[uiGroupName];
        }

        /// <summary>
        /// 获取界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>要获取的界面</returns>
        public UIForm GetUIForm(string uiFormAssetName)
        {
            foreach (KeyValuePair<string, UIGroup> uiGroup in m_UIGroupDict)
            {
                UIForm uiForm = uiGroup.Value.GetUIForm(uiFormAssetName);
                if (uiForm != null)
                {
                    return uiForm;
                }
            }

            return null;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <param name="uiGroupName">界面组名称</param>
        public void OpenUIForm(string uiFormAssetName, string uiGroupName, object userData)
        {
            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);

            if (uiGroup.HasUIForm(uiFormAssetName))
            {
                uiGroup.GetUIForm(uiFormAssetName).OnOpen(userData);
            }
            else
            {
                if (!m_isLoading.Contains(uiFormAssetName))
                {
                    m_isLoading.Add(uiFormAssetName);
                    switch (loaderType)
                    {
                        case LoaderType.Resources:
                            LoadFinishedCallBack(m_Loader.ResourceLoadAsset<GameObject>(uiFormAssetName), uiGroup, uiFormAssetName, userData);
                            break;
                        case LoaderType.Asserbundle:
                            LoadFinishedCallBack(m_Loader.LoadAsset<GameObject>(abName, uiFormAssetName), uiGroup, uiFormAssetName, userData);
                            break;
                        case LoaderType.AsserbundleAsync:
                            m_Loader.LoadAssetAnsy<GameObject>(abName, uiFormAssetName, (UnityEngine.Object obj) =>
                            {
                                LoadFinishedCallBack(obj, uiGroup, uiFormAssetName, userData);
                            });
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="uiFormAssetName">界面</param>
        public void CloseUIForm(UIForm uiForm, object userData)
        {
            UIGroup uiGroup = uiForm.UIGroup;

            if (uiGroup.HasUIForm(uiForm.UIFormAssetName))
            {
                uiGroup.GetUIForm(uiForm.UIFormAssetName).OnClose(userData);
            }
        }

        private void LoadFinishedCallBack(UnityEngine.Object obj, UIGroup uiGroup, string uiFormAssetName, object userData)
        {
            GameObject go = Instantiate(obj, uiGroup.transform) as GameObject;
            UIForm uiForm = go.GetComponent<UIForm>();

            uiForm.OnInit(uiFormAssetName, uiGroup, userData);
            uiForm.OnOpen(userData);

            uiGroup.AddUIForm(uiForm);

            m_isLoading.Remove(uiFormAssetName);
        }
    }
}

