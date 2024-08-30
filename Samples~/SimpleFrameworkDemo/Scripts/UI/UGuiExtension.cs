using SimpleFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UGuiExtension
{

    private static ItemManager uiFormInfo;


    /// <summary>
    /// ��ui���
    /// </summary>
    /// <param name="uiComponent">ui���</param>
    /// <param name="uiFormName">ui��������</param>
    /// <param name="userDate">�Զ��������</param>
    public static void OpenUGuiForm(this UIComponent uiComponent, int uiFormID, object userDate)
    {
        List<string> infos = new List<string>();

        if (uiFormInfo == null)
        {
            uiFormInfo = Resources.Load<ItemManager>("AssetsFile/UIFormInfo");
        }

        foreach (ExItem exItem in uiFormInfo.dataArray)
        {
            if (exItem.itemId == uiFormID)
            {
                infos = exItem.itemData;
            }
        }

        uiComponent.OpenUIForm(infos[3], infos[4], userDate);
    }

    /// <summary>
    /// ��ȡui����߼���
    /// </summary>
    /// <param name="uiComponent"></param>
    /// <param name="uiFormName"></param>
    /// <returns></returns>
    public static UGuiForm GetUGuiForm(this UIComponent uiComponent, int uiFormID)
    {
        foreach (ExItem exItem in uiFormInfo.dataArray)
        {
            if (exItem.itemId == uiFormID)
            {
                UIForm uiForm = (UIForm)uiComponent.GetUIForm(exItem.itemData[3]);
                return (UGuiForm)uiForm.Logic;
            }
        }

        return null;
    }

    /// <summary>
    /// �ر�ui���
    /// </summary>
    /// <param name="uiComponent"></param>
    /// <param name="uiForm"></param>
    public static void CloseUGuiForm(this UIComponent uiComponent, UGuiForm uiForm, object userData)
    {
        uiComponent.CloseUIForm(uiForm.UIForm, userData);
    }

    /// <summary>
    /// ui��彥�����ԵĶ���
    /// </summary>
    /// <param name="canvasGroup">�������</param>
    /// <param name="alpha">Ŀ��͸����ֵ</param>
    /// <param name="duration">��Ҫ��ʱ��</param>
    /// <returns></returns>
    public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
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