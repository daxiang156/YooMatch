using UnityEngine;

namespace ET
{
    public class LoadingBeginEvent_CreateLoadingUI : AEvent<EventType.LoadingBegin>
    {
        protected override async ETTask Run(EventType.LoadingBegin args)
        {
            Log.Console("显示加载页");
            await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UILoading, UILayer.Mid);
        }
    }
}
