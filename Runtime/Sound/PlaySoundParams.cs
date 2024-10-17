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
        /// 初始化播放声音参数的新实例。
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
        /// 获取或设置在声音组内是否静音。
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
        /// 获取或设置是否循环播放。
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
        /// 获取或设置在声音组内音量大小。
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
        /// 获取或设置声音淡入时间，以秒为单位。
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
        /// 获取或设置声音音调。
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
        /// 获取或设置声音立体声声相。
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
        /// 获取或设置声音空间混合量。
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
        /// 获取或设置声音最大距离。
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
        /// 获取或设置声音多普勒等级。
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
        /// 创建播放声音参数。
        /// </summary>
        /// <returns>创建的播放声音参数。</returns>
        public static PlaySoundParams Create()
        {
            PlaySoundParams playSoundParams = ReferencePool.Acquire<PlaySoundParams>();
            return playSoundParams;
        }

        /// <summary>
        /// 清理播放声音参数。
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