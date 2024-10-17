using System;

namespace SimpleFramework
{
    /// <summary>
    /// �ڲ�����
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public class Object<T> : IReference where T : ObjectBase
    {
        private T m_Object;
        private int m_SpawnCount;

        /// <summary>
        /// ��ʼ���ڲ��������ʵ��
        /// </summary>
        public Object()
        {
            m_Object = null;
            m_SpawnCount = 0;
        }

        /// <summary>
        /// ��ȡ�����ϴ�ʹ��ʱ��
        /// </summary>
        public DateTime LastUseTime
        {
            get
            {
                return m_Object.LastUseTime;
            }
        }


        /// <summary>
        /// ��ȡ�����Ƿ�����ʹ��
        /// </summary>
        public bool IsInUse
        {
            get
            {
                return m_SpawnCount > 0;
            }
        }

        /// <summary>
        /// �����ڲ�����
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="spawned">�����Ƿ��ѱ���ȡ</param>
        /// <returns>�������ڲ�����</returns>
        public static Object<T> Create(T obj, bool spawned)
        {
            Object<T> internalObject = ReferencePool.Acquire<Object<T>>();
            internalObject.m_Object = obj;
            internalObject.m_SpawnCount = spawned ? 1 : 0;
            if (spawned)
            {
                obj.OnSpawn();
            }

            return internalObject;
        }

        /// <summary>
        /// �����ڲ�����
        /// </summary>
        public void Clear()
        {
            m_Object = null;
            m_SpawnCount = 0;
        }

        /// <summary>
        /// �鿴����
        /// </summary>
        /// <returns>����</returns>
        public T Peek()
        {
            return m_Object;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns>����</returns>
        public T Spawn()
        {
            m_SpawnCount++;
            m_Object.LastUseTime = DateTime.UtcNow;
            m_Object.OnSpawn();
            return m_Object;
        }

        /// <summary>
        /// ���ն���
        /// </summary>
        public void Unspawn()
        {
            m_Object.OnUnspawn();
            m_Object.LastUseTime = DateTime.UtcNow;
            m_SpawnCount--;
        }

        /// <summary>
        /// �ͷŶ���
        /// </summary>
        /// <param name="isShutdown">�Ƿ��ǹرն����ʱ����</param>
        public void Release(bool isShutdown)
        {
            m_Object.Release(isShutdown);
            ReferencePool.Release(m_Object);
        }
    }
}