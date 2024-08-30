using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleFramework
{
    public class EventListener : EventTrigger
    {
        /// <summary>
        /// 委托事件
        /// </summary>
        /// <param name="ubel"></param>
        public delegate void buttonDelegate(EventListener ubel, PointerEventData eventData);
        /// <summary>
        /// 定义点击事件的委托参数
        /// </summary>
        public buttonDelegate onPointEnter;
        public buttonDelegate onPointExit;
        public buttonDelegate onPointDown;
        public buttonDelegate onPointUp;
        public buttonDelegate onClick;
        public buttonDelegate onDrag;

        /// <summary>
        /// 得到“监听器”组件
        /// </summary>
        /// <param name="go">监听的游戏对象</param>
        public static EventListener Get(GameObject go)
        {
            EventListener lister = go.GetComponent<EventListener>();
            if (lister == null)
            {
                lister = go.AddComponent<EventListener>();
            }
            return lister;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onPointEnter != null)
            {
                onPointEnter(this, eventData);
            }
        }

        /// <summary>
        /// 结束
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onPointExit != null)
            {
                onPointExit(this, eventData);
            }
        }

        /// <summary>
        /// 鼠标触发按钮事件
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onPointDown != null)
            {
                onPointDown(this, eventData);
            }
        }
        /// <summary>
        /// 鼠标触发按钮事件
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onPointUp != null)
            {
                onPointUp(this, eventData);
            }
        }

        /// <summary>
        /// 点击按钮事件
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
            {
                onClick(this, eventData);
            }
        }

        /// <summary>
        /// 拖拽物体
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
            {
                onDrag(this, eventData);
            }
        }
    }
}

