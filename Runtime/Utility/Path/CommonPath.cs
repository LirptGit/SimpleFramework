using UnityEngine;

namespace SimpleFramework 
{
    public class CommonPath : MonoBehaviour
    {
        public static string GetABPath(string localPath) 
        {
            string result = "";

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    result = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.WindowsPlayer:
                    result = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.Android:
                    result = Application.persistentDataPath;
                    break;
            }
            return result + "/" + localPath;
        }
    }
}