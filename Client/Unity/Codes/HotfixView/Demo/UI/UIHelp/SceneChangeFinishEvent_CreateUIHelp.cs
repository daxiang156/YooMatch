namespace ET
{
    public class SceneChangeFinishEvent_CreateUIHelp : AEvent<EventType.SceneChangeFinish>
    {
        protected override async ETTask Run(EventType.SceneChangeFinish args)
        {
            //await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGame, UILayer.Mid);
            await  ETTask.CompletedTask;
        }
    }
}
