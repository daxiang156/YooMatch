using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UINotice)]
    public class UINoticeEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer,object data = null)
        {
            await ETTask.CompletedTask;
            await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UIType.UINotice.StringToAB());
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(UIType.UINotice.StringToAB(), UIType.UINotice);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.UILayers[(int)uiLayer]);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UINotice, gameObject);

            ui.AddComponent<UINoticeComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            ResourcesComponent.Instance.UnloadBundle(UIType.UINotice.StringToAB());
            uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadedResource.Remove(UIType.UINotice.StringToAB());
        }
    }
}