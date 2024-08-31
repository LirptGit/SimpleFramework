using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleFramework
{
    public enum LoaderType
    {
        ResourceLoad,
        Asserbundle,
        AsserbundleAsync,
    }

    public class ResourceLoaderComponent : SimpleFrameworkComponent
    {
        [Header("ab包所在的路径")]
        public string abPath;

        private Dictionary<string, AssetBundle> abCache = new();
        private List<string> abLoadingName = new();

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Resource本地加载
        /// </summary>
        /// <param name="assetPath">资源在Resource下的路径</param>
        /// <param name="action">加载完成的回调函数</param>
        public void ResourceLoadAsset(string assetPath, Action<UnityEngine.Object> action = null)
        {
            action?.Invoke(Resources.Load(assetPath));
        }

        /// <summary>
        /// 同步加载ab资源
        /// </summary>
        /// <param name="abName">ab包的名字</param>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="action">加载完成的回调函数</param>
        public void LoadAsset(string abName, string assetName, Action<UnityEngine.Object> action = null)
        {
            AssetBundle ab = null;
            if (abCache.ContainsKey(abName))
            {
                ab = abCache[abName];
            }
            else
            {
                ab = AssetBundle.LoadFromFile(abPath + "/" + abName);
                abCache.Add(abName, ab);
            }

            UnityEngine.Object go = ab.LoadAsset(assetName);
            action?.Invoke(go);
        }


        /// <summary>
        /// 异步加载ab包的资源
        /// </summary>
        /// <param name="abName">ab包的名字</param>
        /// <param name="assetName">要加载的资源名称</param>
        /// <param name="type">要加载的资源的类型</param>
        /// <param name="action">加载完成的回调函数</param>
        public void LoadAssetAnsy(string abName, string assetName, Type type, Action<UnityEngine.Object> action = null)
        {
            IEnumerator Load()
            {
                //********************加载需要ab包********************
                AssetBundle ab = null;
                if (abCache.ContainsKey(abName))
                {
                    ab = abCache[abName];
                }
                else
                {
                    if (!abLoadingName.Contains(abName))
                    {
                        //【abName】的ab包加载中...
                        abLoadingName.Add(abName);
                        AssetBundleCreateRequest abAsy = AssetBundle.LoadFromFileAsync(abPath + "/" + abName);
                        yield return abAsy;
                        ab = abAsy.assetBundle;
                        abCache.Add(abName, ab);
                        abLoadingName.Remove(abName);
                    }
                    else
                    {
                        //【abName】的ab包正在被加载中，等待加载完成
                        while (abLoadingName.Contains(abName))
                        {
                            yield return null;
                        }
                        ab = abCache[abName];
                    }

                }
                //********************加载需要的ab包资源********************
                AssetBundleRequest request = ab.LoadAssetAsync(assetName, type);
                yield return request;
                action?.Invoke(request.asset);
            }

            StartCoroutine(Load());
        }

        /// <summary>
        /// 卸载指定ab包
        /// </summary>
        /// <param name="abName">要卸载ab包的名字</param>
        public void UnLoad(string abName)
        {
            if (abCache.ContainsKey(abName))
            {
                abCache[abName].Unload(false);
                abCache.Remove(abName);
            }
        }
    }
}