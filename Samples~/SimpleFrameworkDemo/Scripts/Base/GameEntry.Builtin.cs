using UnityEngine;
using SimpleFramework;

/// <summary>
/// 游戏入口
/// </summary>
public partial class GameEntry : MonoBehaviour
{
    /// <summary>
    /// 获取游戏基础组件
    /// </summary>
    public static BaseComponent Base
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取事件组件
    /// </summary>
    public static EventComponent Event
    {
        get;
        private set;
    }


    /// <summary>
    /// 获取对象池组件
    /// </summary>
    public static ObjectPoolComponent Pool
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取资源加载器组件
    /// </summary>
    public static ResourceLoaderComponent Loader
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取场景组件
    /// </summary>
    public static SceneComponent Scene
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取声音组件
    /// </summary>
    public static SoundComponent Sound
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取ui组件
    /// </summary>
    public static UIComponent UI
    {
        get;
        private set;
    }

    private static void InitBuiltinComponents()
    {
        Base = SimpleFramework.GameEntry.GetComponent<BaseComponent>();
        Event = SimpleFramework.GameEntry.GetComponent<EventComponent>();
        Pool = SimpleFramework.GameEntry.GetComponent<ObjectPoolComponent>();
        Loader = SimpleFramework.GameEntry.GetComponent<ResourceLoaderComponent>();
        Scene = SimpleFramework.GameEntry.GetComponent<SceneComponent>();
        Sound = SimpleFramework.GameEntry.GetComponent<SoundComponent>();
        UI = SimpleFramework.GameEntry.GetComponent<UIComponent>();
    }
}
