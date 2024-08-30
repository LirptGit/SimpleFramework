using System;

namespace SimpleFramework
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public interface IObjectPool<T>
    {
        /// <summary>
        /// 获取对象池对象类型
        /// </summary>
        Type ObjectType
        {
            get;
        }

        /// <summary>
        /// 获取对象池中对象的数量
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// 获取或设置对象池的容量
        /// </summary>
        int Capacity
        {
            get;
            set;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="spawned">对象是否已被获取</param>
        void Register(T obj, bool spawned);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns>要获取的对象</returns>
        T Spawn();

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="obj">要回收的对象</param>
        void Unspawn(T obj);

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="obj">要释放的对象</param>
        /// <returns>释放对象是否成功</returns>
        bool ReleaseObject(T obj);
    }
}