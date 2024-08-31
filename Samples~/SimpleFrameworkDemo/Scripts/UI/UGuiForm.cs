using UnityEngine;
using System.Collections;
using SimpleFramework;

namespace SimpleFrameworkDemo
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
			StartCoroutine(FadeToAlpha(m_CanvasGroup, 1f, FadeTime));
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
				GameEntry.UI.CloseUIForm(this.UIForm, null);
			}
			else
			{
				StartCoroutine(CloseCo(FadeTime));
			}
		}
	
		private IEnumerator CloseCo(float duration)
		{
			yield return FadeToAlpha(m_CanvasGroup, 0f, duration);
			GameEntry.UI.CloseUIForm(this.UIForm, null);
		}
	
		/// <summary>
		/// ui面板渐隐渐显的动画
		/// </summary>
		/// <param name="canvasGroup">动画组件</param>
		/// <param name="alpha">目标透明度值</param>
		/// <param name="duration">需要的时间</param>
		/// <returns></returns>
		private IEnumerator FadeToAlpha(CanvasGroup canvasGroup, float alpha, float duration)
		{
			float time = 0f;
			float originalAlpha = canvasGroup.alpha;
			while (time < duration)
			{
				time += Time.deltaTime;
				canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
				yield return new WaitForEndOfFrame();
			}
	
			canvasGroup.alpha = alpha;
		}
	}
	
}

