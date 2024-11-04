using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class UiLoadingComponentAwakeSystem : AwakeSystem<UILoadingComponent>
    {
        public override void Awake(UILoadingComponent self)
        {
            self.text = self.GetParent<UI>().GameObject.Get<GameObject>("text").GetComponent<Text>();
            self.slider = self.GetParent<UI>().GameObject.Get<GameObject>("slider").GetComponent<Slider>();
            self.StartAsync().Coroutine();
            GlobalComponent.Instance.uiLoading = self.GetParent<UI>().GameObject.transform;

            self.slider.value = 0;
            self.slider.DOValue(1, 3);

            EventDispatcher.AddObserver(this, EventName.LoadingCloseListen, (object[] info) =>
            {
                Log.Console("关闭Loading");
                UIHelper.Remove(self.ZoneScene(), UIType.UILoading).Coroutine();
                return false;
            }, null);
        }
    }

    public static class UiLoadingComponentSystem
    {
        public static async ETTask StartAsync(this UILoadingComponent self)
        {
            long instanceId = self.InstanceId;
            while (true)
            {
                await TimerComponent.Instance.WaitAsync(1000);

                if (self.InstanceId != instanceId)
                {
                    return;
                }
            }
        }
    }

    [ObjectSystem]
    public class LoadingDestroySystem: DestroySystem<UILoadingComponent>
    {
        public override void Destroy(UILoadingComponent self)
        {
            EventDispatcher.RemoveObserver(EventName.LoadingCloseListen);
        }
    }
}