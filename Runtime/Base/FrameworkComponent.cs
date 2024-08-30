using UnityEngine;

namespace SimpleFramework
{
    public abstract class SimpleFrameworkComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            GameEntry.RegisterComponent(this);
        }
    }
}

