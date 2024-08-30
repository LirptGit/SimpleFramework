using System;

namespace SimpleFramework
{
    /// <summary>
    /// ����ؽӿ�
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public interface IObjectPool<T>
    {
        /// <summary>
        /// ��ȡ����ض�������
        /// </summary>
        Type ObjectType
        {
            get;
        }

        /// <summary>
        /// ��ȡ������ж��������
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// ��ȡ�����ö���ص�����
        /// </summary>
        int Capacity
        {
            get;
            set;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="spawned">�����Ƿ��ѱ���ȡ</param>
        void Register(T obj, bool spawned);

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns>Ҫ��ȡ�Ķ���</returns>
        T Spawn();

        /// <summary>
        /// ���ն���
        /// </summary>
        /// <param name="obj">Ҫ���յĶ���</param>
        void Unspawn(T obj);

        /// <summary>
        /// �ͷŶ���
        /// </summary>
        /// <param name="obj">Ҫ�ͷŵĶ���</param>
        /// <returns>�ͷŶ����Ƿ�ɹ�</returns>
        bool ReleaseObject(T obj);
    }
}