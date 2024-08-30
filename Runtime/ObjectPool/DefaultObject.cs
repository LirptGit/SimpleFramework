using UnityEngine;

namespace SimpleFramework
{
    public class DefaultObject : ObjectBase
    {
        public static DefaultObject Create(GameObject target)
        {
            DefaultObject defaultObject = ReferencePool.Acquire<DefaultObject>();
            defaultObject.Initialize(target);
            return defaultObject;
        }

        protected internal override void OnSpawn()
        {
            GameObject target = (GameObject)Target;
            target.SetActive(true);
        }

        protected internal override void OnUnspawn()
        {
            GameObject target = (GameObject)Target;
            target.SetActive(false);
        }

        protected internal override void Release(bool isShutdown)
        {
            Object.Destroy((Object)Target);
        }
    }
}