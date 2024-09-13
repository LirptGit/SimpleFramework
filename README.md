# 框架包含内容
1. 【ReferencePool】缓存池           
2. 【ObjectPool】对象池
3. 【ResourceLoaderComponent】AB包加载器
4. 【SoundComponent】声音（建议使用Assetbundle异步加载）

**注：AudioMixerGroupName枚举，这里是根据声音混合器的名字来写的，可自行扩展**
6. 【UIComponent】UI（建议使用Assetbundle同步加载）**注：UI组的名称按照场景分配：卸载场景时，提供了根据组名称卸载当前组里所有的UI面板资源**
7. 【SceneComponent】场景 注：场景若需要Assetbundle包加载的话，请先从加载器中加载对应的AB包，在调用
8. 【BT】AI
9. 【Event】事件
10. 【Epplus】Excel加载的dll文件
11. 【Lijson】源码
12. 【Player】简单的状态机示例
13. 【MouseLook】相机原地旋转
14. 【Draggable】2D 3D物体的拖拽 （3D物体拖拽 需在相机上添加PhysicsRaycaster脚本）
15. 框架依赖于：assetbundle browser 插件（自动会安装）

# 文档
https://www.lirpt.com/index.php/2024/09/06/tool_003/
