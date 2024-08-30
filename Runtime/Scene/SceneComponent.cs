using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SimpleFramework
{
    public class SceneComponent : SimpleFrameworkComponent
    {
        [Range(0, 1)]
        public float LoadSceneProgress;

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadScene(string sceneName, LoadSceneMode LoadSceneMode)
        {
            IEnumerator Load()
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode);
                operation.allowSceneActivation = false;

                while (operation.progress < 0.9f)
                {
                    LoadSceneProgress = operation.progress;
                }

                while (LoadSceneProgress < 1)
                {
                    LoadSceneProgress += Time.deltaTime;
                }
                LoadSceneProgress = 1;

                operation.allowSceneActivation = true;
                yield return null;
            }

            LoadSceneProgress = 0;
            StartCoroutine(Load());
        }

        /// <summary>
        /// 异步卸载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void UnloadScene(string sceneName)
        {
            // 使用异步方式卸载场景  
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}