using UnityEngine;

namespace SimpleFramework 
{
    public class UIForm : MonoBehaviour
    {
        private string m_UIFormAssetName;
        private UIGroup m_UIGroup;
        private UIFormLogic m_UIFormLogic;

        /// <summary>
        /// 获取界面资源名称
        /// </summary>
        public string UIFormAssetName
        {
            get => m_UIFormAssetName;
        }

        /// <summary>
        /// 获取界面所属的界面组
        /// </summary>
        public UIGroup UIGroup
        {
            get => m_UIGroup;
        }

        /// <summary>
        /// 获取界面逻辑
        /// </summary>
        public UIFormLogic Logic
        {
            get => m_UIFormLogic;
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void OnInit(string uiFormAssetName, UIGroup uiGroup, object userData)
        {
            m_UIFormAssetName = uiFormAssetName;
            m_UIGroup = uiGroup;
            m_UIFormLogic = GetComponent<UIFormLogic>();
            if (m_UIFormLogic == null)
            {
                Debug.LogError("UI Form " + uiFormAssetName + "can not get ui form logic");
                return;
            }
            m_UIFormLogic.OnInit(userData);
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        public void OnOpen(object userData)
        {
            m_UIFormLogic.OnOpen(userData);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        public void OnClose(object userData)
        {
            m_UIFormLogic.OnClose(userData);
        }
    }
}