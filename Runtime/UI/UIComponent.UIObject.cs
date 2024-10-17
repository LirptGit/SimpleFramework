using UnityEngine;

namespace SimpleFramework 
{
    public partial class UIComponent : SimpleFrameworkComponent 
    {
        [System.Serializable]
        public class UIObject : ObjectBase
        {
            public static UIObject Create(UIForm uiForm) 
            {
                UIObject uiObject = ReferencePool.Acquire<UIObject>();
                uiObject.Initialize(uiForm);
                return uiObject;
            }

            protected internal override void Release(bool isShutdown)
            {
                UIForm uiForm = Target as UIForm;
                Object.Destroy(uiForm.gameObject);
                ReferencePool.Release(this);
            }
        }
    }
}


