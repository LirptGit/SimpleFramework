using System;
using System.Collections.Generic;

namespace SimpleFramework
{
    public class ReferenceCollection
    {
        private readonly Queue<IReference> m_References;
        private readonly Type m_ReferenceType;

        public ReferenceCollection(Type referenceType)
        {
            m_References = new Queue<IReference>();
            m_ReferenceType = referenceType;
        }

        public Type ReferenceType
        {
            get
            {
                return m_ReferenceType;
            }
        }

        public T Acquire<T>() where T : class, IReference, new()
        {
            if (typeof(T) != m_ReferenceType)
            {
                return null;
            }

            lock (m_References)
            {
                if (m_References.Count > 0)
                {
                    return (T)m_References.Dequeue();
                }
            }

            return new T();
        }

        public void Release(IReference reference)
        {
            reference.Clear();

            lock (m_References)
            {
                if (m_References.Contains(reference))
                {
                    return;
                }
                m_References.Enqueue(reference);
            }
        }

        public void RemoveAll()
        {
            lock (m_References)
            {
                m_References.Clear();
            }
        }
    }
}

