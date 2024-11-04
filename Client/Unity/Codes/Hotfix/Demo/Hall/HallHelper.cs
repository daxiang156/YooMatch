using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;

namespace ET
{
    public static class HallHelper
    {
        public static Session gateSession = null;

        /// <summary>
        /// 
        /// </summary>
    /// <param name="zoneScene"></param>
        public static async void ConnectToGate(Scene zoneScene)
        {
            AccountInfoComponent accountData = zoneScene.GetComponent<AccountInfoComponent>();
            try
            {
                try
                {
                    if(gateSession != null && !gateSession.IsDisposed)
                        return;
                    if(!GameDataMgr.Instance.isConnecting)
                        GameDataMgr.Instance.curConnectType = ConnectType.GateConnecting;
                    HallInfoComponent.Instance._isFirstBag = true;
                    //connect to gate
                    C2G_LoginGameGate cmd = new C2G_LoginGameGate();
                    cmd.Key = accountData.Token;
                    cmd.AccountId = accountData.AccountId;
                    cmd.AccountSkinId = accountData.AccountSkinId;
                    cmd.Country = GameDataMgr.Instance.country;
                    cmd.Region = GameDataMgr.Instance.myRegion;
                    cmd.Name = GameDataMgr.Instance.nickName;
                    cmd.DeviceId = GameDataMgr.Instance.DeviceId();
                    cmd.AppsFlyerId = GameDataMgr.Instance.appsFlyerId;
                    gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(accountData.Address));
                    if(gateSession.GetComponent<PingComponent>() == null) 
                        gateSession.AddComponent<PingComponent>();
                    if(zoneScene.GetComponent<SessionComponent>() == null)
                        zoneScene.AddComponent<SessionComponent>().Session = gateSession;
                    else
                    {
                        zoneScene.GetComponent<SessionComponent>().Session = gateSession;
                    }
                    G2C_LoginGameGate message = (G2C_LoginGameGate) await gateSession.Call(cmd);
                    if (message.Error != ErrorCode.ERR_Success)
                    {
                        await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                        {
                            ZoneScene = zoneScene,
                            errorId = message.Error
                        });
                        return;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    return;
                }

                if(!GameDataMgr.Instance.isConnecting)
                    GameDataMgr.Instance.curConnectType = ConnectType.GateConnected;
                EnterGame(zoneScene);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

           
            return;
        }

        public static async void EnterGame(Scene zoneScene)
        {
            AccountInfoComponent accountData = zoneScene.GetComponent<AccountInfoComponent>();
            if(!GameDataMgr.Instance.isConnecting)
                GameDataMgr.Instance.curConnectType = ConnectType.EnterGameing;
            //try
            {
                C2G_EnterGame cmd = new C2G_EnterGame();
                cmd.ClientVersion = GameDataMgr.Instance.version;
                long houseId = 0;
                if (GameDataMgr.Instance.isConnecting)
                    cmd.IsReLogin = 1;
                if (GameDataMgr.Instance.isConnecting && GameDataMgr.Instance.curConnectType == ConnectType.InGame && long.TryParse(GameDataMgr.Instance.cityId, out houseId))
                    cmd.HouseId = houseId;
                G2C_EnterGame message = (G2C_EnterGame) await gateSession.Call(cmd);
                AppInfoComponent.Instance.registContry = message.Country;
                AppInfoComponent.Instance.registRetion = message.Region;
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }

                if (cmd.HouseId != 0)
                {
                    message.HouseId = cmd.HouseId;
                }

                GameDataMgr.isLoginFinish = true;
                AppInfoComponent.Instance.singleState = SingleState.interned;
                if (GameDataMgr.Instance.isConnecting)
                {
                    Log.Console("重连完成");
                    EventDispatcher.PostEvent(EventName.CloseWaitingUI, null);
                    GameDataMgr.Instance.isConnecting = false;
                    if (!CommonFuc.IsMainCity() && GameDataMgr.Instance.curConnectType != ConnectType.MBMap &&
                        GameDataMgr.Instance.curConnectType != ConnectType.MBGame )
                    {
                        await Game.EventSystem.PublishAsync(new EventType.CreateUIMainCity() { ZoneScene = accountData.ZoneScene()});

                        if (AppInfoComponent.Instance.enterType == KeyDefine.enterMainCity)
                        {
                            int sceneId0 = 3;
                            Log.Console("回主城6");
                            MapConfig config0 = MapConfigCategory.Instance.Get(sceneId0);
                            await SceneChangeHelper.SceneChangeTo(zoneScene, config0.source, config0.Id);
                        }
                    }
                    if(GameDataMgr.Instance.curConnectType < ConnectType.EnterGamed)
                        GameDataMgr.Instance.curConnectType = ConnectType.EnterGamed;
//                    EventDispatcher.PostEvent(EventName.ChgModuleIdSkinUI, null, GameDataMgr.Instance.moduleId);
                    EventDispatcher.PostEvent(ETEventName.LoginServerFinish,null);
                    return;
                }
                EventDispatcher.PostEvent(ETEventName.LoginServerFinish,null);
                if (MBDataComponent.Instance.failTime == 0)
                {
                    EventDispatcher.PostEvent(ETEventName.GetFunyGameFailData, null);
                }

                if(AppInfoComponent.Instance.enterType != KeyDefine.enterMainCity)
                    return;
                GameDataMgr.Instance.curConnectType = ConnectType.EnterGamed;
                

                var cameraObj = Camera.main.gameObject;
                var hallInfo = Game.Scene.GetComponent<HallInfoComponent>();
                if (hallInfo == null)
                {
                    hallInfo = Game.Scene.AddComponent<HallInfoComponent>();
                }
                hallInfo.cameraObj = cameraObj;
                
                int sceneId = 3;
                MapConfig config = MapConfigCategory.Instance.Get(sceneId);
                //bool isGoMainCity = false;
                if (CommonFuc.CurScene() == "Start")
                {
                    //isGoMainCity = true;
                    Log.Console("回主城7");
                    await SceneChangeHelper.SceneChangeTo(zoneScene, config.source, config.Id);
                }

                await Game.EventSystem.PublishAsync(new EventType.CreateUIMainCity() { ZoneScene = accountData.ZoneScene()});

                if ( zoneScene != null && sceneId == 3)
                {
                    Debug.Log("change scene create uichatunder");
                    await TimerComponent.Instance.WaitAsync(2000);
                    //await UIHelper.Create(zoneScene, UIType.UIChatUnder, UILayer.Mid);
                    await EventSystem.Instance.PublishAsync(new EventType.CreateChatUnder(){ZoneScene = accountData.ZoneScene()});
                    // if(isGoMainCity)
                    //     EventDispatcher.PostEvent(EventName.EnterOrExitMainCity, null, 1);
                }
                //await Game.EventSystem.PublishAsync(new EventType.LoadingFinish() { Scene = accountData.ZoneScene()});
                //GetHeroRoad(zoneScene, false);
                //await Game.EventSystem.PublishAsync(new EventType.EnterGame() { Scene = accountData.ZoneScene(), });
            }
            // catch (Exception e)
            // {
            //     if (e.Message.Contains("dispose"))
            //     {
            //         GameDataMgr.Instance.isConnecting = false;
            //         
            //         Log.Console("重连完成");
            //         EventDispatcher.PostEvent(EventName.ApplicationFocus, null, true, true);
            //     }
            //
            //     Log.Error(e.Message);
            //     return;
            // }
            
