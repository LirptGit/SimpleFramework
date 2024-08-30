using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SimpleFramework 
{
    public class UIBtnClickSoundPlayer : MonoBehaviour
    {
        private Button m_Btn;

        private void Awake()
        {
            m_Btn = GetComponent<Button>();

            m_Btn.onClick.AddListener(() => 
            {
                GameEntry.GetComponent<SoundComponent>().PlayUIClick();
            });
        }
    }
}