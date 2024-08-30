using UnityEngine;

namespace SimpleFramework
{
    /// <summary>
    /// 界面逻辑类
    /// </summary>
    [RequireComponent(typeof(UIForm))]
    public abstract class UIFormLogic : MonoBehaviour
    {
        private bool m_Visible = false;
        private UIForm m_UIForm = null;

        /// <summary>
        /// 获取界面
        /// </summary>
        public UIForm UIForm
        {
            get => m_UIForm;
        }

        /// <summary>
        /// 获取或设置界面名称
        /// </summary>
        public string Name
        {
            get => gameObject.name;
            set => gameObject.name = value;
        }

        /// <summary>
        /// 获取或设置界面是否可见
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                if (m_Visible == value)
                {
                    return;
                }

                m_Visible = value;

                InternalSetVisible(value);
            }
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        protected internal virtual void OnInit(object userData)
        {
            m_UIForm = GetComponent<UIForm>();
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        protected internal virtual void OnOpen(object userData)
        {
            Visible = true;
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        protected internal virtual void OnClose(object userData)
        {
            Visible = false;
        }

        /// <summary>
        /// 设置界面的可见性
        /// </summary>
        /// <param name="visible">界面的可见性</param>
        protected virtual void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}