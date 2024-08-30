using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleFramework
{
    /// <summary>
    /// Canvas上务必添加 GraphicRaycaster 组件
    /// </summary>
    [RequireComponent(typeof(EventListener))]
    public class DraggableUI : MonoBehaviour
    {
        [Header("移动速度")]
        [SerializeField] private float MoveSpeed = 50;

        private bool isGrag;
        private Vector2 offest;
        private Vector2 endPos;

        private EventListener listener;

        private void Awake()
        {
            listener = GetComponent<EventListener>();

            listener.onPointDown += OnPointerDown;
            listener.onPointUp += OnPointerUp;
            listener.onDrag += OnDrag;
        }

        public void OnPointerDown(EventListener listener, PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.isValid)
            {
                offest = (Vector2)this.transform.position - eventData.position;
            }
        }

        public void OnDrag(EventListener listener, PointerEventData eventData)
        {
            isGrag = true;
            endPos = eventData.position + offest;
        }

        public void OnPointerUp(EventListener listener, PointerEventData eventData)
        {
            isGrag = false;
        }

        private void Update()
        {
            if (isGrag)
            {
                transform.position = Vector2.Lerp((Vector2)transform.position, endPos, Time.deltaTime * MoveSpeed);
            }
        }

        private void OnDestroy()
        {
            listener.onPointDown -= OnPointerDown;
            listener.onPointUp -= OnPointerUp;
            listener.onDrag -= OnDrag;
        }
    }
}