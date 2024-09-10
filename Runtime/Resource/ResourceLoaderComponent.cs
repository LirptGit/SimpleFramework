using System;
using System.Collections;
using System.Collections.Generic;
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
        private static AssetBundle dependenceBundle = null;
        private readonly Dictionary<string, AssetBundle> abDir = new Dictionary<string, AssetBundle>();
        private string abPath
        {
            get
            {
                return Application.streamingAssetsPath + "/" + mainABName + "/";
            }
        }

        private string mainABName
        {
            get
            {
                return "StandaloneWindows";
            }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public UnityEngine.Object ResourceLoadAsset<T>(string assetName) where T : UnityEngine.Object
        {
            UnityEngine.Object obj = Resources.Load<T>(assetName);
            return obj;
        }

        /// <summary>
        /// ͬ������ab��Դ
        /// </summary>
        /// <param name="abName">ab��������</param>
        /// <param name="assetName">Ҫ���ص���Դ����</param>
        /// <param name="action">������ɵĻص�����</param>
        public UnityEngine.Object LoadAsset<T>(string abName, string assetName) where T : UnityEngine.Object
        {
            AssetBundle ab = LoadAB(abName);

            return ab.LoadAsset<T>(assetName);
        }


        /// <summary>
        /// �첽����ab������Դ
        /// </summary>
        /// <param name="abName">ab��������</param>
        /// <param name="assetName">Ҫ���ص���Դ����</param>
        /// <param name="action">������ɵĻص�����</param>
        public void LoadAssetAnsy<T>(string abName, string assetName, Action<UnityEngine.Object> action = null) where T : UnityEngine.Object
        {
            IEnumerator Load(AssetBundle ab)
            {
                AssetBundleRequest abr = ab.LoadAssetAsync<T>(assetName);
                yield return abr;
                action?.Invoke(abr.asset);
            }

            AssetBundle ab = LoadAB(abName);
            StartCoroutine(Load(ab));
        }

        /// <summary>
        /// ж��ָ��ab��
        /// </summary>
        /// <param name="abName">Ҫж��ab��������</param>
        public void UnLoad(string abName)
        {
            if (!abDir.ContainsKey(abName))
            {
                return;
            }

            abDir[abName].Unload(true);
            abDir.Remove(abName);
        }

        /// <summary>
        /// ����AB��
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private AssetBundle LoadAB(string abName) 
        {
            string[] dependences = GetAssetBundleDependences(abName);

            AssetBundle ab = null;
            for (int i = 0; i < dependences.Length; i++)
            {
                if (!abDir.ContainsKey(dependences[i]))
                {
                    ab = AssetBundle.LoadFromFile(abPath + dependences[i]);
                    abDir.Add(dependences[i], ab);
                }
            }

            if (!abDir.ContainsKey(abName))
            {
                ab = AssetBundle.LoadFromFile(abPath + abName);
                abDir.Add(abName, ab);
            }

            return abDir[abName];
        }

        /// <summary>
        /// ��ȡAB����ص�������
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        private string[] GetAssetBundleDependences(string abName) 
        {
            if (dependenceBundle != null)
            {
                dependenceBundle.Unload(true);
                dependenceBundle = null;
            }

            if (dependenceBundle == null)
            {
                dependenceBundle = AssetBundle.LoadFromFile(abPath + mainABName);
            }

            AssetBundleManifest manifest = dependenceBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] dependences = manifest.GetAllDependencies(abName);
            return dependences;
        }
    }
}