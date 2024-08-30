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
        /// 订阅事件处理回调函数
        /// </summary>
        /// <param name="id">事件类型编号</param>
        /// <param name="handler">要订阅的事件处理回调函数</param>
        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (!m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers.Add(id, handler);
            }
        }

        /// <summary>
        /// 取消订阅事件处理回调函数
        /// </summary>
        /// <param name="id">事件类型编号</param>
        /// <param name="handler">要取消订阅的事件处理回调函数</param>
        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (!m_EventHandlers.Remove(id))
            {
                return;
            }
        }

        /// <summary>
        /// 抛出事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件内容</param>
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
