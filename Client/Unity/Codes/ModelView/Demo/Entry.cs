using System;
using System.Runtime.CompilerServices;

namespace ET
{
	public static class Entry
	{
		public static void Start()
		{
			try
			{
				Log.Info("entry start");
				CodeLoader.Instance.Update += Game.Update;
				CodeLoader.Instance.LateUpdate += Game.LateUpdate;
				CodeLoader.Instance.OnApplicationQuit += Game.Close;

				
				Game.EventSystem.Add(CodeLoader.Instance.GetTypes());

				
				Game.EventSystem.Publish(new EventType.AppStart());
				
				// CodeLoader.Instance.EventPublic += () =>
				// {
				// 	// Game.EventSystem.Publish(new EventType.MonoResult()
				// 	// {
				// 	// 	ZoneScene = Game.Scene.GetComponent<ZoneSceneManagerComponent>().ZoneScenes[1]
				// 	// });
				// };

				// CodeLoader.Instance.Event2ET += () => { 
				// 	EventListen();
				// };
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void EventListen()
		{
			// Scene ZoneScene = Game.Scene.GetComponent<ZoneSceneManagerComponent>().ZoneScenes[1];
			// int id = GameDataMgr.Instance.eventId;
			// object paras = GameDataMgr.Instance.paras;
			// switch (id)
			// {
			// 	case EventId.CloseWaitingUI:
			// 		//Game.EventSystem.PublishAsync(new EventType.LoadingFinish() { Scene = ZoneScene,}).Coroutine();
			// 		EventDispatcher.PostEvent(EventName.LoadingCloseListen, null);
			// 		//Game.EventSystem.PublishAsync(new EventType.IsShowMainCity() { ZoneScene = ZoneScene, isShow = true}).Coroutine();
			// 		break;
			// 	default:
			// 		break;
			// }
		}
	}
}