using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SimpleFramework
{
    public class SceneComponent : SimpleFrameworkComponent
    {
        [Range(0, 1)]
        public float LoadSceneProgress;

        AsyncOperation operation;
        protected override void Awake()
        {
            base.Awake();
        }

        void Update()
        {
            if(operation != null)
            {
                LoadSceneProgress = operation.progress;
            }
            if (LoadSceneProgress >= 0.9f)
            {
                LoadSceneProgress = 1;
                operation.allowSceneActivation = true;
            }
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadScene(string sceneName, LoadSceneMode LoadSceneMode)
        {
            LoadSceneProgress = 0;
            operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode);
            operation.allowSceneActivation = false;
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
