using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UIGoldEffect)]
    public class UIGoldEffectEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer, object data = null)
        {
            await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UIType.UIGoldEffect.StringToAB());
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(UIType.UIGoldEffect.StringToAB(), UIType.UIGoldEffect);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.UILayers[(int)uiLayer]);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIGoldEffect, gameObject);

            ui.AddComponent<UIGoldEffectComponent,object>(data);
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadedResource.Remove(UIType.UIGoldEffect.StringToAB());
        }
    }
}