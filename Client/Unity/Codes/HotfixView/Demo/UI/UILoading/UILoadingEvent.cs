using System;
using UnityEngine;

namespace ET
{
	[UIEvent(UIType.UILoading)]
    public class UILoadingEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent, UILayer uiLayer, object data = null)
        {
	        try
	        {
		        Log.Console("打开Loading");
		        await uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadAsync(UIType.UILoading.StringToAB());
		        GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset(UIType.UILoading.StringToAB(), UIType.UILoading);
		        GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, UIEventComponent.Instance.UILayers[(int)uiLayer]);
		        UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UILogin, gameObject);
		        ui.AddComponent<UILoadingComponent>();
		        
		        await ETTask.CompletedTask;
				//GameObject bundleGameObject = ((GameObject)Resources.Load("KV")).Get<GameObject>(UIType.UILoading);
				//GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
				//go.layer = LayerMask.NameToLayer(LayerNames.UI);
				//UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UILoading, go);

				//ui.AddComponent<UILoadingComponent>();
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
	        uiComponent.Domain.GetComponent<ResourcesLoaderComponent>().LoadedResource.Remove(UIType.UILoading.StringToAB());
	        ResourcesComponent.Instance.UnloadBundle(UIType.UILoading.StringToAB());
        }
    }
}