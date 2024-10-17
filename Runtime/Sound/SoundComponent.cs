using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SimpleFramework
{
    /// <summary>
    /// ������������Ҫ����һ��
    /// </summary>
    public enum AudioMixerGroupName 
    {
        Master,
        Music,
        UI,
        Voice
    }

    public partial class SoundComponent : SimpleFrameworkComponent
    {
        [Header("Sound Instances Root")]
        [SerializeField] private Transform m_Root;

        [Header("Resource Loader")]
        [SerializeField] private ResourceLoaderComponent m_Loader = null;

        [Header("AB����Ϣ����")]
        [SerializeField] private LoaderType loaderType;
        [SerializeField] private string abLocalPath;
        [SerializeField] private string abName;
        [SerializeField] private string mainabName;
        [SerializeField] private string assetSuffix;

        [Header("Object Pool")]
        [SerializeField] private ObjectPoolComponent m_Pool = null;
        [SerializeField] private int capacity = 30;
        private IObjectPool<SoundObject> soundPool;

        [SerializeField]
        private AudioMixer m_AudioMixer = null;

        private AudioListener m_AudioListener = null;

        private readonly List<DefaultSoundHelper> playSoundCache = new List<DefaultSoundHelper>();
        private readonly List<DefaultSoundHelper> releaseSoundCache = new List<DefaultSoundHelper>();
        private readonly Queue<DefaultSoundHelper> releaseSoundQueue = new Queue<DefaultSoundHelper>();

        /// <summary>
        /// ��Ϸ��������ʼ��
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_AudioListener = gameObject.GetOrAddComponent<AudioListener>();
            soundPool = m_Pool.CreateObjectPool<SoundObject>("SoundPool", capacity);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="assetName">��Դ����</param>
        /// <param name="outputName">�����������</param>
        public void PlaySound(string assetName, AudioMixerGroupName outputName, Transform target = null) 
        {
            PlaySound(assetName, outputName, null, false, target);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="clip">����</param>
        /// <param name="outputName">�����������</param>
        public void PlaySound(AudioClip clip, AudioMixerGroupName outputName, Transform target = null)
        {
            PlaySound(clip, outputName, null, false, target);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="assetName">��Դ����</param>
        /// <param name="outputName">�����������</param>
        /// <param name="isAutoReplace">�Ƿ���Ҫ�滻</param>
        public void PlaySound(string assetName, AudioMixerGroupName outputName, bool isAutoReplace, Transform target = null) 
        {
            PlaySound(assetName, outputName, null, isAutoReplace, target);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="clip">����</param>
        /// <param name="outputName">�����������</param>
        /// <param name="isAutoReplace">�Ƿ���Ҫ�滻</param>
        public void PlaySound(AudioClip clip, AudioMixerGroupName outputName, bool isAutoReplace, Transform target = null)
        {
            PlaySound(clip, outputName, null, isAutoReplace, target);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="assetName">��Դ����</param>
        /// <param name="outputName">�����������</param>
        /// <param name="playSoundParams">���������Ĳ���</param>
        /// <param name="isAutoReplace">�Ƿ���Ҫ�滻</param>
        public void PlaySound(string assetName, AudioMixerGroupName outputName, PlaySoundParams playSoundParams, bool isAutoReplace, Transform target = null)
        {
            if (playSoundParams == null)
            {
                playSoundParams = PlaySoundParams.Create();
            }

            LoadAudioClip(assetName, outputName, playSoundParams, isAutoReplace, target);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="clip">����</param>
        /// <param name="outputName">�����������</param>
        /// <param name="playSoundParams">���������Ĳ���</param>
        /// <param name="isAutoReplace">�Ƿ���Ҫ�滻</param>
        public void PlaySound(AudioClip clip, AudioMixerGroupName outputName, PlaySoundParams playSoundParams, bool isAutoReplace, Transform target = null)
        {
            if (playSoundParams == null)
            {
                playSoundParams = PlaySoundParams.Create();
            }

            LoadFinishedCallBack(clip, outputName, playSoundParams, isAutoReplace, target);
        }

        /// <summary>
        /// �����ض���ĳ������
        /// </summary>
        /// <param name="assetName">��Դ����</param>
        public void StopSound(string assetName)
        {
            for (int i = 0; i < playSoundCache.Count; i++)
            {
                if (playSoundCache[i].ObjName == assetName)
                {
                    playSoundCache[i].Stop();
                }
            }
        }

        /// <summary>
        /// �ͷ�������Դ
        /// </summary>
        /// <param name="assetName">��Դ����</param>
        public void ReleaseSound(string assetName) 
        {
            for (int i = 0; i < releaseSoundCache.Count; i++)
            {
                if (releaseSoundCache[i].ObjName == assetName)
                {
                    releaseSoundQueue.Enqueue(releaseSoundCache[i]);
                }
            }
        }

        private void Update()
        {
            RefreshAudioListener();

            for (int i = 0; i < playSoundCache.Count; i++)
            {
                DefaultSoundHelper sound = playSoundCache[i];
                if (sound.Loop)
                {
                    continue;
                }
                if (!sound.IsPlaying)
                {
                    sound.Reset();
                    soundPool.Unspawn(sound.SoundObject);
                    playSoundCache.Remove(sound);
                }
            }

            if (releaseSoundQueue.Count > 0)
            {
                DefaultSoundHelper sound = releaseSoundQueue.Dequeue();
                if (soundPool.ReleaseObject(sound.SoundObject))
                {
                    releaseSoundCache.Remove(sound);
                }
            }
        }

        /// <summary>
        /// ˢ������������
        /// </summary>
        private void RefreshAudioListener()
        {
            m_AudioListener.enabled = FindObjectsOfType<AudioListener>().Length <= 1;
        }

        /// <summary>
        /// ����������Դ
        /// </summary>
        private void LoadAudioClip(string assetName, AudioMixerGroupName outputName, PlaySoundParams playSoundParams, bool isAutoReplace, Transform target = null) 
        {
            LoaderABTask abTask = LoaderABTask.Create(loaderType, CommonPath.GetABPath(abLocalPath), abName, typeof(AudioClip), assetName + assetSuffix, mainabName);

            m_Loader.AddAsset(abTask, (UnityEngine.Object obj) =>
            {
                if (obj == null)
                {
                    Debug.LogError($"ab�����ص�����Ϊ�գ�����SoundCompontent�ű��ϵ�AB����Ϣ�����Ƿ���ȷ");
                }

                LoadFinishedCallBack(obj, outputName, playSoundParams, isAutoReplace, target);
            });
        }

        /// <summary>
        /// ��Դ������ɵĻص�����
        /// </summary>
        private void LoadFinishedCallBack(UnityEngine.Object obj, AudioMixerGroupName outputName, PlaySoundParams playSoundParams, bool isAutoReplace, Transform target = null)
        {
            AudioClip clip = (AudioClip)obj;

            AudioMixerGroup audioMixerGroup = GetAudioMixerGroup(outputName.ToString());

            if (isAutoReplace)
            {
                for (global::System.Int32 i = 0; i < playSoundCache.Count; i++)
                {
                    if (playSoundCache[i].OutputAudioMixerGroup.name == outputName.ToString())
                    {
                        playSoundCache[i].Play(clip, playSoundParams, audioMixerGroup, target);
                        return;
                    }
                }
            }

            SoundObject soundObject = CreateSoundPrefab();
            if (soundObject == null)
            {
                return;
            }
            DefaultSoundHelper sound = soundObject.Target as DefaultSoundHelper;
            sound.Play(clip, soundObject, playSoundParams, audioMixerGroup, target);
            playSoundCache.Add(sound);
        }

        /// <summary>
        /// �����ͻ�ȡSoundObject
        /// </summary>
        /// <returns></returns>
        private SoundObject CreateSoundPrefab() 
        {
            SoundObject soundObject = soundPool.Spawn();
            if (soundObject == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.transform.parent = m_Root;

                DefaultSoundHelper sound = gameObject.GetOrAddComponent<DefaultSoundHelper>();
                soundObject = SoundObject.Create(sound);
                if (!soundPool.Register(soundObject, true))
                {
                    Destroy(gameObject);
                    ReferencePool.Release(soundObject);
                    soundObject = null;
                }
                releaseSoundCache.Add(sound);
            }
            return soundObject;
        }

        /// <summary>
        /// ��ȡָ�������������
        /// </summary>
        /// <param name="outputName"></param>
        /// <returns></returns>
        private AudioMixerGroup GetAudioMixerGroup(string outputName) 
        {
            if (m_AudioMixer != null)
            {
                AudioMixerGroup[] audioMixerGroups = m_AudioMixer.FindMatchingGroups(($"Master/{outputName}"));
                if (audioMixerGroups.Length > 0)
                {
                    return audioMixerGroups[0];
                }
                else
                {
                    return m_AudioMixer.FindMatchingGroups("Master")[0];
                }
            }
            return null;
        }
    }
}