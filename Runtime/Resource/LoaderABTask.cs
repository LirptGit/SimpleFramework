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

        //ab包的路径
        public string ABPath;

        //ab包的名称
        public string ABName;

        //加载资源的类型
        public Type AssetType;

        //资源的名称
        public string AssetName;

        //ab包主包的名称【如果有依赖包在填写主包的名称，依赖包的路径一定要和ab包一致】
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
