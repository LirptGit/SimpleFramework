using System;

namespace SimpleFramework
{
    /// <summary>
    /// ��Ϸ�߼��¼����ࡣ
    /// </summary>
    public abstract class BaseEventArgs : EventArgs, IReference
    {
        /// <summary>
        /// ��ȡ���ͱ��
        /// </summary>
        public abstract int Id
        {
            get;
        }

        /// <summary>
        /// �������á�
        /// </summary>
        public abstract void Clear();
    }
}

