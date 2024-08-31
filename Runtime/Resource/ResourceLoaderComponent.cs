using System;
using XFABManager;
using System.Collections;
using UnityEngine;

namespace SimpleFramework
{
    public enum LoaderType
    {
        Resources,
        Asserbundle,
        AsserbundleAsync,
    }

    public class ResourceLoaderComponent : SimpleFrameworkComponent
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void ResourceLoadAsset<T>(string assetName, Action<UnityEngine.Object> action = null) where T : UnityEngine.Object
        {
            UnityEngine.Object obj = Resources.Load<T>(assetName);
            action?.Invoke(obj);
        }

        /// <summary>
        /// 同步加载ab资源
        /// </summary>
        /// <param name="abName">ab包的名字</param>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="action">加载完成的回调函数</param>
        public void LoadAsset<T>(string projectName, string assetName, Action<UnityEngine.Object> action = null) where T : UnityEngine.Object
        {
            UnityEngine.Object obj = AssetBundleManager.LoadAsset<T>(projectName, assetName);
            action?.Invoke(obj);
        }


        /// <summary>
        /// 异步加载ab包的资源
        /// </summary>
        /// <param name="abName">ab包的名字</param>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="type">要加载的资源的类型</param>
        /// <param name="action">加载完成的回调函数</param>
        public void LoadAssetAnsy<T>(string projectName, string assetName, Action<UnityEngine.Object> action = null) where T : UnityEngine.Object
        {
            IEnumerator Load()
            {
                LoadAssetRequest request = AssetBundleManager.LoadAssetAsync<T>(projectName, assetName);
                yield return request;
                UnityEngine.Object obj = request.asset;
                action?.Invoke(obj);
            }

            StartCoroutine(Load());
        }

        /// <summary>
        /// 卸载指定ab包
        /// </summary>
        /// <param name="abName">要卸载ab包的名字</param>
        public void UnLoad(object asset)
        {
            AssetBundleManager.UnloadAsset(asset);
        }
    }
}