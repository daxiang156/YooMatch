using UnityEngine;

namespace ET
{
    public class SceneChangeStart_AddComponent: AEvent<EventType.SceneChangeStart>
    {
        protected override async ETTask Run(EventType.SceneChangeStart args)
        {
            Scene currentScene = args.ZoneScene.CurrentScene();
            // 加载场景资源
            await ResourcesComponent.Instance.LoadBundleAsync($"{currentScene.Name}.unity3d");
            // 切换到map场景

            SceneChangeComponent sceneChangeComponent = null;
            try
            {
                sceneChangeComponent = Game.Scene.GetComponent<SceneChangeComponent>();
                if (sceneChangeComponent == null)
                {
                    sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>();
                }
                {
                    await sceneChangeComponent.ChangeSceneAsync(currentScene.Name);
                }
                GameObject UIUIBg = GameObject.Find("/Global/UI/Low2/UIUIBg");
                UIUIBg?.SetActive(false);
                EventDispatcher.PostEvent(EventName.ChangeSceneFinish, null, currentScene.Id);
            }
            finally
            {
                sceneChangeComponent?.Dispose();
            }
			

            currentScene.AddComponent<OperaComponent>();
        }
    }
}