            FireBaseMgr.GetInstance().SendFireBaseTokenToServer();
            await TimerComponent.Instance.WaitAsync(5000);
            return;
        }
        
        

        public static async void JoinMatch(Scene zoneScene, int mapId, bool isMBMore = false, Action callback = null)
        {
            try
            {
                if (gateSession == null || gateSession.IsDisposed)
                {                            
                    EventDispatcher.PostEvent(EventName.ShowTips, null, "No internet connection, you are in offline mode"); //展示断线弹窗
                    EventDispatcher.PostEvent(EventName.ConnectMySerInGame, null);
                    int connectTime = 0;
                    while (gateSession == null || gateSession.IsDisposed)
                    {
                        connectTime++;
                        if (connectTime > 8)
                        {
                            break;
                        }
                        await TimerComponent.Instance.WaitAsync(500);
                    }
                }
                C2M_JoinMatch cmd = new C2M_JoinMatch();
                cmd.MpaId = mapId;
                EventDispatcher.PostEvent(EventName.NetWaitUI, null, true);
                M2C_JoinMatch message = (M2C_JoinMatch) await gateSession.Call(cmd);
                EventDispatcher.PostEvent(EventName.NetWaitUI, null, false);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }

                HallInfoComponent.Instance.curMap = mapId;
                if(mapId != MapDefine.MBMore)
                    GameDataMgr.Instance.curConnectType = ConnectType.Matching;
                await Game.EventSystem.PublishAsync(new EventType.JoinMatch() { ZoneScene = zoneScene, });
                if (callback != null)
                    callback();

                if(isMBMore)
                    return;
                //GameDataMgr.Instance.cityId = message.MatchId.ToString();
                
                EventDispatcher.PostEvent(EventName.PunLeaveRoom, null);
                EventDispatcher.PostEvent(EventName.HideMainCityTpsControl, null, false);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }

