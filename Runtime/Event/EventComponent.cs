using System;
using System.Collections.Generic;

namespace SimpleFramework
{
    public class EventComponent : SimpleFrameworkComponent
    {
        private readonly Dictionary<int, EventHandler<GameEventArgs>> m_EventHandlers = new Dictionary<int, EventHandler<GameEventArgs>>();

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// �����¼�����ص�����
        /// </summary>
        /// <param name="id">�¼����ͱ��</param>
        /// <param name="handler">Ҫ���ĵ��¼�����ص�����</param>
        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (!m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers.Add(id, handler);
            }
        }

        /// <summary>
        /// ȡ�������¼�����ص�����
        /// </summary>
        /// <param name="id">�¼����ͱ��</param>
        /// <param name="handler">Ҫȡ�����ĵ��¼�����ص�����</param>
        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (!m_EventHandlers.Remove(id))
            {
                return;
            }
        }

        /// <summary>
        /// �׳��¼�
        /// </summary>
        /// <param name="sender">�¼�������</param>
        /// <param name="e">�¼�����</param>
        public void Fire(object sender, GameEventArgs e)
        {
            EventHandler<GameEventArgs> eventHandler = null;
            if (m_EventHandlers.TryGetValue(e.Id, out eventHandler))
            {
                eventHandler(sender, e);
            }
            ReferencePool.Release(e);
        }
    }

}
