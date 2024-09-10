using UnityEngine;

namespace SimpleFramework
{
    public partial class SoundComponent : SimpleFrameworkComponent
    {
        [Header("Music Settings")]
        [SerializeField] private AudioSource m_MusicAudioSource;

        [Header("Voice Settings")]
        [SerializeField] private AudioSource m_VoiceAudioSource;

        private AudioListener m_AudioListener = null;

        /// <summary>
        /// ��Ϸ��������ʼ��
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

        private void Update()
        {
            RefreshAudioListener();
        }

        /// <summary>
        /// ˢ������������
        /// </summary>
        private void RefreshAudioListener()
        {
            m_AudioListener.enabled = FindObjectsOfType<AudioListener>().Length <= 1;
        }
    }
}
