namespace ET
{
    public class LoadingFinishEvent_RemoveLoadingUI : AEvent<EventType.LoadingFinish>
    {
        protected override async ETTask Run(EventType.LoadingFinish args)
        {
            //if(UIHelper.Get(args.Scene, UIType.UILoading) != null)
                await UIHelper.Remove(args.Scene, UIType.UILoading);
            await ETTask.CompletedTask;
        }
    }
}
