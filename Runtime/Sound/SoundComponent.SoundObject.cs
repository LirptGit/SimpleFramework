using UnityEngine;

namespace SimpleFramework 
{
    [System.Serializable]
    public class SoundObject : ObjectBase
    {
        public static SoundObject Create(object target)
        {
            SoundObject soundObject = ReferencePool.Acquire<SoundObject>();
            soundObject.Initialize(target);
            return soundObject;
        }

        protected internal override void OnSpawn()
        {
            DefaultSoundHelper sound = Target as DefaultSoundHelper;
            sound.gameObject.SetActive(true);
        }

        protected internal override void OnUnspawn()
        {
            DefaultSoundHelper sound = Target as DefaultSoundHelper;
            sound.gameObject.SetActive(false);
        }

        protected internal override void Release(bool isShutdown)
        {
            DefaultSoundHelper sound = Target as DefaultSoundHelper;
            Object.Destroy(sound.gameObject);
            ReferencePool.Release(this);
        }
    }
}