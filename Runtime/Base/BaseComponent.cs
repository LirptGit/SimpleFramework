using UnityEngine;

namespace SimpleFramework
{
    /// <summary>
    /// 基础组件。
    /// </summary>
    public class BaseComponent : SimpleFrameworkComponent
    {
        [Range(1, 120)]
        [SerializeField]
        private int m_FrameRate = 60;

        /// <summary>
        /// 获取或设置游戏帧率。
        /// </summary>
        public int FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                Application.targetFrameRate = m_FrameRate = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = m_FrameRate;
        }
    }
}

