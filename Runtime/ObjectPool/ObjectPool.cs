using System;
using System.Collections.Generic;

namespace SimpleFramework
{
    /// <summary>
    /// �����
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
    {
        private readonly List<Object<T>> m_Objects;
        private readonly Dictionary<object, Object<T>> m_ObjectMap;

        private int m_Capacity;

        /// <summary>
        /// ��ʼ������ص���ʵ��
        /// </summary>
        /// <param name="autoReleaseInterval">������Զ��ͷſ��ͷŶ���ļ������</param>
        /// <param name="capacity">����ص�����</param>
        public ObjectPool(int capacity)
        {
            m_Objects = new List<Object<T>>(capacity);
            m_ObjectMap = new Dictionary<object, Object<T>>(capacity);

            Capacity = capacity;
        }

        /// <summary>
        /// ��ȡ����ض�������
        /// </summary>
        public Type ObjectType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// ��ȡ������ж��������
        /// </summary>
        public int Count
        {
            get
            {
                return m_ObjectMap.Count;
            }
        }

        /// <summary>
        /// ��ȡ�����ö���ص�����
        /// </summary>
        public int Capacity
        {
            get
            {
                return m_Capacity;
            }
            set
            {
                if (m_Capacity == value)
                {
                    return;
                }

                m_Capacity = value;
            }
        }


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="spawned">�����Ƿ��ѱ���ȡ</param>
        public bool Register(T obj, bool spawned)
        {
            Object<T> internalObject = Object<T>.Create(obj, spawned);
            m_Objects.Add(internalObject);
            m_ObjectMap.Add(obj.Target, internalObject);

            if (Count > m_Capacity)
            {
                Release();
            }
            if (Count > m_Capacity)
            {
                UnityEngine.Debug.LogError("����ص�������С����������");
                return false;
            }

            return true;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns>Ҫ��ȡ�Ķ���</returns>
        public T Spawn()
        {
            foreach (Object<T> internalObject in m_Objects)
            {
                if (!internalObject.IsInUse)
                {
                    return internalObject.Spawn();
                }
            }

            return null;
        }

        /// <summary>
        /// ���ն���
        /// </summary>
        /// <param name="obj">Ҫ���յĶ���</param>
        public void Unspawn(T obj)
        {
            Object<T> internalObject = GetObject(obj.Target);

            if (internalObject != null)
            {
                internalObject.Unspawn();
            }
        }

        /// <summary>
        /// �ͷſ����ͷŵĶ���
        /// </summary>
        private void Release()
        {
            DateTime minTime = DateTime.MaxValue;
            Object<T> toReleaseObjects = null;
            foreach (var item in m_Objects)
            {
                if (minTime < item.LastUseTime)
                {
                    toReleaseObjects = item;
                }
            }

            if (toReleaseObjects == null)
            {
                return;
            }

            ReleaseObject(toReleaseObjects.Peek());
        }

        /// <summary>
        /// �ͷŶ���
        /// </summary>
        /// <param name="target">Ҫ�ͷŵĶ���</param>
        /// <returns>�ͷŶ����Ƿ�ɹ�</returns>
        public bool ReleaseObject(T obj)
        {
            Object<T> internalObject = GetObject(obj.Target);

            if (internalObject.IsInUse)
            {
                UnityEngine.Debug.Log($"{obj.Target}����Ŀǰ����ʹ�ã��޷��ͷ�");
                return false;
            }

            m_Objects.Remove(internalObject);
            m_ObjectMap.Remove(internalObject.Peek().Target);

            internalObject.Release(false);
            ReferencePool.Release(internalObject);
            return true;
        }

        /// <summary>
        /// ��ȡ�ڲ�����
        /// </summary>
        /// <param name="target">Ŀ��</param>
        /// <returns>�ڲ�����</returns>
        private Object<T> GetObject(object target)
        {
            Object<T> internalObject = null;
            if (m_ObjectMap.TryGetValue(target, out internalObject))
            {
                return internalObject;
            }

            return null;
        }

        internal override void Shutdown()
        {
            foreach (KeyValuePair<object, Object<T>> objectInMap in m_ObjectMap)
            {
                objectInMap.Value.Release(true);
                ReferencePool.Release(objectInMap.Value);
            }

            m_Objects.Clear();
            m_ObjectMap.Clear();
        }
    }
}
