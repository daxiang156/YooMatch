using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UIMBMore)]
    public class UIMBMoreEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer, object data = null)
        {
            await ETTask.CompletedTask;
            await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UIType.UIMBMore.StringToAB());
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(UIType.UIMBMore.StringToAB(), UIType.UIMBMore);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.UILayers[(int)uiLayer]);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIMBMore, gameObject);

            ui.AddComponent<UIMBMoreComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            ResourcesComponent.Instance.UnloadBundle(UIType.UIMBMore.StringToAB());
            uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadedResource.Remove(UIType.UIMBMore.StringToAB());
        }
    }
}