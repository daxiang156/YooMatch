namespace ET
{
    public class CreateTip: AEvent<EventType.TipErrorCode>
    {
        protected override async ETTask Run(EventType.TipErrorCode args)
        {
            UIHelper.ShowTip(args.ZoneScene, args.errorId);
            await ETTask.CompletedTask;
        }
    }
}