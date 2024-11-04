namespace ET
{
    [MessageHandler]
    public class M2C_MBHouseStartHandle : AMHandler<M2C_HouseStart>
    {
        protected override async ETTask Run(Session session, M2C_HouseStart message)
        {
            MBMoreDataComponent.Instance.StartTime = message.StartTime;
            await ETTask.CompletedTask;
        }
    }
}