        public static async void JoinMatchCancel(Scene zoneScene)
        {
            
            try
            {
                C2M_CancelMatch cmd = new C2M_CancelMatch();
                cmd.MpaId = 6;
                M2C_CancelMatch message = (M2C_CancelMatch) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                var hallInfo = Game.Scene.GetComponent<HallInfoComponent>();
                //hallInfo.cameraObj.SetActive(false);
                await Game.EventSystem.PublishAsync(new EventType.JoinMatchCancel() { ZoneScene = zoneScene, });
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }

        public static async void JoinGame(Scene zoneScene)
        {
            try
            {
                C2M_EnterMap cmd = new C2M_EnterMap();
                cmd.MapId = 6;
                M2C_EnterMap message = (M2C_EnterMap) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                //await SceneChangeHelper.SceneChangeTo(zoneScene, "scene_bld", 6);
                //await SceneChangeHelper.SceneChangeTo(zoneScene, "scene_bld10_000005", 15);
                EventDispatcher.PostEvent(ETEventName.StopBgMusic,null);
                //var timeComp = Game.Scene.GetComponent<TimerComponent>();
                //await timeComp.WaitAsync(200);
                //await Game.EventSystem.PublishAsync(new EventType.EnterUIGame() { ZoneScene = zoneScene, });
                //await SceneChangeHelper.SceneChangeTo(zoneScene, "scene_bld", 6);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }

        public static async void GetHeroRoad(Scene zoneScene, bool isShowUI = true, Action callback = null)
        {
            try
            {
                if (gateSession == null || gateSession.IsDisposed)
                {
                    EventDispatcher.PostEvent(EventName.ConnectMySerInGame, null);
                    EventDispatcher.PostEvent(EventName.ShowTips, null, "No internet connection, you are in offline mode"); //展示断线弹窗
                    int connectTime = 0;
                    while (gateSession == null || gateSession.IsDisposed)
                    {
                        connectTime++;
                        if (connectTime > 8)
                        {
                            break;
                        }

                        await TimerComponent.Instance.WaitAsync(500);
                    }
                }
                C2M_GetAchievementInfo cmd = new C2M_GetAchievementInfo();
                M2C_GetAchievementInfo message = (M2C_GetAchievementInfo) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                HeroRoadInfoComponent info = zoneScene.GetComponent<HeroRoadInfoComponent>();
                if (info == null)
                {
                    info = zoneScene.AddComponent<HeroRoadInfoComponent>();
                }

                if (callback != null)
                    callback();
                HallInfoComponent hallInfo = Game.Scene.GetComponent<HallInfoComponent>();
                hallInfo.heroRoadInfo = message;

                for (int i = 0; i < message.AchList.Count; i++)
                {
                    HeroItemInfo item = new HeroItemInfo();
                    item.AchId = message.AchList[i].AchId;
                    item.state = message.AchList[i].state;
                    info.AchList.Add(item);
                }

                info.curProgress = Convert.ToInt32(message.curProgress);

                if (isShowUI)
                {
                    await Game.EventSystem.PublishAsync(new EventType.EnterHeroRoad() { ZoneScene = zoneScene, });
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }

        //切换皮肤
        public static async void ChangeCurSkin(Scene zoneScene, int skinId, Action<int> callback)
        {
            M2C_SkinInfo_List skinInfo = HallInfoComponent.Instance.skinInfoList;
            if (skinInfo == null)
            {
                callback(0);
                return;
            }

            for (int i = 0; i < skinInfo.SkinList.Count; i++)
            {
                if (skinInfo.SkinList[i].SkinId == skinId)
                {
                    callback(skinId);
                    HallInfoComponent.Instance.SkinId = skinInfo.SkinList[i].SkinId;
                    EventDispatcher.PostEvent(ETEventName.ChangeSkin,null,skinId);
                    return;
                }
            }


            //try
            {
                C2M_ChangeCurSkin cmd = new C2M_ChangeCurSkin();
                cmd.SkinId = skinId;
                M2C_ChangeCurSkin message = (M2C_ChangeCurSkin) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                callback(message.SkinId);
                HallInfoComponent.Instance.SkinId = skinId;
                EventDispatcher.PostEvent(ETEventName.ChangeSkin,null,skinId);
            }
            // catch (Exception e)
            // {
            //     Log.Error(e.Message);
            //     return;
            // }
            return;
        }
        
        //切换衣服
        public static async void ChangeAppearance(Scene zoneScene, int skinId, int appearanceId, Action<int, int> callback)
        {
            try
            {
                C2M_ChangeSkinAppearance cmd = new C2M_ChangeSkinAppearance();
                cmd.SkinId = skinId;
                cmd.AppearanceId = appearanceId;
                M2C_ChangeSkinAppearance message = (M2C_ChangeSkinAppearance) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                callback(message.SkinId, message.AppearanceId);
                //await Game.EventSystem.PublishAsync(new EventType.ChangeSkinId() { ZoneScene = zoneScene,skinId = message.SkinId});
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }
        
        //切换穿戴
        public static async void ChangeWear(Scene zoneScene, int skinId, long ItemUId, int wearId, Action<int, int> callback)
        {
            //try
            {
                C2M_ChangeSkinWear cmd = new C2M_ChangeSkinWear();
                cmd.SkinId = skinId;
                cmd.WearId = wearId;
                cmd.ItemUId = ItemUId;
                M2C_ChangeSkinWear message = (M2C_ChangeSkinWear) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                callback(message.SkinId, message.WearId);
                //await Game.EventSystem.PublishAsync(new EventType.ChangeSkinId() { ZoneScene = zoneScene,skinId = message.SkinId});
            }
            // catch (Exception e)
            // {
            //     Log.Error(e.Message);
            //     return;
            // }
            return;
        }
        
        //结算
        public static async void GameResult(Scene zoneScene, int rank, int itemNum = 0, int mapId = 4, Action callback = null, bool isConenctData = false, 
            int level = 1, int curpross = 0, bool isOffline = false, List<RankInfo> RankInfo = null, List<FunyLevelEvent> levelList = null)
        {
            try
            {
                // if (gateSession == null || gateSession.IsDisposed)
                // {
                //     EventDispatcher.PostEvent(EventName.ConnectMySerInGame, null);
                //     int connectTime = 0;
                //     while (gateSession == null || gateSession.IsDisposed)
                //     {
                //         connectTime++;
                //         if (connectTime > 8)
                //         {
                //             EventDispatcher.PostEvent(EventName.ShowTips, null, "No internet connection, you are in offline mode"); //展示断线弹窗
                //             break;
                //         }
                //
                //         await TimerComponent.Instance.WaitAsync(500);
                //     }
                // }
                Log.Console("请求结算");
                GameDataMgr.Instance.bot_number = -1;
                C2M_GameResult cmd = new C2M_GameResult();
                cmd.MapId = mapId;
                cmd.offline = isOffline;
                if (RankInfo == null)
                {
                    var rankInfo = new RankInfo();
                    rankInfo.roleId = GameDataMgr.Instance.unitId;
                    rankInfo.rank = (byte) rank;
                    cmd.RankInfo.Add(rankInfo);
                }
                else
                    cmd.RankInfo = RankInfo;

                if (rank == 1)
                    cmd.resultState = 1;
                else
                    cmd.resultState = 0;
                cmd.funyGameId = level;
                if (rank == 1)
                    cmd.resultState = 1;
                if (curpross != 1)
                    cmd.funyGameProgress = curpross;
                if (cmd.resultState == 1 )
                {
                    cmd.funyGameProgress = 100;
                }

                cmd.levelEvent = levelList;

                EventDispatcher.PostEvent(EventName.NetWaitUI, null, true, rank);
                M2C_GameResult message = (M2C_GameResult) await gateSession.Call(cmd);
                EventDispatcher.PostEvent(EventName.NetWaitUI, null, false);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }

                if (isConenctData)
                {
                    return;
                }

                var gameInfo = Game.Scene.GetComponent<GameInfoComponent>();
                if (gameInfo == null)
                {
                    gameInfo = Game.Scene.AddComponent<GameInfoComponent>();
                }


                gameInfo.result = message;
                Log.Console("准备打开WinBe");
                if (callback != null)
                    callback();
                else
                    await Game.EventSystem.PublishAsync(new EventType.GameFinish() { ZoneScene = zoneScene});
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }
        
        public static async void GM(Scene zoneScene, string itenStr)
        {
            try
            {
                C2M_GM_AddItem cmd = new C2M_GM_AddItem();
                cmd.itemStr = itenStr;
                M2C_GM_AddItem message = (M2C_GM_AddItem) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    gateSession.Dispose();
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                //await Game.EventSystem.PublishAsync(new EventType.ChangeSkinId() { ZoneScene = zoneScene,skinId = message.SkinId});
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }
        
        public static async void PickGold(Scene zoneScene, int gold, int goldIndex)
        {
            try
            {
                C2M_PickUpGold cmd = new C2M_PickUpGold();
                cmd.GoldNum = gold;
                cmd.GoldIndex = (byte)goldIndex;
                M2C_PickUpGold message = (M2C_PickUpGold) await gateSession.Call(cmd);
                //Log.Error("收到金币返回");
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                await Game.EventSystem.PublishAsync(new EventType.UpdateGold() { ZoneScene = zoneScene,gold = (int)message.GoldNum});
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }
        
        public static async void GetGoodList(Scene zoneScene, Action callback = null)
        {
            // try
            // {
            //     if (gateSession == null || gateSession.IsDisposed)
            //     {                            
            //         EventDispatcher.PostEvent(EventName.ShowTips, null, "No internet connection, you are in offline mode"); //展示断线弹窗
            //         EventDispatcher.PostEvent(EventName.ConnectMySerInGame, null);
            //         int connectTime = 0;
            //         while (gateSession == null || gateSession.IsDisposed)
            //         {
            //             connectTime++;
            //             if (connectTime > 8)
            //             {
            //                 break;
            //             }
            //             await TimerComponent.Instance.WaitAsync(500);
            //         }
            //     }
            //     C2M_GetActivityDetail cmd = new C2M_GetActivityDetail();
            //     cmd.ActId = 1;
            //     M2C_GetActivityDetail message = (M2C_GetActivityDetail) await gateSession.Call(cmd);
            //     if (message.Error != ErrorCode.ERR_Success)
            //     {
            //         await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
            //         {
            //             ZoneScene = zoneScene,
            //             errorId = message.Error
            //         });
            //         return;
            //     }
            //
            //     if (callback != null)
            //         callback();
            //     HallInfoComponent info = Game.Scene.GetComponent<HallInfoComponent>();
            //     info.goodActivityDetail = message;
            //     await Game.EventSystem.PublishAsync(new EventType.GetGoodList() { ZoneScene = zoneScene});
            // }
            // catch (Exception e)
            // {
            //     Log.Error(e.Message);
            //     return;
            // }
            // return;
            
            List<ActivityGoods> GoodsList = new List<ActivityGoods>();
            ActivityConfig activityConfigs = ActivityConfigCategory.Instance.Get(1);
            string[] goodIds = activityConfigs.goodsId.Split(':');
            for(int i = 0; i <goodIds.Length; i++)
            {
                int goodId = int.Parse(goodIds[i]);
                ActivityGoodsConfig activityGoodsConfig = ActivityGoodsConfigCategory.Instance.Get(goodId);
                ActivityGoods activityGoods = new ActivityGoods();
                activityGoods.GoodsId = goodId;
                activityGoods.Type = (byte)activityGoodsConfig.type;
                //activityGoods.Num = activityGoodsConfig.buyTimes;
                //activityGoods.Sale = activityGoodsConfig.
                activityGoods.BuyTimes = activityGoodsConfig.buyTimes;
                string[] buyItemInfo = activityGoodsConfig.goods.Split(':');
                string[] costItemInfo = activityGoodsConfig.cost.Split(':');
                activityGoods.ItemId = int.Parse(buyItemInfo[0]);
                activityGoods.itemNum = int.Parse(buyItemInfo[1]);
                //activityGoods.Sale = byte.Parse(buyItemInfo[2]);
                activityGoods.CostId = new List<int>();
                activityGoods.CostId.Add(int.Parse(costItemInfo[0]));
                activityGoods.CostNum = new List<int>();
                activityGoods.CostNum.Add(int.Parse(costItemInfo[1]));
                activityGoods.Icon = activityGoodsConfig.icon;
                GoodsList.Add(activityGoods);
            }
            if (callback != null)
                callback();
            HallInfoComponent info = Game.Scene.GetComponent<HallInfoComponent>();
            info.goodActivityDetail = GoodsList;
            await Game.EventSystem.PublishAsync(new EventType.GetGoodList() { ZoneScene = zoneScene});
           
            return;
        }

        public static async void BuyGoods(Scene zoneScene, int goodId, Action callback, bool isShowReward = true)
        {
            //try
            {
                C2M_BuyGoods cmd = new C2M_BuyGoods();
                cmd.ActId = 1;
                cmd.GoodsId = goodId;
                AppInfoComponent.Instance.isWaitingSer = true;
                M2C_BuyGoods message = (M2C_BuyGoods) await gateSession.Call(cmd);
                AppInfoComponent.Instance.isWaitingSer = false;
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }

                if (callback != null)
                    callback();

                if (isShowReward)
                {
                    HallInfoComponent hallInfoComponent = Game.Scene.GetComponent<HallInfoComponent>();
                    hallInfoComponent.goodId = goodId;

                    string[] goods = ActivityGoodsConfigCategory.Instance.Get(goodId).goods.Split(':');
                    Log.Console("Goods:" + goods);
                    int itemId = int.Parse(goods[0]);
                    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemId);
                    if (itemConfig.type == ItemBigType.ItemType_Clothes)
                    {
                        await Game.EventSystem.PublishAsync(new EventType.GetSkin() { ZoneScene = zoneScene, });
                    }
                    else
                    {
                        await Game.EventSystem.PublishAsync(new EventType.GetHeroRoadReward() { ZoneScene = zoneScene, });
                    }
                }
            }
            // catch (Exception e)
            // {
            //     Log.Error(e.Message);
            //     return;
            // }
            return;
        }
        
        public static async void GetHeroRoadReward(Scene zoneScene, int AchId, Action callBack)
        {
            try
            {
                if (gateSession == null || gateSession.IsDisposed)
                {                            
                    EventDispatcher.PostEvent(EventName.ShowTips, null, "No internet connection, you are in offline mode"); //展示断线弹窗
                    EventDispatcher.PostEvent(EventName.ConnectMySerInGame, null);
                    int connectTime = 0;
                    while (gateSession == null || gateSession.IsDisposed)
                    {
                        connectTime++;
                        if (connectTime > 8)
                        {
                            break;
                        }
                        await TimerComponent.Instance.WaitAsync(500);
                    }
                }
                C2M_GetAchievementReward cmd = new C2M_GetAchievementReward();
                cmd.AchId = AchId;
                M2C_GetAchievementReward message = (M2C_GetAchievementReward) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                
                if (callBack != null)
                    callBack();
                //HallInfoComponent hallInfoComponent = Game.Scene.GetComponent<HallInfoComponent>();
                //hallInfoComponent.goodId = goodId;
                
                HallInfoComponent hallInfo = Game.Scene.GetComponent<HallInfoComponent>();
                hallInfo.getHeroRodReward = message;
                if (message.rewardList[0].itemType == ItemType.skin)
                {
                    await Game.EventSystem.PublishAsync(new EventType.GetSkin() { ZoneScene = zoneScene,});
                }
                else
                {
                    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(message.rewardList[0].itemId);
                    if (itemConfig.type == ItemBigType.ItemType_Clothes)
                    {
                        await Game.EventSystem.PublishAsync(new EventType.GetSkin() { ZoneScene = zoneScene,});
                    }
                    else
                    {
                        await Game.EventSystem.PublishAsync(new EventType.GetHeroRoadReward() { ZoneScene = zoneScene, });
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }

            return;
        }
        
        public static async void ExitGame(Scene zoneScene, Action<string> callback)
        {
            try
            {
                C2M_ExitPlayHouse cmd = new C2M_ExitPlayHouse();
                M2C_ExitPlayHouse message = (M2C_ExitPlayHouse) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }

                GameDataMgr.Instance.curConnectType = ConnectType.EnterGamed;
                if (callback != null)
                {
                    GameDataMgr.Instance.curConnectType = ConnectType.InGame;
                    callback(message.CityId.ToString());
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }
        
        public static async void GetBagList(Scene zoneScene)
        {
            try
            {
                C2M_GetBag_List cmd = new C2M_GetBag_List();
                M2C_GetBag_List message = (M2C_GetBag_List) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }

                HallInfoComponent hallInfoComponent = Game.Scene.GetComponent<HallInfoComponent>();
                hallInfoComponent.bagInfoList = message;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }
        
        public static async void GetSkinList(Scene zoneScene)
        {
            try
            {
                C2M_SkinInfo_List cmd = new C2M_SkinInfo_List();
                M2C_SkinInfo_List message = (M2C_SkinInfo_List) await gateSession.Call(cmd);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }
                HallInfoComponent hallInfoComponent = Game.Scene.GetComponent<HallInfoComponent>();
                hallInfoComponent.skinInfoList = message;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }
        
        public static async void CreatHouseRobotSucceed(Scene zoneScene)
        {
            try
            {
                C2M_CreatHouseRobotSucceed cmd = new C2M_CreatHouseRobotSucceed();
                var msg = await gateSession.Call(cmd);
                Log.Error("机器人成功");
                // if (message.Error != ErrorCode.ERR_Success)
                // {
                //     await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                //     {
                //         ZoneScene = zoneScene,
                //         errorId = message.Error
                //     });
                //     return;
                // }
                // HallInfoComponent hallInfoComponent = Game.Scene.GetComponent<HallInfoComponent>();
                // hallInfoComponent.skinInfoList = message;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }
        
        public static async void AddGameScore(Scene zoneScene, int teamId)
        {
            try
            {
                C2M_Sync_HouseEvent cmd = new C2M_Sync_HouseEvent();
                cmd.event_info.id = teamId;
                cmd.event_info.event_value = 1;
                M2C_Sync_HouseEvent message = (M2C_Sync_HouseEvent)await gateSession.Call(cmd);
                // if (message.Error != ErrorCode.ERR_Success)
                // {
                //     await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                //     {
                //         ZoneScene = zoneScene,
                //         errorId = message.Error
                //     });
                //     return;
                // }
                // HallInfoComponent hallInfoComponent = Game.Scene.GetComponent<HallInfoComponent>();
                // hallInfoComponent.skinInfoList = message;
                EventDispatcher.PostEvent(EventName.AddGameScore, null, teamId);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return;
            }
            return;
        }

        public static Dictionary<int, List<RankInfoProto>> RankDataDic = new Dictionary<int, List<RankInfoProto>>();
        public static async ETTask GetRankList(Scene zoneScene, string region = "", Action<int> callback = null, 
            string controy = "", int StartRank = 1, int RankRange = 30, bool isShowWait = true,object openParam = null)
        {
            //try
            {
               
                C2Rank_GetRansInfo cmd = new C2Rank_GetRansInfo();
                cmd.Region = region;
                cmd.Country = controy;
                cmd.StartRank = StartRank;
                cmd.RankRange = RankRange;
                if(isShowWait)
                    EventDispatcher.PostEvent(EventName.NetWaitUI, null, true);
                
                Rank2C_GetRansInfo message = (Rank2C_GetRansInfo) await gateSession.Call(cmd);
                EventDispatcher.PostEvent(ETEventName.GetRankData,null,message);
                if(isShowWait)
                    EventDispatcher.PostEvent(EventName.NetWaitUI, null, false);
                if (message.Error != ErrorCode.ERR_Success)
                {
                    await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                    {
                        ZoneScene = zoneScene,
                        errorId = message.Error
                    });
                    return;
                }

                string rankStr = PlayerPrefs.GetString(ItemSaveStr.rankProcess, "0:0");
                if (rankStr != "0:0")
                {
                    string[] rankProcess = rankStr.Split(':');;
                    int lv = int.Parse(rankProcess[0]);
                    int pro = int.Parse(rankProcess[1]);
                    if ((lv > message.Count) || (lv == message.Count && pro > message.ParamInt))
                    {
                        C2M_ModifyFunyGameData msg = new C2M_ModifyFunyGameData();
                        msg.funyGameId = lv;
                        msg.funyGameProgress = pro;
                        Log.Error("排行榜数据对不上， 本地进度：" + rankStr);
                        await gateSession.Call(msg);
                        return;
                    }
                }
                HallInfoComponent hallInfoComponent = Game.Scene.GetComponent<HallInfoComponent>();
                hallInfoComponent.rankList = message;
                if (message.RankInfoProtoList == null || message.RankInfoProtoList.Count == 0)
                {
                    return;
                }
                
                string key = controy == "" ? KeyDefine.RankWorld : controy;
                if(!HallInfoComponent.Instance.allRankDic.ContainsKey(key))
                    HallInfoComponent.Instance.allRankDic.Add(key, message);

                RankDataDic[message.StartRank] = message.RankInfoProtoList;
                if (callback != null)
                    callback(message.StartRank);
                
            }
            return;
        }
        
        public static async void UseItem(Scene zoneScene, int itemId, Action callback = null, int itemNum = 1, int level = 0, bool isOffline = false,  int isFree = 0)
        {
            if(gateSession == null)
                return;
            C2M_UseItem cmd = new C2M_UseItem();
            cmd.itemId = itemId;
            if(itemNum != 1)
                cmd.param = itemNum.ToString();
            cmd.offline = isOffline;
            cmd.param2 = level.ToString();
            cmd.isFree = isFree;
            //await Game.EventSystem.PublishAsync(new EventType.EnterMBRankUI() { ZoneScene = zoneScene, });
            M2C_UseItem message = (M2C_UseItem) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }
            if (callback != null)
                callback();
        }
        
        public static int GetStrNum(string str, int lv)
        {
            int difficulty = -1;
            if (!string.IsNullOrEmpty(str))
            {
                string[] levels = str.Split(',');
                for (int k = 0; k < levels.Length; k++)
                {
                    string[] levelOne = levels[k].Split(':');
                    int level = int.Parse(levelOne[0]);
                    if (level == lv)
                    {
                        difficulty = int.Parse(levelOne[1]);
                        break;
                    }
                }
            }
            return difficulty;
        }

        public static async void EnterFunGame(Scene zoneScene, int funyGameId, string freeItem, string levelDiffult, Action callback = null, bool isOffline = false)
        {
            C2M_EnterFunGame cmd = new C2M_EnterFunGame();
            cmd.funyGameId = funyGameId;
            cmd.offline = isOffline;
            cmd.free_item_level = GetStrNum(freeItem, funyGameId).ToString();
            cmd.free_item_level = cmd.free_item_level == "-1"? "2" : cmd.free_item_level;
            cmd.level_difficulty = GetStrNum(levelDiffult, funyGameId).ToString();
            cmd.level_difficulty = cmd.level_difficulty == "-1"? "1000" : cmd.level_difficulty;
            if(!isOffline)
                EventDispatcher.PostEvent(EventName.EnterFunGame, null, true, funyGameId);
            //await Game.EventSystem.PublishAsync(new EventType.EnterMBRankUI() { ZoneScene = zoneScene, });
            M2C_EnterFunGame message = (M2C_EnterFunGame) await gateSession.Call(cmd);
            if(!isOffline)
                EventDispatcher.PostEvent(EventName.EnterFunGame, null, false, funyGameId);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }
            if (callback != null)
                callback();
        }
        
        public static async void StartBindEmail(Scene zoneScene, string email, Action callback = null)
        {
            C2M_StartBindEmail cmd = new C2M_StartBindEmail();
            cmd.EmailId = email;
            
            //await Game.EventSystem.PublishAsync(new EventType.EnterMBRankUI() { ZoneScene = zoneScene, });
            M2C_StartBindEmail message = (M2C_StartBindEmail) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }
            if (callback != null)
                callback();
        }
        
        public static async void StartBindEmailCode(Scene zoneScene, int Code, Action callback = null)
        {
            C2M_StartBindEmailCode cmd = new C2M_StartBindEmailCode();
            cmd.Code = Code;
            
            //await Game.EventSystem.PublishAsync(new EventType.EnterMBRankUI() { ZoneScene = zoneScene, });
            M2C_StartBindEmailCode message = (M2C_StartBindEmailCode) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }
            if (callback != null)
                callback();
        }
        
        public static async void EmailInfo(Scene zoneScene)
        {
            C2M_EmailInfo cmd = new C2M_EmailInfo();
            
            //await Game.EventSystem.PublishAsync(new EventType.EnterMBRankUI() { ZoneScene = zoneScene, });
            M2C_EmailInfo message = (M2C_EmailInfo) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }

            HallInfoComponent.Instance.emailAddress = message.EmailId;
        }
        
        public static async void TaskGetAward(Scene zoneScene, int taskId, Action callback)
        {
            C2M_GetTaskReward cmd = new C2M_GetTaskReward();
            cmd.TaskId = taskId;
            //await Game.EventSystem.PublishAsync(new EventType.EnterMBRankUI() { ZoneScene = zoneScene, });
            M2C_GetTaskReward message = (M2C_GetTaskReward) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }

            if (callback != null)
                callback();
        }
        
        public static async void ChargeToSer(Scene zoneScene, string payId, string key,string price ,string isoCurrencyCode, Action callback, bool isOffline = false)
        {
            C2M_Get_RechargeReward cmd = new C2M_Get_RechargeReward();
            cmd.Key = key;
            cmd.productId = payId;
#if UNITY_EDITOR
            cmd.IsTest = false;
#else
            cmd.IsTest = true;            
#endif
            cmd.offline = isOffline;
            cmd.price = price;
            cmd.isoCurrencyCode = isoCurrencyCode;

            if(GameDataMgr.Instance.Platflam() == PlatForm.Android)
                cmd.platform = "android";
            else if (GameDataMgr.Instance.Platflam() == PlatForm.IOS)
                cmd.platform = "ios";
            else
                cmd.platform = "editor";
            //await Game.EventSystem.PublishAsync(new EventType.EnterMBRankUI() { ZoneScene = zoneScene, });
            //EventDispatcher.PostEvent(EventName.NetWaitUI, null, true);
            M2C_Get_RechargeReward message = (M2C_Get_RechargeReward) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }

            EventDispatcher.PostEvent(EventName.NetWaitUI, null, false);
            //EventDispatcher.PostEvent(EventName.PayServerCallback, null, message.productId);
            if (callback != null)
                callback();
        }
        
        public static async void FindAccount(Scene zoneScene, string mail, long code, Action callback = null)
        {
            C2M_FindAccountByEmail cmd = new C2M_FindAccountByEmail();
            cmd.EmailId = mail;
            cmd.Code = code;
            M2C_FindAccountByEmail message = (M2C_FindAccountByEmail)await HallHelper.gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }

            HallInfoComponent.Instance.account = message.AccountName;
            if (callback != null)
                callback();
        }
        
        public static async void SyncMBClick(Scene zoneScene, long unitId, List<int> itemList, List<int> itemMaskList = null, Action callback = null, int eventType = 1)
        {
            C2M_SynFunGameState cmd = new C2M_SynFunGameState();
            cmd.PicId = unitId;
            cmd.itemList = itemList;
            cmd.eventType = eventType;
            cmd.itemMaskList = itemMaskList;
            await gateSession.Call(cmd);
            // M2C_SynFunGameState message = (M2C_SynFunGameState)await HallHelper.gateSession.Call(cmd);
            // if (message.Error != ErrorCode.ERR_Success)
            // {
            //     await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
            //     {
            //         ZoneScene = zoneScene,
            //         errorId = message.Error
            //     });
            //     return;
            // }
        
            if (callback != null)
                callback();
        }
        
        public static async void DeleteAccount(Scene zoneScene, int delete = 1, Action callback = null)//操作类型 1，删除  2，取消删除
        {
            C2M_DeleteRole cmd = new C2M_DeleteRole();
            cmd.OpType = delete;
            M2C_DeleteRole message = (M2C_DeleteRole) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }
            if (callback != null)
                callback();
        }
        
        public static async void DeleteAccountState(Scene zoneScene)//操作类型 1，删除  2，取消删除
        {
            C2M_GetRoleStatus cmd = new C2M_GetRoleStatus();
            M2C_GetRoleStatus message = (M2C_GetRoleStatus) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }

            HallInfoComponent.Instance.isDeletingAccount = message.StatusTime < 86400 * 2 * 1000;
        }
        
