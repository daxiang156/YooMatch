using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UIMBLoading)]
    public class UIMBLoadingEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer,object data = null)
        {
            await ETTask.CompletedTask;
            await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UIType.UIMBLoading.StringToAB());
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(UIType.UIMBLoading.StringToAB(), UIType.UIMBLoading);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.UILayers[(int)uiLayer]);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIMBLoading, gameObject);

            ui.AddComponent<UIMBLoadingComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            ResourcesComponent.Instance.UnloadBundle(UIType.UIMBLoading.StringToAB());
            uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadedResource.Remove(UIType.UIMBLoading.StringToAB());
        }
    }
}