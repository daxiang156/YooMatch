using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UIGuide)]
    public class UIGuideEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer, object data = null)
        {
            try
            {
                await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UIType.UIGuide.StringToAB());
                GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset(UIType.UIGuide.StringToAB(), UIType.UIGuide);
                GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.UILayers[(int)uiLayer]);
                UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIGuide, gameObject);

                ui.AddComponent<UIGuideComponent,object>(data);
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadedResource.Remove(UIType.UIGuide.StringToAB());
            ResourcesComponent.Instance.UnloadBundle(UIType.UIGuide.StringToAB());
        }
    }
}