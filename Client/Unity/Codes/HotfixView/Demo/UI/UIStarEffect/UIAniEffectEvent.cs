using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UIAniEffect)]
    public class UIStarEffectEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer, object data = null)
        {
            await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UIType.UIAniEffect.StringToAB());
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(UIType.UIAniEffect.StringToAB(), UIType.UIAniEffect);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.UILayers[(int)uiLayer]);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIAniEffect, gameObject);

            ui.AddComponent<UIAniEffectComponent,object>(data);
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            
        }
    }
}