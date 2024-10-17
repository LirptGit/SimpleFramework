using System;

namespace SimpleFramework
{
    /// <summary>
    /// �������
    /// </summary>
    public abstract class ObjectBase : IReference
    {
        private object m_Target;
        private DateTime m_LastUseTime;

        /// <summary>
        /// ��ʼ������������ʵ��
        /// </summary>
        public ObjectBase()
        {
            m_Target = null;
            m_LastUseTime = default(DateTime);
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        public object Target
        {
            get
            {
                return m_Target;
            }
        }

        /// <summary>
        /// ��ȡ�����ϴ�ʹ��ʱ��
        /// </summary>
        public DateTime LastUseTime
        {
            get
            {
                return m_LastUseTime;
            }
            internal set
            {
                m_LastUseTime = value;
            }
        }


        /// <summary>
        /// ��ʼ���������
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="target">����</param>
        /// <param name="locked">�����Ƿ񱻼���</param>
        /// <param name="priority">��������ȼ�</param>
        protected void Initialize(object target)
        {
            m_Target = target;
            m_LastUseTime = DateTime.UtcNow;
        }

        public virtual void Clear()
        {
            m_Target = null;
            m_LastUseTime = default(DateTime);
        }

        /// <summary>
        /// ��ȡ����ʱ���¼�
        /// </summary>
        protected internal virtual void OnSpawn()
        {
        }

        /// <summary>
        /// ���ն���ʱ���¼�
        /// </summary>
        protected internal virtual void OnUnspawn()
        {
        }

        /// <summary>
        /// �ͷŶ���
        /// </summary>
        /// <param name="isShutdown">�Ƿ��ǹرն����ʱ����</param>
        protected internal abstract void Release(bool isShutdown);
    }
}

