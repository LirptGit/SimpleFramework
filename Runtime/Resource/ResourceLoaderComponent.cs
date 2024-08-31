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
        /// ͬ������ab��Դ
        /// </summary>
        /// <param name="abName">ab��������</param>
        /// <param name="assetName">Ҫ���ص���Դ����</param>
        /// <param name="action">������ɵĻص�����</param>
        public void LoadAsset<T>(string projectName, string assetName, Action<UnityEngine.Object> action = null) where T : UnityEngine.Object
        {
            UnityEngine.Object obj = AssetBundleManager.LoadAsset<T>(projectName, assetName);
            action?.Invoke(obj);
        }


        /// <summary>
        /// �첽����ab������Դ
        /// </summary>
        /// <param name="abName">ab��������</param>
        /// <param name="assetName">Ҫ���ص���Դ����</param>
        /// <param name="type">Ҫ���ص���Դ������</param>
        /// <param name="action">������ɵĻص�����</param>
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
        /// ж��ָ��ab��
        /// </summary>
        /// <param name="abName">Ҫж��ab��������</param>
        public void UnLoad(object asset)
        {
            AssetBundleManager.UnloadAsset(asset);
        }
    }
}