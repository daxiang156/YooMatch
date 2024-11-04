using System;
using System.Net;
using Assets.Mono.DownLoad;
using ILRuntime.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace ET
{
    public class AppStart_Init: AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Log.Console("AppStart_Init");
            //EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Event_ET_Start);
            //Log.Console("FireBase Init...Start");
            Game.Scene.AddComponent<FireBComponent>();
            Log.Console("FireBase Init...async");
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();

            Log.Console("FireBase Init...async2");
            // 加载配置
            Game.Scene.AddComponent<ResourcesComponent>();
            Log.Console("config 1");
            await ResourcesComponent.Instance.LoadBundleAsync("config.unity3d");

            Game.Scene.AddComponent<MBDataComponent>();
            Game.Scene.AddComponent<ConfigComponent>();
            //ConfigComponent.Instance.Load();
            //Type mblv = ConfigComponent.Instance.GetTypeByName(curmblvName);
            // ConfigComponent.Instance.LoadOneConfig(mblv);
            // ConfigComponent.Instance.LoadNeedConfig();
            Log.Console("config 加载完成");
            
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            
            Game.Scene.AddComponent<NetThreadComponent>();
            Game.Scene.AddComponent<SessionStreamDispatcher>();
            Game.Scene.AddComponent<ZoneSceneManagerComponent>();
            
            Game.Scene.AddComponent<GlobalComponent>();
            Game.Scene.AddComponent<LanguageComponent>();
            
            Game.Scene.AddComponent<HallInfoComponent>();
            Game.Scene.AddComponent<GameInfoComponent>();

            Game.Scene.AddComponent<AIDispatcherComponent>();
            Game.Scene.AddComponent<TipInfoComponent>();
            Scene zoneScene = SceneFactory.CreateZoneScene(1, "Game", Game.Scene);
            GlobalComponent.Instance.scene = zoneScene;
            //UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMainUnderBg, UILayer.Mid).Coroutine();
            Game.Scene.AddComponent<AppInfoComponent>();
            Game.Scene.AddComponent<SoundComponent>();
//            bool isNewAccount = false;
            if (GameDataMgr.Instance.mbEdit)
            {
                UIHelper.Create(zoneScene, UIType.UIMBEdit, UILayer.Mid).Coroutine();
                return;
            }
            //AppInfoComponent.Instance.enterType = FirebaseSetup.Instance.RCJ.first_enterGame;
            string account = UnityEngine.PlayerPrefs.GetString(ItemSaveStr.account);
            if (account != null && account != "")
            {
                //LoginHelper.Login(zoneScene, ConstValue.LoginAddress, long.Parse(account), "").Coroutine();
                // LoginHelper.Login(
                //     zoneScene,ConstValue.LoginAddress,long.Parse(account), "").Coroutine();
            }
            else
            {
//                isNewAccount = true;
                long account0 = GetTimeStamp();
                account0 = account0 * 1000 + Random.Range(0, 999);
                PlayerPrefs.SetString(ItemSaveStr.account, account0.ToString());
                HallInfoComponent.Instance.NickName = "100" + account;
                //EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.New_User);
                // LoginHelper.Login(
                //     zoneScene,ConstValue.LoginAddress,account0, "").Coroutine();
                Log.Info("发起登录3333");
            }
            await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMB, UILayer.Mid);
            EventDispatcher.PostEvent(EventName.LoadingCloseListen, null);
            //this.DoGetIP(zoneScene,isNewAccount).Coroutine();
        }

        private async ETTask DoGetIP(Scene zoneScene,bool isNewAccount)
        {
            GetCountryByIp(async (string info) =>
            {
                try
                {
                    LoginHelper.LanguageByIp();
                }
                catch
                {
                    Log.Console("LoginHelper.LanguageByIp error");
                }
                GoogleLoginMgr.GetInstance().Init();
                int ip = (int) GameDataMgr.Instance.IpSelect;
                GameDataMgr.Instance.IpSelect = (IPMenu)PlayerPrefs.GetInt(ItemSaveStr.MyIp, ip);
                if(!string.IsNullOrEmpty(GameDataMgr.Instance.IPAdress))
                    ConstValue.LoginAddress = GameDataMgr.Instance.IPAdress;
                // int waitTime = 0;
                // while (FireBComponent.Instance.initFinish == 0 && waitTime < 30)
                // {
                //     Log.Console(TimeHelper.DateTimeNow().ToString("HH:mm:ss") + " 等待Fire Romote");
                //     await TimerComponent.Instance.WaitAsync(1);
                //     waitTime++;
                // }

                int language = PlayerPrefs.GetInt(ItemSaveStr.Language, -1);
                if (language != -1)
                {
                    GameDataMgr.Instance.languageSelect = (LanguageSelect) language;
                }

                //            ConfigComponent.Instance.LoadMBLvAsync().Coroutine();
                // if(waitTime >= 30)
                //     AppInfoComponent.Instance.enterType = FirebaseSetup.Instance.RCJ.first_enterGame;
                // else
                //     AppInfoComponent.Instance.enterType = (int)FirebaseSetup.Instance.RCJ.GetRemoteLong(FireBRemoteName.first_time_player_experien);
                if (!isNewAccount)
                {
                    //Log.Info("auto login,account:" + account);
                    if (AppInfoComponent.Instance.enterType != KeyDefine.enterMainCity)
                    {
                        MBDataComponent.Instance.curPlayLevel = MBDataComponent.Instance.level;
                    }
                }
                else
                {
                    AppInfoComponent.Instance.isNewDevice = 3;
                    if (AppInfoComponent.Instance.enterType != KeyDefine.enterMainCity)
                    {
                        AppInfoComponent.Instance.guideStep = 2;
                        MBDataComponent.Instance.curPlayLevel = MBDataComponent.Instance.level;
                        Log.Console("开始加载MB");
                        EventDispatcher.PostEvent(EventName.ChangeItemNum, null, KeyDefine.entityId, 0, -1);
                    }
                }

                if (AppInfoComponent.Instance.enterType != KeyDefine.enterMainCity)
                {
                    await TimerComponent.Instance.WaitAsync(1500);
                    
                    //UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMainCity, UILayer.Low2, null, false, 0).Coroutine();
                    // if(HallHelper.gateSession != null)
                    //     UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIChatUnder, UILayer.Mid, null, false, 0).Coroutine();
                }
                DynamicDownLoadModelMgr.GetInstance().EnterGameLoad().Coroutine();
                //await Game.EventSystem.PublishAsync(new EventType.AppStartInitFinish() { ZoneScene = zoneScene });
                });

            await ETTask.CompletedTask;
        }

        // 获取当前时间戳--10位时间戳, 注意,int32的时间戳, 只能到2038年, 所以采用了long(int64) 
        public long GetTimeStamp()
        {
            // 注意, 如果直接使用DateTime.Now, 会有系统时区问题, 导致误差
            System.TimeSpan timeStamp = System.DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            return timeStamp.TotalSeconds.ToInt64();
        }

      
        public void GetCountryByIp(Action<string> callBack)
        {
            // if (HallInfoComponent.Instance.country != "")
            // {
            //     GameDataMgr.Instance.country = HallInfoComponent.Instance.country;
            //     Debug.Log(" 国家是:" + GameDataMgr.Instance.country);
            //     callBack(GameDataMgr.Instance.country);
            //     return;
            // }
            // IpInfo ipInfo = new IpInfo();
            // try
            // {
            //     Log.Info("get country 1");
            //     await this.GetIp((string info) =>
            //     {
            //         Log.Info("get country 2:" + info);
            //         if (string.IsNullOrEmpty(info))
            //         {
            //             callBack(GameDataMgr.Instance.country);
            //         }
            //
            //         ipInfo = LitJson.JsonMapper.ToObject<IpInfo>(info);
            //         GameDataMgr.Instance.country = ipInfo.country;
            //         HallInfoComponent.Instance.country = ipInfo.country;
            //         Debug.Log(" 国家save:" + GameDataMgr.Instance.country);
            //         // GameDataMgr.Instance.region = ipInfo.region;
            //         // GameDataMgr.Instance.city = ipInfo.city;
            //         for (int i = 0; i < PhotonRegion.photonIp.Length; i++)
            //         {
            //             string[] region = PhotonRegion.photonIp[i];
            //             for (int j = 0; j < region.Length; j++)
            //             {
            //                 if (ipInfo.country == region[j])
            //                 {
            //                     GameDataMgr.Instance.photonIp = PhotonIp.photonAllIp[i];
            //                     break;
            //                 }
            //             }
            //         }
            //         if (GameDataMgr.Instance.country == "United States")
            //         {
            //             string[] region = PhotonRegion.photonIp[1];
            //             for (int i = 0; i < region.Length; i++)
            //             {
            //                 if (ipInfo.region == region[i])
            //                 {
            //                     GameDataMgr.Instance.photonIp = PhotonIp.photonAllIp[1];
            //                     break;
            //                 }
            //             }
            //         }
            //
            //         if (GameDataMgr.Instance.photonIp == "")
            //         {
            //             GameDataMgr.Instance.photonIp = PhotonIp.photonAllIp[9];
            //         }
            //
            //         callBack(GameDataMgr.Instance.country);
            //         Debug.Log(" 国家是:" + GameDataMgr.Instance.country + ". 州/省份:" + GameDataMgr.Instance.region);
            //         Debug.Log("GameDataMgr地区的值是" + GameDataMgr.Instance.photonIp);
            //     });
            //     Log.Info("get country 2");
            // }
            // catch (Exception)
            // {
            //     ipInfo.country = null;
            // }
        }
        private async ETTask GetIp(Action<String> callBack)
        {
            Log.Console("request Ip :http://ip-api.com/json");
            UnityWebRequest webRequest = UnityWebRequest.Get("http://ip-api.com/json");
            string info = "";
            webRequest.SendWebRequest();
            float timeOut = Time.time;
            //TimeHelper.ExecutionTime("获得IP：：",1);
            while (webRequest != null && !webRequest.isDone)
            {
                if ((Time.time - timeOut) > 5000)
                {
                    Log.Error("获取国家IP超过5秒" + "http://ip-api.com/json");
                    callBack(null);
                    webRequest?.Dispose();
                    webRequest = null;
                    return;
                }
            }
            //TimeHelper.ExecutionTime("获得IP：：",2);
            Log.Info("55555555555");
            if (webRequest.result !=  UnityWebRequest.Result.Success)
            {
                Log.Info("获取国家IP失败");
                callBack("");
            }
            else
            {
                info = webRequest.downloadHandler.text;
                callBack(info);
                Log.Info("获取国家IP成功:::" + info);
            }
            webRequest?.Dispose();
            webRequest = null;
            await ETTask.CompletedTask;
        }

    }
}
