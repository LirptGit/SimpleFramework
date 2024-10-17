using UnityEngine;
using UnityEngine.Audio;

namespace SimpleFramework 
{
    [RequireComponent(typeof(AudioSource))]
    public class DefaultSoundHelper : MonoBehaviour
    {
        private AudioSource m_AudioSource;

        Transform targetObj;
        SoundObject soundObject;
        PlaySoundParams playSoundParams;


        public string ObjName
        {
            get => this.gameObject.name;
        }

        public SoundObject SoundObject 
        {
            get => soundObject;
        }

        public bool IsPlaying 
        {
            get => m_AudioSource.isPlaying;
        }

        public bool Loop
        {
            get => m_AudioSource.loop;
            set => m_AudioSource.loop = value;
        }

        public AudioMixerGroup OutputAudioMixerGroup 
        {
            get => m_AudioSource.outputAudioMixerGroup;
            private set => m_AudioSource.outputAudioMixerGroup = value;
        }

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.playOnAwake = false;
        }

        public void Play(AudioClip clip, PlaySoundParams playSoundParams, AudioMixerGroup audioMixerGroup, Transform target = null) 
        {
            Play(clip, null, playSoundParams, audioMixerGroup, target);
        }

        public void Play(AudioClip clip, SoundObject soundObject, PlaySoundParams playSoundParams, AudioMixerGroup audioMixerGroup, Transform target = null) 
        {
            if (soundObject != null)
            {
                this.soundObject = soundObject;
            }
            targetObj = target;

            this.gameObject.name = clip.name;
            this.playSoundParams = playSoundParams;

            SetParams();
            OutputAudioMixerGroup = audioMixerGroup;
            m_AudioSource.clip = clip;
            m_AudioSource.Play();

        }

        public void Stop() 
        {
            m_AudioSource.Stop();
            Reset();
        }

        public void Reset() 
        {
            ReferencePool.Release(playSoundParams);

            SetParams();
            m_AudioSource.clip = null;
            OutputAudioMixerGroup = null;
            targetObj = null;
            this.transform.position = Vector3.zero;
        }

        private void SetParams()
        {
            m_AudioSource.mute = playSoundParams.Mute;
            m_AudioSource.loop = playSoundParams.Loop;
            m_AudioSource.volume = playSoundParams.Volume;
            m_AudioSource.pitch = playSoundParams.Pitch;
            m_AudioSource.panStereo = playSoundParams.PanStereo;
            m_AudioSource.spatialBlend = playSoundParams.SpatialBlend;
            m_AudioSource.maxDistance = playSoundParams.MaxDistance;
            m_AudioSource.dopplerLevel = playSoundParams.DopplerLevel;
        }
    }
}