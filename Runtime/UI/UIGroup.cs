using UnityEngine;
using System.Collections.Generic;

namespace SimpleFramework 
{
    public class UIGroup : MonoBehaviour
    {
        private string m_Name;
        private List<UIForm> uiFormList = new List<UIForm>();

        /// <summary>
        /// 获取界面组名称
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
        /// 界面组是否存在界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
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
        /// 添加界面到界面组中
        /// </summary>
        /// <param name="uiFormAssetName">界面资源</param>
        public void AddUIForm(UIForm uiFormAsset)
        {
            if (!uiFormList.Contains(uiFormAsset))
            {
                uiFormList.Add(uiFormAsset);
            }
        }

        /// <summary>
        /// 从界面组中获取界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称</param>
        /// <returns>要获取的界面</returns>
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