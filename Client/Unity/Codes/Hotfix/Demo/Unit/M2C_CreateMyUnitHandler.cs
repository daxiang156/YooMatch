using ET;

namespace ET
{
	[MessageHandler]
	public class M2C_CreateMyUnitHandler : AMHandler<M2C_CreateMyUnit>
	{
		protected override async ETTask Run(Session session, M2C_CreateMyUnit message)
		{
			// 通知场景切换协程继续往下走
			session.DomainScene().GetComponent<ObjectWait>().Notify(new WaitType.Wait_CreateMyUnit() {Message = message});
			// var hallInfo = Game.Scene.GetComponent<HallInfoComponent>();
			// if (hallInfo == null)
			// {
			// 	hallInfo = Game.Scene.AddComponent<HallInfoComponent>();
			// }
			//hallInfo.platFormCoin = message.Unit.Ks[1004];
			EventDispatcher.PostEvent(EventName.InitHallInfo, null, message);
			//HallInfoComponent.Instance.InitHallInfo(message);
			await ETTask.CompletedTask;
		}
	}
}
