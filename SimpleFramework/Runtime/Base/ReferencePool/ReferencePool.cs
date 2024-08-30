using System;
using System.Collections.Generic;

namespace SimpleFramework
{
    /// <summary>
    /// ���ó�
    /// </summary>
    public static class ReferencePool
    {
        private static readonly Dictionary<Type, ReferenceCollection> m_ReferenceCollections = new Dictionary<Type, ReferenceCollection>();

        /// <summary>
        /// ��ȡ���óص�����
        /// </summary>
        public static int Count
        {
            get
            {
                return m_ReferenceCollections.Count;
            }
        }

        /// <summary>
        /// �������е����ó�
        /// </summary>
        public static void ClearAll()
        {
            lock (m_ReferenceCollections)
            {
                foreach (var item in m_ReferenceCollections)
                {
                    item.Value.RemoveAll();
                }
            }
        }

        /// <summary>
        /// �����óػ�ȡ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }

        /// <summary>
        /// �����ù黹���ó�
        /// </summary>
        /// <param name="reference"></param>
        public static void Release(IReference reference)
        {
            if (reference == null)
            {
                return;
            }

            Type referenceType = reference.GetType();
            GetReferenceCollection(referenceType);
        }

        /// <summary>
        /// ��ȡ��Ӧ���͵����ó��ռ���
        /// </summary>
        /// <param name="referenceType">Ҫ��ȡ����</param>
        /// <returns>��Ӧ���͵��ռ���</returns>
        private static ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            if (referenceType == null)
            {
                return null;
            }

            ReferenceCollection referenceCollection = null;
            lock (m_ReferenceCollections)
            {
                if (!m_ReferenceCollections.TryGetValue(referenceType, out referenceCollection))
                {
                    referenceCollection = new ReferenceCollection(referenceType);
                    m_ReferenceCollections.Add(referenceType, referenceCollection);
                }
            }
            return referenceCollection;
        }
    }
}

