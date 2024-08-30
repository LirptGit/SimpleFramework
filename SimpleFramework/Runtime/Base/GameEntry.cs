using System;
using System.Collections.Generic;

namespace SimpleFramework
{
    /// <summary>
    /// 游戏入口
    /// </summary>
    public static class GameEntry
    {
        public static List<SimpleFrameworkComponent> FrameworkComponents = new List<SimpleFrameworkComponent>();

        /// <summary>
        /// 注册游戏框架
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
        /// 获取游戏框架组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>要获取的游戏框架</returns>
        public static T GetComponent<T>() where T : SimpleFrameworkComponent
        {
            return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// 获取游戏框架组件
        /// </summary>
        /// <param name="type">组件类型</param>
        /// <returns>要获取的游戏框架</returns>
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

