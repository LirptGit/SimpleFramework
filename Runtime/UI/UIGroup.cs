using UnityEngine;
using System.Collections.Generic;

namespace SimpleFramework 
{
    public class UIGroup : MonoBehaviour
    {
        private string m_Name;
        private List<UIForm> uiFormList = new List<UIForm>();

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public string Name
        {
            get => m_Name;
        }

        public void Initialize(string name)
        {
            m_Name = name;
        }

        /// <summary>
        /// �������Ƿ���ڽ���
        /// </summary>
        /// <param name="uiFormAssetName">������Դ����</param>
        /// <returns></returns>
        public bool HasUIForm(string uiFormAssetName)
        {
            foreach (UIForm uiForm in uiFormList)
            {
                if (uiForm.UIFormAssetName == uiFormAssetName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ӽ��浽��������
        /// </summary>
        /// <param name="uiFormAssetName">������Դ</param>
        public void AddUIForm(UIForm uiFormAsset)
        {
            if (!uiFormList.Contains(uiFormAsset))
            {
                uiFormList.Add(uiFormAsset);
            }
        }

        /// <summary>
        /// �ӽ������л�ȡ����
        /// </summary>
        /// <param name="uiFormAssetName">������Դ����</param>
        /// <returns>Ҫ��ȡ�Ľ���</returns>
        public UIForm GetUIForm(string uiFormAssetName)
        {
            foreach (UIForm uiForm in uiFormList)
            {
                if (uiForm.UIFormAssetName == uiFormAssetName)
                {
                    return uiForm;
                }
            }
            return null;
        }

        public List<UIForm> GetAllUIForm()
        {
            return uiFormList;
        }

        public void RemoveUIForm(UIForm uiFormAsset) 
        {
            if (uiFormList.Contains(uiFormAsset))
            {
                uiFormList.Remove(uiFormAsset);
            }
        }
    }
}