namespace ET
{
    public class GameFinish_GameResule: AEvent<EventType.GameResult>
    {
        protected override async ETTask Run(EventType.GameResult args)
        {
            //SceneChangeHelper.SceneChangeTo(args.ZoneScene, "Init", 11).Coroutine();
           // CurrentScenesComponent currentScenesComponent = args.ZoneScene.GetComponent<CurrentScenesComponent>();
            //var battleScene = currentScenesComponent.Scene.GetComponent<BattleComponent>();
            //battleScene.Dispose();
           // currentScenesComponent.Scene?.Dispose();
            //Game.Scene.GetComponent<HallInfoComponent>().cameraObj.SetActive(true);

            //UI ui = await UIHelper.Create(args.ZoneScene, UIType.UIWin, UILayer.Mid);
            
            
            
            //await SceneChangeHelper.SceneChangeTo(args.ZoneScene, "City01", 13);
            int MapId = 3;
            Log.Console("回主城2");
            MapConfig config = MapConfigCategory.Instance.Get(MapId);
            await SceneChangeHelper.SceneChangeTo(args.ZoneScene, config.source, MapId);
            SoundComponent.Instance.PlayBgMusic_Res("Music/bgm");
            
            await UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIGame);
            //await UIHelper.Create(GlobalComponent.Instance.scene,UIType.UIMainCity, UILayer.Low2);
        }
    }
}