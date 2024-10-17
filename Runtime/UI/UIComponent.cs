using UnityEngine;
using System.Collections.Generic;

namespace SimpleFramework
{
    public partial class UIComponent : SimpleFrameworkComponent
    {
        [Header("UI Instances Root")]
        [SerializeField] private Transform m_InstanceRoot = null;

        [Header("Resource Loader")]
        [SerializeField] private ResourceLoaderComponent m_Loader = null;

        [Header("AB����Ϣ����")]
        [SerializeField] private LoaderType loaderType;
        [SerializeField] private string abLocalPath;
        [SerializeField] private string abName;
        [SerializeField] private string mainabName;
        [SerializeField] private string assetSuffix;

        [Header("Object Pool")]
        [SerializeField] private ObjectPoolComponent m_Pool = null;
        [SerializeField] private int capacity = 100;
        private IObjectPool<UIObject> uiPool;

        [Header("������ְ��ճ��������ַ���")]
        [SerializeField] private string[] uiGroups = null;

        private readonly List<string> m_isLoading = new List<string>();
        private readonly Dictionary<string, UIGroup> m_UIGroupDict = new Dictionary<string, UIGroup>();
        private readonly List<UIObject> releaseUICache = new List<UIObject>();
        private readonly Queue<UIObject> releaseUIQueue = new Queue<UIObject>();

        /// <summary>
        /// ��Ϸ��������ʼ��
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

            uiPool = m_Pool.CreateObjectPool<UIObject>("UI Pool", capacity);

            for (int i = 0; i < uiGroups.Length; i++)
            {
                AddUIGroup(uiGroups[i]);
            }
        }

        private void Update()
        {
            if (releaseUIQueue.Count > 0)
            {
                UIObject uiObject = releaseUIQueue.Dequeue();
                if (uiPool.ReleaseObject(uiObject))
                {
                    releaseUICache.Remove(uiObject);
                }
            }
        }

        /// <summary>
        /// ��ӽ�����
        /// </summary>
        /// <param name="uiGroupName">����������</param>
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
        /// �Ƿ���ڽ�����
        /// </summary>
        /// <param name="uiGroupName">����������</param>
        /// <returns>�Ƿ���ڽ�����</returns>
        public bool HasUIGroup(string uiGroupName)
        {
            return m_UIGroupDict.ContainsKey(uiGroupName);
        }

        /// <summary>
        /// ��ȡ�����顣
        /// </summary>
        /// <param name="uiGroupName">����������</param>
        /// <returns>Ҫ��ȡ�Ľ�����</returns>
        public UIGroup GetUIGroup(string uiGroupName)
        {
            return m_UIGroupDict[uiGroupName];
        }

        /// <summary>
        /// ��ȡ���档
        /// </summary>
        /// <param name="uiFormAssetName">������Դ����</param>
        /// <returns>Ҫ��ȡ�Ľ���</returns>
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
        /// �򿪽���
        /// </summary>
        /// <param name="abName">ab��������</param>
        /// <param name="uiFormAssetName">������Դ����</param>
        /// <param name="uiGroupName">����������</param>
        /// <param name="userData">����</param>
        public void OpenUIForm(string uiFormAssetName, string uiGroupName, object userData)
        {
            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);

            if (uiGroup == null)
            {
                Debug.LogError($"����UICompontent�ű����Ƿ��������");
            }

            if (uiGroup.HasUIForm(uiFormAssetName))
            {
                uiGroup.GetUIForm(uiFormAssetName).OnOpen(userData);
            }
            else
            {
                if (!m_isLoading.Contains(uiFormAssetName))
                {
                    m_isLoading.Add(uiFormAssetName);

                    LoaderABTask abTask = LoaderABTask.Create(loaderType, CommonPath.GetABPath(abLocalPath), abName, typeof(AudioClip), uiFormAssetName + assetSuffix, mainabName);
                    m_Loader.AddAsset(abTask, (UnityEngine.Object obj) =>
                    {
                        if (obj == null)
                        {
                            Debug.LogError($"ab�����ص�ui����Ϊ�գ�����UICompontent�ű��ϵ�AB����Ϣ�����Ƿ���ȷ");
                        }

                        LoadFinishedCallBack(obj, uiGroup, uiFormAssetName, userData);
                    });
                }
            }
        }

        /// <summary>
        /// �򿪽���
        /// </summary>
        /// <param name="uiPanel">����Ԥ����</param>
        /// <param name="uiGroupName">����������</param>
        /// <param name="userData">����</param>
        public void OpenUIForm<T>(GameObject uiPanel, string uiGroupName, object userData) where T : UIFormLogic
        {
            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);

            if (uiGroup.HasUIForm(uiPanel.name))
            {
                uiGroup.GetUIForm(uiPanel.name).OnOpen(userData);
            }
            else
            {
                LoadFinishedCallBack(uiPanel, uiGroup, uiPanel.name, userData);
            }
        }

        /// <summary>
        /// �رս���
        /// </summary>
        /// <param name="uiFormAssetName">����</param>
        public void CloseUIForm(UIForm uiForm, object userData)
        {
            UIGroup uiGroup = uiForm.UIGroup;

            if (uiGroup.HasUIForm(uiForm.UIFormAssetName))
            {
                uiGroup.GetUIForm(uiForm.UIFormAssetName).OnClose(userData);
            }
        }

        /// <summary>
        /// �ͷŽ�����Դ
        /// </summary>
        /// <param name="uiForm">����</param>
        public void ReleaseUIForm(UIForm uiForm)
        {
            UIGroup uiGroup = uiForm.UIGroup;

            for (int i = 0; i < releaseUICache.Count; i++)
            {
                UIForm curUIForm = releaseUICache[i].Target as UIForm;
                if (curUIForm == uiForm)
                {
                    CloseUIForm(uiForm, null);
                    uiGroup.RemoveUIForm(uiForm);
                    uiPool.Unspawn(releaseUICache[i]);
                    releaseUIQueue.Enqueue(releaseUICache[i]);
                }
            }
        }

        /// <summary>
        /// �ͷŵ�ǰ�������еĽ�����Դ
        /// </summary>
        /// <param name="uiGroupName"></param>
        public void ReleaseUIForm(string uiGroupName) 
        {
            UIGroup uiGroup = (UIGroup)GetUIGroup(uiGroupName);
            for (int i = 0; i < uiGroup.GetAllUIForm().Count; i++)
            {
                ReleaseUIForm(uiGroup.GetAllUIForm()[i]);
            }
        }

        private void LoadFinishedCallBack(UnityEngine.Object obj, UIGroup uiGroup, string uiFormAssetName, object userData)
        {
            GameObject go = Instantiate(obj, uiGroup.transform) as GameObject;
            UIForm uiForm = go.GetComponent<UIForm>();
            CreateUIObject(uiForm);

            uiForm.OnInit(uiFormAssetName, uiGroup, userData);
            uiForm.OnOpen(userData);

            uiGroup.AddUIForm(uiForm);

            m_isLoading.Remove(uiFormAssetName);
        }

        /// <summary>
        /// �����ͻ�ȡSoundObject
        /// </summary>
        /// <returns></returns>
        private UIObject CreateUIObject(UIForm uiForm)
        {
            UIObject uiObject = uiPool.Spawn();
            if (uiObject == null)
            {
                uiObject = UIObject.Create(uiForm);

                if (!uiPool.Register(uiObject, true))
                {
                    Destroy(gameObject);
                    ReferencePool.Release(uiObject);
                    uiObject = null;
                }
                releaseUICache.Add(uiObject);
            }
            return uiObject;
        }
    }
}

