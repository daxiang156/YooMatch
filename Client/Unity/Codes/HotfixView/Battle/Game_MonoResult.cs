namespace ET
{
    public class Game_MonoResult: AEvent<EventType.MonoResult>
    {
        protected override async ETTask Run(EventType.MonoResult args)
        {
            //HallHelper.GameResult(args.ZoneScene, 1);
            await ETTask.CompletedTask;
        }
    }
}