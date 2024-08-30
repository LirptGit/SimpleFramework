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
        [Header("ab�����ڵ�·��")]
        public string abPath;

        private Dictionary<string, AssetBundle> abCache = new();
        private List<string> abLoadingName = new();

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Resource���ؼ���
        /// </summary>
        /// <param name="assetPath">��Դ��Resource�µ�·��</param>
        /// <param name="action">������ɵĻص�����</param>
        public void ResourceLoadAsset(string assetPath, Action<UnityEngine.Object> action = null)
        {
            action?.Invoke(Resources.Load(assetPath));
        }

        /// <summary>
        /// ͬ������ab��Դ
        /// </summary>
        /// <param name="abName">ab��������</param>
        /// <param name="assetName">Ҫ���ص���Դ����</param>
        /// <param name="action">������ɵĻص�����</param>
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
        /// �첽����ab������Դ
        /// </summary>
        /// <param name="abName">ab��������</param>
        /// <param name="assetName">Ҫ���ص���Դ����</param>
        /// <param name="type">Ҫ���ص���Դ������</param>
        /// <param name="action">������ɵĻص�����</param>
        public void LoadAssetAnsy(string abName, string assetName, Type type, Action<UnityEngine.Object> action = null)
        {
            IEnumerator Load()
            {
                //********************������Ҫab��********************
                AssetBundle ab = null;
                if (abCache.ContainsKey(abName))
                {
                    ab = abCache[abName];
                }
                else
                {
                    if (!abLoadingName.Contains(abName))
                    {
                        //��abName����ab��������...
                        abLoadingName.Add(abName);
                        AssetBundleCreateRequest abAsy = AssetBundle.LoadFromFileAsync(abPath + "/" + abName);
                        yield return abAsy;
                        ab = abAsy.assetBundle;
                        abCache.Add(abName, ab);
                        abLoadingName.Remove(abName);
                    }
                    else
                    {
                        //��abName����ab�����ڱ������У��ȴ��������
                        while (abLoadingName.Contains(abName))
                        {
                            yield return null;
                        }
                        ab = abCache[abName];
                    }

                }
                //********************������Ҫ��ab����Դ********************
                AssetBundleRequest request = ab.LoadAssetAsync(assetName, type);
                yield return request;
                action?.Invoke(request.asset);
            }

            StartCoroutine(Load());
        }

        /// <summary>
        /// ж��ָ��ab��
        /// </summary>
        /// <param name="abName">Ҫж��ab��������</param>
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