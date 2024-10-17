using UnityEngine;

namespace SimpleFramework 
{
    public class PlaySoundParams : IReference
    {
        private bool m_Mute;
        private bool m_Loop;
        private float m_Volume;
        private float m_FadeInSeconds;
        private float m_Pitch;
        private float m_PanStereo;
        private float m_SpatialBlend;
        private float m_MaxDistance;
        private float m_DopplerLevel;

        /// <summary>
        /// ��ʼ������������������ʵ����
        /// </summary>
        public PlaySoundParams()
        {
            m_Mute = false;
            m_Loop = false;
            m_Volume = 1f;
            m_Pitch = 1f;
            m_PanStereo = 0f;
            m_SpatialBlend = 0f;
            m_MaxDistance = 100f;
            m_DopplerLevel = 1f;
        }

        /// <summary>
        /// ��ȡ�����������������Ƿ�����
        /// </summary>
        public bool Mute
        {
            get
            {
                return m_Mute;
            }
            set
            {
                m_Mute = value;
            }
        }

        /// <summary>
        /// ��ȡ�������Ƿ�ѭ�����š�
        /// </summary>
        public bool Loop
        {
            get
            {
                return m_Loop;
            }
            set
            {
                m_Loop = value;
            }
        }

        /// <summary>
        /// ��ȡ����������������������С��
        /// </summary>
        public float Volume
        {
            get
            {
                return m_Volume;
            }
            set
            {
                m_Volume = value;
            }
        }

        /// <summary>
        /// ��ȡ��������������ʱ�䣬����Ϊ��λ��
        /// </summary>
        public float FadeInSeconds
        {
            get
            {
                return m_FadeInSeconds;
            }
            set
            {
                m_FadeInSeconds = value;
            }
        }

        /// <summary>
        /// ��ȡ����������������
        /// </summary>
        public float Pitch
        {
            get
            {
                return m_Pitch;
            }
            set
            {
                m_Pitch = value;
            }
        }

        /// <summary>
        /// ��ȡ�������������������ࡣ
        /// </summary>
        public float PanStereo
        {
            get
            {
                return m_PanStereo;
            }
            set
            {
                m_PanStereo = value;
            }
        }

        /// <summary>
        /// ��ȡ�����������ռ�������
        /// </summary>
        public float SpatialBlend
        {
            get
            {
                return m_SpatialBlend;
            }
            set
            {
                m_SpatialBlend = value;
            }
        }

        /// <summary>
        /// ��ȡ���������������롣
        /// </summary>
        public float MaxDistance
        {
            get
            {
                return m_MaxDistance;
            }
            set
            {
                m_MaxDistance = value;
            }
        }

        /// <summary>
        /// ��ȡ���������������յȼ���
        /// </summary>
        public float DopplerLevel
        {
            get
            {
                return m_DopplerLevel;
            }
            set
            {
                m_DopplerLevel = value;
            }
        }

        /// <summary>
        /// ������������������
        /// </summary>
        /// <returns>�����Ĳ�������������</returns>
        public static PlaySoundParams Create()
        {
            PlaySoundParams playSoundParams = ReferencePool.Acquire<PlaySoundParams>();
            return playSoundParams;
        }

        /// <summary>
        /// ����������������
        /// </summary>
        public void Clear()
        {
            m_Mute = false;
            m_Loop = false;
            m_Volume = 1f;
            m_Pitch = 1f;
            m_PanStereo = 0f;
            m_SpatialBlend = 0f;
            m_MaxDistance = 100f;
            m_DopplerLevel = 1f;
        }
    }
}