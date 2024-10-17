using System;
using System.Collections.Generic;

namespace SimpleFramework
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
    {
        private readonly List<Object<T>> m_Objects;
        private readonly Dictionary<object, Object<T>> m_ObjectMap;

        private int m_Capacity;

        /// <summary>
        /// 初始化对象池的新实例
        /// </summary>
        /// <param name="autoReleaseInterval">对象池自动释放可释放对象的间隔秒数</param>
        /// <param name="capacity">对象池的容量</param>
        public ObjectPool(int capacity)
        {
            m_Objects = new List<Object<T>>(capacity);
            m_ObjectMap = new Dictionary<object, Object<T>>(capacity);

            Capacity = capacity;
        }

        /// <summary>
        /// 获取对象池对象类型
        /// </summary>
        public Type ObjectType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// 获取对象池中对象的数量
        /// </summary>
        public int Count
        {
            get
            {
                return m_ObjectMap.Count;
            }
        }

        /// <summary>
        /// 获取或设置对象池的容量
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
        /// 创建对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="spawned">对象是否已被获取</param>
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
                UnityEngine.Debug.LogError("对象池的容量过小，超出限制");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns>要获取的对象</returns>
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
        /// 回收对象
        /// </summary>
        /// <param name="obj">要回收的对象</param>
        public void Unspawn(T obj)
        {
            Object<T> internalObject = GetObject(obj.Target);

            if (internalObject != null)
            {
                internalObject.Unspawn();
            }
        }

        /// <summary>
        /// 释放可以释放的对象
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
        /// 释放对象
        /// </summary>
        /// <param name="target">要释放的对象</param>
        /// <returns>释放对象是否成功</returns>
        public bool ReleaseObject(T obj)
        {
            Object<T> internalObject = GetObject(obj.Target);

            if (internalObject.IsInUse)
            {
                UnityEngine.Debug.Log($"{obj.Target}对象目前正在使用，无法释放");
                return false;
            }

            m_Objects.Remove(internalObject);
            m_ObjectMap.Remove(internalObject.Peek().Target);

            internalObject.Release(false);
            ReferencePool.Release(internalObject);
            return true;
        }

        /// <summary>
        /// 获取内部对象
        /// </summary>
        /// <param name="target">目标</param>
        /// <returns>内部对象</returns>
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
