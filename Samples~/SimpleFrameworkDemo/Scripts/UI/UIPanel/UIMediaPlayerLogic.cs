//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using SimpleFramework;
//using UnityEngine.EventSystems;
//using RenderHeads.Media.AVProVideo;

//public class UIMediaPlayerLogic : UGuiForm
//{
//    private MediaPlayer mediaPlayer;
//    [Header("视频播放器的名字")]
//    [SerializeField] string mediaPlayerName;
//    [Header("视频的路径")]
//    [SerializeField] string mediaPath;

//    [Header("Options")]
//    [SerializeField] float jumpDeltaTime = 5f;
//    [SerializeField] float userInactiveDuration = 3f;

//    [Header("UI 组件")]
//    [SerializeField] DisplayUGUI displayUGUI;
//    [SerializeField] Slider sliderTimeline;
//    [SerializeField] CanvasGroup controlGroup;

//    private EventListener sliderTimelineEvent;

//    [Header("UI 组件 (选项)")]
//    [SerializeField] Button buttonPlayPause;
//    [SerializeField] Button buttonNavBack;
//    [SerializeField] Button buttonNavForward;
//    [SerializeField] Button buttonVolume;
//    [SerializeField] Slider sliderVolume;
//    [SerializeField] TextMeshProUGUI textTimeDuration_2;
//    [SerializeField] TextMeshProUGUI textTimeDuration_3;
//    [SerializeField] Button buttonClose;

//    [Header("UI 组件属性素材 (选项)")]
//    [Tooltip("0: 播放按钮图标; 1:暂停按钮图标")]
//    [SerializeField] Sprite[] buttonPlayPauseSp;

//    private Image buttonPlayPauseImg;
//    private EventListener sliderVolumeEvent;

//    private float controlsFade = 1f;

//    private bool isAutoTimeline = true;
//    当前播放暂停按钮是否为播放按钮
//    private bool isActivePlayButton = false;
//    静音前的声音大小
//    private float isMuteForwardVolumeSize = 1.0f;

//    protected override void OnInit(object userData)
//    {
//        base.OnInit(userData);

//        mediaPlayer = GameEntry.Video.GetMediaPlayer(mediaPlayerName);
//        displayUGUI.CurrentMediaPlayer = mediaPlayer;

//        buttonPlayPause.onClick.AddListener(OnPlayPausePressed);
//        buttonNavBack.onClick.AddListener(OnPlayTimeBackButtonPressed);
//        buttonNavForward.onClick.AddListener(OnPlayTimeForwardButtonPressed);
//        buttonVolume.onClick.AddListener(OnMediaIsMuteButtonPressed);
//        buttonClose.onClick.AddListener(Close);

//        sliderTimelineEvent = sliderTimeline.GetComponent<EventListener>();

//        buttonPlayPauseImg = buttonPlayPause.GetComponent<Image>();
//        sliderVolumeEvent = sliderVolume.GetComponent<EventListener>();

//        sliderTimelineEvent.onPointDown += OnSliderTimelinePointDown;
//        sliderTimelineEvent.onPointUp += OnSliderTimelinePointUp;

//        sliderVolumeEvent.onPointDown += OnSliderVolumePointDown;
//        sliderVolumeEvent.onDrag += OnSliderVolumeDrag;
//    }

//    protected override void OnOpen(object userData)
//    {
//        base.OnOpen(userData);

//        if (userData != null)
//        {
//            mediaPath = userData as string;
//        }

//        if (mediaPath != "")
//        {
//            LoadVideo(mediaPath);
//        }
//    }

//    protected override void OnClose(object userData)
//    {
//        base.OnClose(userData);

//        CloseResetInfo();
//    }

//    private void Update()
//    {
//        if (!mediaPlayer)
//            return;

//        UpdateControlsVisibility();

//        if (mediaPlayer.Control.IsPlaying())
//        {
//            if (mediaPlayer.Control.IsFinished())
//            {
//                CloseResetInfo();
//            }

//            ChangePlayPauseSprite(1);
//        }
//        else
//        {
//            ChangePlayPauseSprite(0);
//        }

//        if (mediaPlayer.Info != null && mediaPlayer.Control != null)
//        {
//            if (isAutoTimeline)
//            {
//                float jindu = (float)mediaPlayer.Control.GetCurrentTime() / (float)mediaPlayer.Info.GetDuration();
//                if (mediaPlayer.Info.GetDuration() != 0)
//                {
//                    sliderTimeline.value = Mathf.Clamp(jindu, 0, 1);
//                }
//            }

//            textTimeDuration_2.text = $"{UnityExtension.TimeFormatTwo((float)mediaPlayer.Control.GetCurrentTime())} / {UnityExtension.TimeFormatTwo((float)mediaPlayer.Info.GetDuration())}";

//            textTimeDuration_3.text = $"{UnityExtension.TimeFormat((float)mediaPlayer.Control.GetCurrentTime())} / {UnityExtension.TimeFormat((float)mediaPlayer.Info.GetDuration())}";
//        }
//    }

//    private void OnSliderTimelinePointDown(EventListener listener, PointerEventData eventData)
//    {
//        if (mediaPlayer.Info != null)
//        {
//            isAutoTimeline = false;
//        }
//    }

//    private void OnSliderTimelinePointUp(EventListener listener, PointerEventData eventData)
//    {
//        if (mediaPlayer.Info != null && mediaPlayer.Control != null)
//        {
//            mediaPlayer.Control.Seek(sliderTimeline.value * (float)mediaPlayer.Info.GetDuration());
//            isAutoTimeline = true;
//        }

