using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleFramework
{
    /// <summary>
    /// 相机上务必添加 PhysicsRaycaster 组件
    /// </summary>
    [RequireComponent(typeof(EventListener))]
    public class Draggable : MonoBehaviour
    {
        [Header("移动速度")]
        [SerializeField] private float MoveSpeed = 50;

        private Camera eventCamera;
        private bool isGrag;
        private Plane plane;
        private Vector3 offest;
        private Vector3 endPos;

        [Header("绘画参数")]
        [SerializeField] private bool isStart = true;
        private Color lineColor = new Color(1.0f, 0, 0, 1.0f);
        [SerializeField] private int solidRectangleWidth = 4;
        private Color solidRectangleFaceColor = new Color(1.0f, 0, 0, 0.1f);
        private Color solidRectangleOutLineColor = new Color(1.0f, 0, 0, 1.0f);

        //绘画点位数据
        private Vector3 startjiaoPoint;
        private Vector3 dargjiaoPoint;

        private EventListener listener;

        private void Awake() 
        {
            listener = GetComponent<EventListener>();

            listener.onPointDown += OnPointerDown;
            listener.onPointUp += OnPointerUp;
            listener.onDrag += OnDrag;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (eventCamera == null || isStart == false)
                return;

            Vector3[] verts = new Vector3[]
            {
                new Vector3(startjiaoPoint.x - solidRectangleWidth / 2, startjiaoPoint.y - solidRectangleWidth / 2, startjiaoPoint.z),
                new Vector3(startjiaoPoint.x - solidRectangleWidth / 2, startjiaoPoint.y + solidRectangleWidth / 2, startjiaoPoint.z),
                new Vector3(startjiaoPoint.x + solidRectangleWidth / 2, startjiaoPoint.y + solidRectangleWidth / 2, startjiaoPoint.z),
                new Vector3(startjiaoPoint.x + solidRectangleWidth / 2, startjiaoPoint.y - solidRectangleWidth / 2, startjiaoPoint.z)
            };
            Handles.DrawSolidRectangleWithOutline(verts, solidRectangleFaceColor, solidRectangleOutLineColor);

            //Line
            Handles.color = lineColor;
            Handles.DrawLine(eventCamera.transform.position, dargjiaoPoint);
        }
#endif

        public void OnPointerDown(EventListener listener, PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.isValid)
            {
                eventCamera = eventData.pressEventCamera;

                Vector3 normal = eventCamera.transform.forward;
                startjiaoPoint = eventData.pointerCurrentRaycast.worldPosition;
                dargjiaoPoint = eventData.pointerCurrentRaycast.worldPosition;
                plane = new Plane(normal, startjiaoPoint);

                offest = transform.position - startjiaoPoint;
            }
        }

        public void OnDrag(EventListener listener, PointerEventData eventData)
        {
            isGrag = true;
            Ray ray = eventCamera.ScreenPointToRay(eventData.position);
            if (plane.Raycast(ray, out float distance))
            {
                dargjiaoPoint = ray.origin + ray.direction * distance;
                endPos = dargjiaoPoint + offest;
                Debug.Log("Drag: " + Time.time);
            }
        }

        public void OnPointerUp(EventListener listener, PointerEventData eventData)
        {
            isGrag = false;
        }

        private void Update()
        {
            if (isGrag)
            {
                Debug.Log("Update: " + Time.time);
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * MoveSpeed);
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