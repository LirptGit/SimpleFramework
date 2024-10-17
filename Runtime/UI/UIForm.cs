using UnityEngine;

namespace SimpleFramework 
{
    public class UIForm : MonoBehaviour
    {
        private string m_UIFormAssetName;
        private UIGroup m_UIGroup;
        private UIFormLogic m_UIFormLogic;

        /// <summary>
        /// ��ȡ������Դ����
        /// </summary>
        public string UIFormAssetName
        {
            get => m_UIFormAssetName;
        }

        /// <summary>
        /// ��ȡ���������Ľ�����
        /// </summary>
        public UIGroup UIGroup
        {
            get => m_UIGroup;
        }

        /// <summary>
        /// ��ȡ�����߼�
        /// </summary>
        public UIFormLogic Logic
        {
            get => m_UIFormLogic;
        }

        /// <summary>
        /// ��ʼ������
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
        /// �򿪽���
        /// </summary>
        public void OnOpen(object userData)
        {
            m_UIFormLogic.OnOpen(userData);
        }

        /// <summary>
        /// �رս���
        /// </summary>
        public void OnClose(object userData)
        {
            m_UIFormLogic.OnClose(userData);
        }
    }
}