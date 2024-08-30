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
        /// ���������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="capacity">����</param>
        /// <returns>Ҫ���Ķ����</returns>
        public IObjectPool<T> CreateObjectPool<T>(string name, int capacity) where T : ObjectBase
        {
            ObjectPool<T> objectPool = new ObjectPool<T>(capacity);
            m_ObjectPools.Add(name, objectPool);
            return objectPool;
        }

        /// <summary>
        /// ��ȡ�����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <returns>Ҫ��ȡ�Ķ����</returns>
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
        /// ���ٶ����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <returns>�Ƿ����ٶ���سɹ�</returns>
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

