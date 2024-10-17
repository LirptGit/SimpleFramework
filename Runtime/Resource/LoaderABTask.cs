using System;

namespace SimpleFramework
{
    public enum LoaderType
    {
        Asserbundle,
        AsserbundleAsync,
    }

    [System.Serializable]
    public class LoaderABTask : IReference
    {
        public LoaderType LoaderType = LoaderType.Asserbundle;

        //ab����·��
        public string ABPath;

        //ab��������
        public string ABName;

        //������Դ������
        public Type AssetType;

        //��Դ������
        public string AssetName;

        //ab�����������ơ����������������д���������ƣ���������·��һ��Ҫ��ab��һ�¡�
        public string MainABName;

        public bool IsLoading { get; set; }

        public static LoaderABTask Create(LoaderType loaderType, string abPath, string abName, Type assetType, string assetName, string mainABName = null)
        {
            LoaderABTask loaderABTask = ReferencePool.Acquire<LoaderABTask>();
            loaderABTask.LoaderType = loaderType;
            loaderABTask.ABPath = abPath;
            loaderABTask.ABName = abName;
            loaderABTask.AssetType = assetType;
            loaderABTask.AssetName = assetName;
            loaderABTask.MainABName = mainABName;
            return loaderABTask;
        }

        public void Clear()
        {
            LoaderType = LoaderType.Asserbundle;
            ABPath = null;
            ABName = null;
            AssetType = null;
            AssetName = null;
            MainABName = null;
            IsLoading = false;
        }
    }
}
