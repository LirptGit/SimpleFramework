using System;
using System.Collections.Generic;

namespace SimpleFramework
{
    /// <summary>
    /// ��Ϸ���
    /// </summary>
    public static class GameEntry
    {
        public static List<SimpleFrameworkComponent> FrameworkComponents = new List<SimpleFrameworkComponent>();

        /// <summary>
        /// ע����Ϸ���
        /// </summary>
        /// <param name="FrameworkComponent"></param>
        public static void RegisterComponent(SimpleFrameworkComponent FrameworkComponent)
        {
            Type type = FrameworkComponent.GetType();

            foreach (var item in FrameworkComponents)
            {
                if (item.GetType() == type)
                {
                    return;
                }
            }
            FrameworkComponents.Add(FrameworkComponent);
        }

        /// <summary>
        /// ��ȡ��Ϸ������
        /// </summary>
        /// <typeparam name="T">�������</typeparam>
        /// <returns>Ҫ��ȡ����Ϸ���</returns>
        public static T GetComponent<T>() where T : SimpleFrameworkComponent
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// ��ȡ��Ϸ������
        /// </summary>
        /// <param name="type">�������</param>
        /// <returns>Ҫ��ȡ����Ϸ���</returns>
        public static SimpleFrameworkComponent GetComponent(Type type)
        {
            foreach (var item in FrameworkComponents)
            {
                if (item.GetType() == type)
                {
                    return item;
                }
            }

            return null;
        }
    }
}

