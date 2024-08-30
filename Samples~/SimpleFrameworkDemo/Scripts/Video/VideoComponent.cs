//using UnityEngine;
//using SimpleFramework;
//using System.Collections.Generic;
//using RenderHeads.Media.AVProVideo;

//public class VideoComponent : SimpleFrameworkComponent
//{
//    [SerializeField] private string[] mediaNames;

//    private Dictionary<string, MediaPlayer> mediaDict = new Dictionary<string, MediaPlayer>();
//    protected override void Awake()
//    {
//        base.Awake();

//        for (int i = 0; i < mediaNames.Length; i++)
//        {
//            GameObject videoObj = new GameObject(mediaNames[i]);
//            videoObj.transform.parent = transform;
//            MediaPlayer media = videoObj.GetOrAddComponent<MediaPlayer>();
//            media.AutoOpen = false;
//            media.AutoStart = false;
//            mediaDict.Add(mediaNames[i], media);
//        }
//    }

//    public MediaPlayer GetMediaPlayer(string mediaName)
//    {
//        if (mediaDict.TryGetValue(mediaName, out MediaPlayer mediaPlayer))
//        {
//            return mediaPlayer;
//        }
//        return null;
//    }
//}