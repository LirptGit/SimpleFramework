using UnityEngine;

namespace SimpleFramework 
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomSoundPlayerAB : MonoBehaviour
    {
        [System.Serializable]
        public struct ClipABInfo
        {
            public string abName;
            public string assetName;
        }

        public ClipABInfo[] clipABInfos;

        public AudioClip[] clips;

        public bool randomizePitch = false;
        public float pitchRange = 0.2f;

        private AudioSource m_Source;

        public bool isCanPlay
        {
            get
            {
                return m_Source.clip == null && !m_Source.isPlaying;
            }
        }

        private void Awake()
        {
            m_Source = GetComponent<AudioSource>();

            clips = new AudioClip[clipABInfos.Length];
            for (int i = 0; i < clips.Length; i++)
            {
                GameEntry.GetComponent<ResourceLoaderComponent>().LoadAssetAnsy<AudioClip>(clipABInfos[i].abName, clipABInfos[i].assetName, (UnityEngine.Object obj) =>
                {
                    clips[i] = obj as AudioClip;
                });
            }
        }

        public void PlayRandomSound()
        {
            AudioClip[] source = clips;

            int choice = Random.Range(0, source.Length);

            if (randomizePitch)
                m_Source.pitch = Random.Range(1.0f - pitchRange, 1.0f + pitchRange);

            m_Source.PlayOneShot(source[choice]);
        }

        public void Stop()
        {
            m_Source.Stop();
            m_Source.clip = null;
        }
    }
}


