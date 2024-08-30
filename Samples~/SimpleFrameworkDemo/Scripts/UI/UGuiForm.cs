using UnityEngine;
using System.Collections;
using SimpleFramework;

namespace RPT
{
    public class UGuiForm : UIFormLogic
    {
        private const float FadeTime = 0.2f;

        private CanvasGroup m_CanvasGroup = null;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_CanvasGroup.alpha = 0f;

            StopAllCoroutines();
            StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                GameEntry.UI.CloseUGuiForm(this, null);
            }
            else
            {
                StartCoroutine(CloseCo(FadeTime));
            }
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f, duration);
            GameEntry.UI.CloseUGuiForm(this, null);
        }
    }
}

