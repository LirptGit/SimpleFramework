using SimpleFramework;
using UnityEngine;

namespace SimpleFrameworkDemo
{
	/// <summary>
	/// 游戏入口。
	/// </summary>
	public partial class GameEntry : MonoBehaviour
	{
		public string msg = "游戏入口";

		private void Start()
		{
			InitBuiltinComponents();
			InitCustomComponents();

			Sound.PlaySound("卡农", AudioMixerGroupName.Music);
        }
    }
}