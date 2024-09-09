using System;
using System.Collections.Generic;

namespace SimpleFramework
{
    /// <summary>
    /// 引用池
    /// </summary>
    public static class ReferencePool
    {
        private static readonly Dictionary<Type, ReferenceCollection> m_ReferenceCollections = new Dictionary<Type, ReferenceCollection>();

        /// <summary>
        /// 获取引用池的数量
        /// </summary>
        public static int Count
        {
            get
            {
                return m_ReferenceCollections.Count;
            }
        }

        /// <summary>
        /// 清理所有的引用池
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
        /// 从引用池获取引用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }

        /// <summary>
        /// 将引用归还引用池
        /// </summary>
        /// <param name="reference"></param>
        public static void Release(IReference reference)
        {
            if (reference == null)
            {
                return;
            }

            Type referenceType = reference.GetType();
            GetReferenceCollection(referenceType).Release(reference);
        }

        /// <summary>
        /// 获取对应类型的引用池收集者
        /// </summary>
        /// <param name="referenceType">要获取类型</param>
        /// <returns>对应类型的收集者</returns>
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