        public static async void SyncItem(Scene zoneScene, List<ItemInfo> itemList)
        {
            C2M_SynItem cmd = new C2M_SynItem();
            cmd.ItemList = itemList;
            gateSession.Call(cmd).Coroutine();
            await ETTask.CompletedTask;
        }
        
        public static async void SyncGuide(Scene zoneScene, int guideStep)
        {
            C2M_GuideStep cmd = new C2M_GuideStep();
            cmd.Step = guideStep;
            M2C_GuideStep message = (M2C_GuideStep) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }
            await ETTask.CompletedTask;
        }
        
        public static async void SyncItem(Scene zoneScene, int itemId, int itemNum, int resourceId = 0, int resourceNum = 0)
        {
            C2M_SynItem cmd = new C2M_SynItem();
            if (itemId != 0)
            {
                cmd.ItemList = new List<ItemInfo>();
                ItemInfo itemInfo = new ItemInfo();
                itemInfo.ItemId = itemId;
                itemInfo.ItemNum = itemNum;
                cmd.ItemList.Add(itemInfo);
            }
            
            if (resourceId != 0)
            {
                cmd.ResourceList = new List<ItemInfo>();
                ItemInfo itemInfo = new ItemInfo();
                itemInfo.ItemId = resourceId;
                itemInfo.ItemNum = resourceNum;
                cmd.ResourceList.Add(itemInfo);
            }
            gateSession.Call(cmd).Coroutine();
            await ETTask.CompletedTask;
        }
        
        public static async void EnterCity(Scene zoneScene, int OpType)
        {
            C2M_ChangeSceneByClient cmd = new C2M_ChangeSceneByClient();
            cmd.OpType = OpType;
            gateSession.Call(cmd).Coroutine();
            await ETTask.CompletedTask;
        }
        
        public static async void FunyGameRemove(Scene zoneScene, List<int> RType, List<int> num)
        {
            C2M_FunyGameRemove cmd = new C2M_FunyGameRemove();
            cmd.RType = RType;
            cmd.RCount = num;
            gateSession.Call(cmd).Coroutine();
            await ETTask.CompletedTask;
        }
        
        public static async void GetADAward(Scene zoneScene, List<int> itemIds, List<int> itemNums)
        {
            C2M_SendGetADAward cmd = new C2M_SendGetADAward();
            cmd.itemIds = itemIds;
            cmd.itemNums = itemNums;
            gateSession.Call(cmd).Coroutine();
            await ETTask.CompletedTask;
        }
        
        public static async void ADLog(Scene zoneScene, int OpType, int path, int level, long ClientTime, bool isOffline = false, int isAlreadyLoad = -1)
        {
            C2M_SendADLogByClient cmd = new C2M_SendADLogByClient();
            cmd.OpType = OpType;
            cmd.param1 = path;// 0 结算时 1 消消乐主页
            cmd.param2 = level;
            cmd.ClientTime = ClientTime;
            cmd.offline = isOffline;
            cmd.param5 = isAlreadyLoad;
            gateSession.Call(cmd).Coroutine();
            await ETTask.CompletedTask;
        }
        
        public static async void SendFireBaseTokenToServer()
        {
            C2M_SendFireBaseToken cmd = new C2M_SendFireBaseToken();
            cmd.fireBaseToken = GameDataMgr.Instance.FireBaseToken;
            HallHelper.gateSession.Call(cmd).Coroutine();
            await ETTask.CompletedTask;
        }
        
        public static async void ChangeName(Scene zoneScene, string chgName, Action callback = null)
        {
            C2M_ReName cmd = new C2M_ReName();
            cmd.NewName = chgName;
            M2C_ReName message = (M2C_ReName) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }
            if (callback != null)
                callback();
            EventDispatcher.PostEvent(ETEventName.ChangeName,null);
            await ETTask.CompletedTask;
        }
        
        public static async void SendBILogByClient(int opType = 17, int param1 = 0, int param2 = 0, int param3 = 0, int param4 = 0, int param5 = 0, long clientTime = 0)
        {
            C2M_SendBILogByClient cmd = new C2M_SendBILogByClient();
            cmd.OpType = opType;
            cmd.param1 = param1;
            cmd.param2 = param2;
            cmd.param3 = param3;
            cmd.param4 = param4;
            cmd.param5 = param5;
            cmd.ClientTime = clientTime;
            HallHelper.gateSession.Call(cmd).Coroutine();
            await ETTask.CompletedTask;
        }
        
        public static async void CreateRobotSucceed()
        {
            C2M_CreatHouseRobotSucceed cmd = new C2M_CreatHouseRobotSucceed();
            HallHelper.gateSession.Call(cmd).Coroutine();
            await ETTask.CompletedTask;
        }
        
        public static async void PvpResult(Scene zoneScene, int funyGameProgress, List<RankInfo> rankInfos, Action callback = null)
        {
            C2M_FunyGamePvpResult cmd = new C2M_FunyGamePvpResult();
            cmd.funyGameProgress = funyGameProgress;
            cmd.MapId = MapDefine.MBMore;
            cmd.RankInfo = rankInfos;
            M2C_FunyGamePvpResult message = (M2C_FunyGamePvpResult) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }
            if (callback != null)
                callback();
            await ETTask.CompletedTask;
        }
        
        public static async void GetFunyGameEvent(Scene zoneScene, int funyGameId, Action callback = null)
        {
            C2M_GetFunyGameEvent cmd = new C2M_GetFunyGameEvent();
            cmd.funyGameId = funyGameId;
            M2C_GetFunyGameEvent message = (M2C_GetFunyGameEvent) await gateSession.Call(cmd);
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }

            MBDataComponent.Instance.funyLevelList = message.levelEvent;
            if (callback != null)
            {
                callback();
            }
            await ETTask.CompletedTask;
        }
    }
}