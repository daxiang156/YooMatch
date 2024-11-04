using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UIGM)]
    public class UIGMEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer, object data = null)
        {
            await ETTask.CompletedTask;
            await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UIType.UIGM.StringToAB());
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(UIType.UIGM.StringToAB(), UIType.UIGM);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.UILayers[(int)uiLayer]);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIGM, gameObject);

            ui.AddComponent<UIGMComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            ResourcesComponent.Instance.UnloadBundle(UIType.UIGM.StringToAB());
            uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadedResource.Remove(UIType.UIGM.StringToAB());
        }
    }
}