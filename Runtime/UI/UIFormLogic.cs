using UnityEngine;

namespace SimpleFramework
{
    /// <summary>
    /// �����߼���
    /// </summary>
    [RequireComponent(typeof(UIForm))]
    public abstract class UIFormLogic : MonoBehaviour
    {
        private bool m_Visible = false;
        private UIForm m_UIForm = null;

        /// <summary>
        /// ��ȡ����
        /// </summary>
        public UIForm UIForm
        {
            get => m_UIForm;
        }

        /// <summary>
        /// ��ȡ�����ý�������
        /// </summary>
        public string Name
        {
            get => gameObject.name;
            set => gameObject.name = value;
        }

        /// <summary>
        /// ��ȡ�����ý����Ƿ�ɼ�
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
        /// ��ʼ������
        /// </summary>
        protected internal virtual void OnInit(object userData)
        {
            m_UIForm = GetComponent<UIForm>();
        }

        /// <summary>
        /// �򿪽���
        /// </summary>
        protected internal virtual void OnOpen(object userData)
        {
            Visible = true;
        }

        /// <summary>
        /// �رս���
        /// </summary>
        protected internal virtual void OnClose(object userData)
        {
            Visible = false;
        }

        /// <summary>
        /// ���ý���Ŀɼ���
        /// </summary>
        /// <param name="visible">����Ŀɼ���</param>
        protected virtual void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}