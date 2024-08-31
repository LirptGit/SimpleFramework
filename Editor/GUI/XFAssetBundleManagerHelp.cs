using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace XFABManager
{

    public class PackageInfo {

        public string version;
    }

    public class XFAssetBundleManagerHelp : EditorWindow
    {

        Rect textureRect = new Rect(0,10,291 * 0.7F ,96 * 0.7F);
        Texture logo;
        private GUIStyle style;

        private string version;

        private void Awake()
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture>("Packages/com.xfkj.xfabmanager/Editor/Texture/logo_web.png");
            
            if(logo == null)
                logo = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Texture/logo_web.png");

            TextAsset p = AssetDatabase.LoadAssetAtPath<TextAsset>("Packages/com.xfkj.xfabmanager/package.json");
            
            if(p == null)
                p = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/package.json");

            PackageInfo info = JsonConvert.DeserializeObject<PackageInfo>(p.text);

            version = string.Format("Version {0}", info.version);
        }

        private void ConfigStyle() {
            style = new GUIStyle( GUI.skin.label);
            style.richText = true;
            style.normal.textColor =new Color(0.03f, 0.4f, 0.9f, 1);
            style.onHover.textColor = Color.white;
            style.alignment = TextAnchor.MiddleLeft;
            style.fontStyle = FontStyle.Italic;
            //style.onFocused.textColor = Color.red;
        }

        // 每秒10帧更新
        void Update()
        {
            //开启窗口的重绘，不然窗口信息不会刷新
            Repaint();
        }

        private void OnGUI()
        {

            GUI.DrawTexture(textureRect, logo);
            GUILayout.Space(textureRect.height + 20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(130);
            GUILayout.Label(version);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

 
            GUILayout.Label("XFABManager提供了AssetBundle的可视化管理功能，我们通过该插件可以很方便的对项目中");
            GUILayout.Label("的AssetBundle进行打包，添加文件，删除文件等等!!");
            GUILayout.Label("除此之外，此插件还提供了AssetBundle的加载，卸载，更新，下载，压缩，释放等功能，");
            GUILayout.Label("通过该插件可以很方便快速的完成AssetBundle相关的功能开发，提升开发效率!");
            GUILayout.Label("更多信息可通过点击下方教程链接获取!");
            GUILayout.Space(20);
            if ( style == null ) {
                ConfigStyle();
            }
     
            DrawLink("视频介绍:", "https://www.bilibili.com/video/BV1Di4y1Y7tb");
            DrawLink("快速入门:", "https://gitee.com/xianfengkeji/xfabmanager/blob/master/Documentation~/XFABManager%E5%BF%AB%E9%80%9F%E5%85%A5%E9%97%A8.md");
            DrawLink("插件源码:", "https://gitee.com/xianfengkeji/xfabmanager");
            DrawLink("项目实战:", "https://space.bilibili.com/258939476/pugv");
            GUILayout.Space(20);
            GUILayout.Label("XFABManager交流群:1058692748");

            //GUILayout.Space(20);
            GUILayout.Label("*弦风课堂制作");
        }

        private void DrawLink(string title,string url) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title,GUILayout.Width(60));

            if (GUILayout.Button(url, style )) {
                Application.OpenURL(url);
            }

            GUILayout.EndHorizontal();
        }

    }

}

