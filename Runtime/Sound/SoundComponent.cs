using UnityEngine;

namespace SimpleFramework
{
    public partial class SoundComponent : SimpleFrameworkComponent
    {
        [Header("Music Settings")]
        [SerializeField] private AudioSource m_MusicAudioSource;

        [Header("Voice Settings")]
        [SerializeField] private AudioSource m_VoiceAudioSource;

        [Header("UI Settings")]
        [SerializeField] private RandomSoundPlayer[] m_UIAudioSource;

        private AudioListener m_AudioListener = null;

        /// <summary>
        /// 游戏框架组件初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_AudioListener = gameObject.GetOrAddComponent<AudioListener>();
        }

        public void PlayMusic(AudioClip clip) 
        {
            m_MusicAudioSource.clip = clip;
            m_MusicAudioSource.Play();
        }

        public void StopMusic()
        {
            m_MusicAudioSource.Stop();
        }

        public void PlayVoice(AudioClip clip)
        {
            m_VoiceAudioSource.clip = clip;
            m_VoiceAudioSource.Play();
        }

        public void StopVoice()
        {
            m_VoiceAudioSource.Stop();
        }

        public void PlayUIClick() 
        {
            for (int i = 0; i < m_UIAudioSource.Length; i++)
            {
                if (m_UIAudioSource[i].isCanPlay)
                {
                    m_UIAudioSource[i].PlayRandomSound();
                    break;
                }
            }
        }

        private void Update()
        {
            RefreshAudioListener();
        }

        /// <summary>
        /// 刷新声音监听者
        /// </summary>
        private void RefreshAudioListener()
        {
            m_AudioListener.enabled = FindObjectsOfType<AudioListener>().Length <= 1;
        }
    }
}
