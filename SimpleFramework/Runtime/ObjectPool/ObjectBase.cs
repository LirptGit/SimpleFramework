using System;

namespace SimpleFramework
{
    /// <summary>
    /// 对象基类
    /// </summary>
    public abstract class ObjectBase : IReference
    {
        private object m_Target;
        private DateTime m_LastUseTime;

        /// <summary>
        /// 初始化对象基类的新实例
        /// </summary>
        public ObjectBase()
        {
            m_Target = null;
            m_LastUseTime = default(DateTime);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public object Target
        {
            get
            {
                return m_Target;
            }
        }

        /// <summary>
        /// 获取对象上次使用时间
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
        /// 初始化对象基类
        /// </summary>
        /// <param name="name">对象名称</param>
        /// <param name="target">对象</param>
        /// <param name="locked">对象是否被加锁</param>
        /// <param name="priority">对象的优先级</param>
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
        /// 获取对象时的事件
        /// </summary>
        protected internal virtual void OnSpawn()
        {
        }

        /// <summary>
        /// 回收对象时的事件
        /// </summary>
        protected internal virtual void OnUnspawn()
        {
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="isShutdown">是否是关闭对象池时触发</param>
        protected internal abstract void Release(bool isShutdown);
    }
}

