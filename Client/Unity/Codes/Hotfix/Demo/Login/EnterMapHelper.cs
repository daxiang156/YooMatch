using System;


namespace ET
{
    public static class EnterMapHelper
    {
        public static async ETTask EnterMapAsync(Scene zoneScene)
        {
            try
            {
                G2C_EnterMap g2CEnterMap = await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2G_EnterMap()) as G2C_EnterMap;
                zoneScene.GetComponent<PlayerComponent>().MyId = g2CEnterMap.MyId;
                
                // 等待场景切换完成
                await zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_SceneChangeFinish>();
                
                Game.EventSystem.Publish(new EventType.EnterMapFinish() {ZoneScene = zoneScene});
            }
            catch (Exception e)
            {
                Log.Error(e);
            }	
        }
        /// <summary>
        /// start change map
        /// </summary>
        /// <returns></returns>
        public static async ETTask EnterMapAsync(Scene scene, int mapId)
        {
            ClientSceneConfig cfg = ClientSceneConfigCategory.Instance.Get(mapId);
            if (cfg == null)
            {
                Log.Error("mapId: " + mapId + " is error!!");
                return;
            }
            C2G_EnterMap cmd = new C2G_EnterMap();
            cmd.MapId = mapId;
            G2C_EnterMap g2CEnterMap = await scene.GetComponent<SessionComponent>().Session.Call(cmd) as G2C_EnterMap;
            scene.GetComponent<PlayerComponent>().MyId = mapId;
            //wait map change finish
            await scene.GetComponent<ObjectWait>().Wait<WaitType.Wait_SceneChangeFinish>();
            Game.EventSystem.Publish(new EventType.EnterMapFinish(){ ZoneScene = scene});
        }
    }
}