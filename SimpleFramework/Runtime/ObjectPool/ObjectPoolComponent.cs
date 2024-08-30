using System;
using System.Collections.Generic;

namespace SimpleFramework
{
    public class ObjectPoolComponent : SimpleFrameworkComponent
    {
        private readonly Dictionary<string, ObjectPoolBase> m_ObjectPools = new Dictionary<string, ObjectPoolBase>();

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="capacity">容量</param>
        /// <returns>要建的对象池</returns>
        public IObjectPool<T> CreateObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            ObjectPool<T> objectPool = new ObjectPool<T>(capacity);
            m_ObjectPools.Add(name, objectPool);
            return objectPool;
        }

        /// <summary>
        /// 获取对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>要获取的对象池</returns>
        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            ObjectPoolBase objectPool = null;
            if (m_ObjectPools.TryGetValue(name, out objectPool))
            {
                return (IObjectPool<T>)objectPool;
            }

            return null;
        }

        /// <summary>
        /// 销毁对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>是否销毁对象池成功</returns>
        public bool DestroyObjectPool<T>(string name) where T : ObjectBase
        {
            ObjectPoolBase objectPool = null;
            if (m_ObjectPools.TryGetValue(name, out objectPool))
            {
                objectPool.Shutdown();
                return m_ObjectPools.Remove(name);
            }

            return false;
        }
    }
}

