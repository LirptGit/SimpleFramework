using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleFramework
{
    public class ResourceLoaderComponent : SimpleFrameworkComponent
    {
        [Header("同时加载AB资源的最大数量")]
        public int ABLoadMaxCount = 1;

        private AssetBundle dependenceBundle = null;
        private readonly Dictionary<string, AssetBundle> abDir = new Dictionary<string, AssetBundle>();
        private readonly Dictionary<string, UnityEngine.Object> assetDir = new Dictionary<string, UnityEngine.Object>();

        private readonly Queue<LoaderABTask> loaderABTaskQueue = new Queue<LoaderABTask>();

        private readonly List<LoaderABTask> curTask = new List<LoaderABTask>();
        private readonly Dictionary<LoaderABTask, Action<UnityEngine.Object>> loaderABTaskToEvent = new Dictionary<LoaderABTask, Action<UnityEngine.Object>>();

        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (curTask.Count < ABLoadMaxCount && loaderABTaskQueue.Count > 0)
            {
                curTask.Add(loaderABTaskQueue.Dequeue());
            }

            for (int i = 0; i < curTask.Count; i++)
            {
                if (curTask[i] != null && !curTask[i].IsLoading)
                {
                    curTask[i].IsLoading = true;

                    if (curTask[i].LoaderType == LoaderType.Asserbundle)
                    {
                        LoadAsset(curTask[i]);
                    }
                    else
                    {
                        StartCoroutine(LoadAssetAnsy(curTask[i]));
                    }
                }
            }
        }

        /// <summary>
        /// 加载ab资源
        /// </summary>
        public void AddAsset(LoaderABTask task, Action<UnityEngine.Object> onFinishEvent = null)
        {
            if (assetDir.ContainsKey(task.AssetName))
            {
                onFinishEvent(assetDir[task.AssetName]);
                return;
            }

            loaderABTaskQueue.Enqueue(task);
            loaderABTaskToEvent.Add(task, onFinishEvent);
        }

        void LoadAsset(LoaderABTask task) 
        {
            AssetBundle ab = LoadAB(task);
            UnityEngine.Object obj = ab.LoadAsset(task.AssetName, task.AssetType);

            loaderABTaskToEvent[task](obj);
            assetDir.Add(task.AssetName, obj);

            curTask.Remove(task);
            loaderABTaskToEvent.Remove(task);
            ReferencePool.Release(task);
        }

        IEnumerator LoadAssetAnsy(LoaderABTask task) 
        {
            AssetBundle ab = LoadAB(task);
            AssetBundleRequest abr = ab.LoadAssetAsync(task.AssetName, task.AssetType);
            yield return abr;

            loaderABTaskToEvent[task](abr.asset);
            assetDir.Add(task.AssetName, abr.asset);

            curTask.Remove(task);
            loaderABTaskToEvent.Remove(task);
            ReferencePool.Release(task);
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private AssetBundle LoadAB(LoaderABTask task)
        {
            AssetBundle ab = null;

            if (task.MainABName != null)
            {
                string[] dependences = GetAssetBundleDependences(task);

                for (int i = 0; i < dependences.Length; i++)
                {
                    if (!abDir.ContainsKey(dependences[i]))
                    {
                        ab = AssetBundle.LoadFromFile(task.ABPath + "/" + dependences[i]);
                        abDir.Add(dependences[i], ab);
                    }
                }
            }

            if (!abDir.ContainsKey(task.ABName))
            {
                ab = AssetBundle.LoadFromFile(task.ABPath + "/" + task.ABName);
                abDir.Add(task.ABName, ab);
            }

            return abDir[task.ABName];
        }

        /// <summary>
        /// 获取AB包相关的依赖包
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private string[] GetAssetBundleDependences(LoaderABTask task) 
        {
            if (dependenceBundle != null)
            {
                dependenceBundle.Unload(true);
                dependenceBundle = null;
            }

            if (dependenceBundle == null)
            {
                dependenceBundle = AssetBundle.LoadFromFile(task.ABPath + "/" + task.MainABName);
            }

            AssetBundleManifest manifest = dependenceBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] dependences = manifest.GetAllDependencies(task.ABName);
            return dependences;
        }
    }
}