//    }

//    private void OnPlayPausePressed()
//    {
//        if (mediaPlayer && mediaPlayer.Control != null)
//        {
//            if (mediaPlayer.Info.HasAudio())
//            {
//                if (mediaPlayer.Control.IsPlaying())
//                {
//                    mediaPlayer.Pause();
//                }
//                else
//                {
//                    mediaPlayer.Play();
//                }
//            }
//        }
//    }

//    private void OnPlayTimeBackButtonPressed()
//    {
//        SeekRelative(-jumpDeltaTime);
//    }

//    private void OnPlayTimeForwardButtonPressed()
//    {
//        SeekRelative(jumpDeltaTime);
//    }

//    private void OnMediaIsMuteButtonPressed()
//    {
//        if (mediaPlayer.AudioMuted)
//        {
//            mediaPlayer.AudioMuted = false;
//            sliderVolume.value = isMuteForwardVolumeSize;
//        }
//        else
//        {
//            mediaPlayer.AudioMuted = true;
//            sliderVolume.value = 0;
//        }
//    }

//    private void OnSliderVolumePointDown(EventListener listener, PointerEventData eventData)
//    {
//        SeekVolume(sliderVolume.value);
//        isMuteForwardVolumeSize = sliderVolume.value;
//    }

//    private void OnSliderVolumeDrag(EventListener listener, PointerEventData eventData)
//    {
//        SeekVolume(sliderVolume.value);
//        isMuteForwardVolumeSize = sliderVolume.value;
//    }

//    private void ChangePlayPauseSprite(int index)
//    {
//        if (buttonPlayPauseSp.Length <= 0)
//            return;

//        buttonPlayPauseImg.sprite = buttonPlayPauseSp[index];
//    }

//    private void SeekRelative(float deltaTime)
//    {
//        if (mediaPlayer && mediaPlayer.Control != null)
//        {
//            double time = mediaPlayer.Control.GetCurrentTime() + deltaTime;
//            time = System.Math.Max(time, 0);
//            time = System.Math.Min(time, mediaPlayer.Info.GetDuration());
//            mediaPlayer.Control.Seek(time);
//        }
//    }

//    private void SeekVolume(float volume)
//    {
//        mediaPlayer.AudioVolume = volume;
//    }

//    private void UpdateControlsVisibility()
//    {
//        if (UserInteraction.IsUserInputThisFrame())
//        {
//            UserInteraction.InactiveTime = 0f;
//            FadeUpControls();
//        }
//        else
//        {
//            UserInteraction.InactiveTime += Time.unscaledDeltaTime;
//            if (UserInteraction.InactiveTime > userInactiveDuration)
//            {
//                FadeDownControls();
//            }
//            else
//            {
//                FadeUpControls();
//            }
//        }
//    }

//    private struct UserInteraction
//    {
//        public static float InactiveTime;
//        private static Vector3 previousMousePos;
//        private static int lastInputFrame;

//        public static bool IsUserInputThisFrame()
//        {
//            if (Time.frameCount == lastInputFrame)
//            {
//                return true;
//            }

//            Input.touchSupported 当设备或平台支持 Stylus Touch 时，返回 true
//                Input.touchCount 触摸次数。保证在整个帧期间不会更改
//                bool touchInput = (Input.touchSupported && Input.touchCount > 0);

//            Input.mousePresent 指示是否检测到鼠标设备
//                bool mouseInput = (Input.mousePresent && (Input.mousePosition != previousMousePos || Input.GetMouseButton(0)));

//            if (touchInput || mouseInput)
//            {
//                previousMousePos = Input.mousePosition;
//                lastInputFrame = Time.frameCount;
//                return true;
//            }

//            return false;
//        }
//    }

//    private void FadeUpControls()
//    {
//        if (!controlGroup.gameObject.activeSelf)
//        {
//            controlGroup.gameObject.SetActive(true);
//        }
//        controlsFade = Mathf.Min(1f, controlsFade + Time.deltaTime * 8f);
//        controlGroup.alpha = Mathf.Pow(controlsFade, 5f);
//    }

//    private void FadeDownControls()
//    {
//        if (controlGroup.gameObject.activeSelf)
//        {
//            controlsFade = Mathf.Max(0f, controlsFade - Time.deltaTime * 3f);
//            controlGroup.alpha = Mathf.Pow(controlsFade, 5f);
//            if (controlGroup.alpha <= 0f)
//            {
//                controlGroup.gameObject.SetActive(false);
//            }
//        }
//    }

//    private void CloseResetInfo()
//    {
//        mediaPlayer.CloseMedia();
//        sliderTimeline.value = 0;
//        sliderTimeline.enabled = false;
//        sliderVolume.enabled = false;
//    }

//    private void OnDestroy()
//    {
//        sliderTimelineEvent.onPointDown -= OnSliderTimelinePointDown;
//        sliderTimelineEvent.onPointUp -= OnSliderTimelinePointUp;

//        sliderVolumeEvent.onPointDown -= OnSliderVolumePointDown;
//        sliderVolumeEvent.onDrag -= OnSliderVolumeDrag;
//    }

//    public void LoadVideo(string path)
//    {
//        sliderTimeline.enabled = true;
//        sliderVolume.enabled = true;
//        mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, path, true);
//    }
//}