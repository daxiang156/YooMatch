using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class HallInfoAwakeSystem: AwakeSystem<HallInfoComponent>
    {
        public override void Awake(HallInfoComponent self)
        {
            HallInfoComponent.Instance = self;
            EventDispatcher.AddObserver(this, EventName.UseItemNum, (object[] paras) =>
            {
                int itemId = (int) paras[0];
                int itemNum = (int) paras[1];
                HallHelper.UseItem(self.ZoneScene(), itemId, null, itemNum, 0, true);
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.InitHallInfo, (object[] paras) =>
            {
                M2C_CreateMyUnit unit = (M2C_CreateMyUnit) paras[0];
                Log.Console("EventName.InitHallInfo");
                self.InitHallInfo(unit);
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.ChangeItemNum, (object[] paras) =>
            {
                int itemId = (int) paras[0];
                int itemNum = (int) paras[1];
                int itemChgNum = (int) paras[2];
                self.ChgItemNum(itemId, itemNum, itemChgNum);
                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, EventName.UpdateReRead, (object[] paras) =>
            {
                string rewardStr = paras[0] as string;
                Log.Console("rewardStr:" + rewardStr);
                string[] itemStrs = rewardStr.Split(':');
                //if (int.Parse(itemStrs[0]) == KeyDefine.ItemId)
                {
                    int itemId = int.Parse(itemStrs[1]);
                    int itemNum = int.Parse(itemStrs[2]);
                    EventDispatcher.PostEvent(EventName.ChangeItemNum, this, itemId, 0, itemNum);
                }
                    
                HallInfoComponent.Instance.rewardList = rewardStr;
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIReward, UILayer.Mid).Coroutine();
                return false;
            }, null);
        }
    }
    
    public static class HallInfoSystem
    {
        public static void InitHallInfo(this HallInfoComponent self, M2C_CreateMyUnit unit)
        {
            bool isFirstLogin = AppInfoComponent.Instance.loginStep < 2;
            bool isMustSerData = (AppInfoComponent.Instance.isNewDevice == 1 || AppInfoComponent.Instance.isNewDevice == 2) && isFirstLogin;
            AppInfoComponent.Instance.loginStep++;
            UnitInfo unitInfo = unit.Unit;
            if (unitInfo.CurSkinId == 0)
            {
                Log.Console("服务端返回皮肤Id为0");
                unitInfo.CurSkinId = 3003;
            }

            self.curSkinId = unitInfo.CurSkinId;
            GameDataMgr.Instance.unitId = unit.Unit.UnitId;
            Log.Console("Init ID: " + GameDataMgr.Instance.unitId);
            //SkinConfig skinConfig = SkinConfigCategory.Instance.Get(self.curSkinId);
            //GameDataMgr.Instance.moduleId = skinConfig.CityModel;
            HallInfoComponent.Instance.SkinId = self.curSkinId;
            EventDispatcher.PostEvent(ETEventName.ChangeSkin,null,self.curSkinId);
            for (int i = 0; i < unitInfo.Ks.Count; i++)
            {
                if (unitInfo.Ks[i] == KeyDefine.PlatFormCoin)
                {
//                  self.platFormCoin = Convert.ToInt32(unitInfo.Vs[i]);
                    if (AppInfoComponent.Instance.isNewDevice == 3)
                    {
                        self.platFormCoin = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.init_diamond);
                    }else if(isMustSerData)
                        self.platFormCoin = Convert.ToInt32(unitInfo.Vs[i]);
                    else if(self.platFormCoin != Convert.ToInt32(unitInfo.Vs[i]))
                        EventDispatcher.PostEvent(EventName.SyncItemToServer, null, KeyDefine.PlatFormCoin, self.platFormCoin);
                    GameDataMgr.Instance.coin = self.platFormCoin;
                }
                else if (unitInfo.Ks[i] == KeyDefine.entityId)
                {
                    if (AppInfoComponent.Instance.isNewDevice == 3)
                    {
                        self.entityNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.init_life);
                    }
                    else if(isMustSerData)
                        self.entityNum = Convert.ToInt32(unitInfo.Vs[i]);
                    int defaultValue = InitConfigCategory.Instance.Get(1).physical;
                    int entityNum = self.entityNum;
                    int numServer = Convert.ToInt32(unitInfo.Vs[i]);
                    if (numServer > entityNum && entityNum != defaultValue)
                    {
                        EventDispatcher.PostEvent(EventName.EnterFunGameClear, null, numServer - entityNum);
                        // for (int j = 0; j < numServer - entityNum; j++)
                        // {
                        //     HallHelper.EnterFunGame(self.ZoneScene(), 1, () =>
                        //     {
                        //
                        //     }, true);
                        // }
                    }
                    if(entityNum != numServer)
                        HallHelper.SyncItem(self.ZoneScene(), KeyDefine.entityId, entityNum);
                    //self.entityNum = entityNum;
                }
                else if (unitInfo.Ks[i] == KeyDefine.MBLv)
                {
                    if (isMustSerData)
                    {
                        Log.Console("切换设备登录，采用Server数据");
                        self.mbLv = Convert.ToInt32(unitInfo.Vs[i]);
                        MBDataComponent.Instance.level = Convert.ToInt32(unitInfo.Vs[i]);
                        MBDataComponent.Instance.curPlayLevel = Convert.ToInt32(unitInfo.Vs[i]);
                    }
                }
                else if (unitInfo.Ks[i] == KeyDefine.star)
                {
                    if (isMustSerData)
                    {
                        Log.Console("切换设备登录，采用Server数据");
                        self.star = Convert.ToInt32(unitInfo.Vs[i]);
                    }
                }
                else if (unitInfo.Ks[i] == KeyDefine.GameCoin)
                {
                    int gameCoinSer = Convert.ToInt32(unitInfo.Vs[i]);
                    if (AppInfoComponent.Instance.isNewDevice == 3)
                    {
                        self.gameCoin = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.init_coin);
                    }else if(isMustSerData)
                        self.gameCoin = gameCoinSer;
                    int localCoin = self.gameCoin;
                    if (gameCoinSer < localCoin)
                    {
                        // int pickGold = PlayerPrefs.GetInt("PickGold", 0);
                        // if (pickGold > 0)
                        // {
                        //     HallHelper.PickGold(self.ZoneScene(), pickGold, 12);
                        // }
                        // else
                        {
                            HallHelper.SyncItem(self.ZoneScene(), KeyDefine.GameCoin, self.gameCoin);
                        }
                        //PlayerPrefs.SetInt("PickGold", 0);
                    }
                    else if (gameCoinSer == localCoin)
                    {
      //                  self.gameCoin = gameCoinSer;
                    }
                    else
                    {
                        HallHelper.SyncItem(self.ZoneScene(), KeyDefine.GameCoin, self.gameCoin);
                        //self.gameCoin = gameCoinSer;
                        Log.Console("服务端金币大于本地,ser = " + gameCoinSer + " local = " + localCoin);
                    }
                    GameDataMgr.Instance.gold = self.gameCoin;
                }
                else if (unitInfo.Ks[i] == KeyDefine.Exp)
                {
                    self.exp = Convert.ToInt32(unitInfo.Vs[i]);
                }
                else if (unitInfo.Ks[i] == KeyDefine.Level)
                {
                    self.level = Convert.ToInt32(unitInfo.Vs[i]);
                }
                // else if (unitInfo.Ks[i] == KeyDefine.GameGold)
                // {
                //     this.gameGold = unitInfo.Vs[i];
                // }
            }
            
            
            string chargeIds = PlayerPrefs.GetString(ItemSaveStr.chargeIds, "");
            if (chargeIds != "")
            {
                HallHelper.ChargeToSer(GlobalComponent.Instance.scene, chargeIds, "1","","", () =>
                {
                    PlayerPrefs.SetString(ItemSaveStr.chargeIds, "");
                    Log.Console("充值Ser返回。。。");
                }, true);
            }

            string rankList = PlayerPrefs.GetString("ResultList", "");
            if (rankList != "")
            {
                Log.Console(rankList);
                SingleLevel singleLevel = LitJson.JsonMapper.ToObject<SingleLevel>(rankList);
                //string[] rankRarry = rankList.Split(',');
                for (int i = 0; i < singleLevel.levelList.Count; i++)
                {
                    //HallHelper.GameResult(self.ZoneScene(), int.Parse(rankRarry[i]), 0, 8, null, true);
                    HallHelper.GameResult(self.ZoneScene(), singleLevel.levelList[i].rank, 0, 8, null, true, singleLevel.levelList[i].level, singleLevel.levelList[i].process, true);
                    if (singleLevel.levelList[i].rank == 1 && singleLevel.levelList[i].rank >= MBDataComponent.Instance.level)
                    {
                        MBDataComponent.Instance.curPlayLevel++;
                        MBDataComponent.Instance.level++;
                    }

                    // if (singleLevel.levelList[i].rank == 1)
                    // {
                    //     HallHelper.EnterFunGame(self.ZoneScene(), 1, () =>
                    //     {
                    //
                    //     }, true);
                    // }
                }
            }
            if (AppInfoComponent.Instance.loginStep >= 2)
                AppInfoComponent.Instance.isNewDevice = 0;
            PlayerPrefs.SetString("ResultList", "");
            
            EventDispatcher.PostEvent(EventName.MBLevelUpdate, null);
        }
        
        public static int GetItemNum(this HallInfoComponent self, int itemId)
        {
            //if (AppInfoComponent.Instance.singleState == SingleState.singled)
            {
                if (itemId == KeyDefine.reLastId)
                {
                    int defaultValue = CommonFuc.GetDefaultValue(itemId, InitConfigCategory.Instance.Get(1).initItem);
                    return PlayerPrefs.GetInt(ItemSaveStr.reLastId, defaultValue);
                }
                else if (itemId == KeyDefine.addGridId)
                {
                    int defaultValue = CommonFuc.GetDefaultValue(itemId, InitConfigCategory.Instance.Get(1).initItem);
                    return PlayerPrefs.GetInt(ItemSaveStr.addGridId, defaultValue);
                }
                else if (itemId == KeyDefine.reSortId)
                {
                    int defaultValue = CommonFuc.GetDefaultValue(itemId, InitConfigCategory.Instance.Get(1).initItem);
                    return PlayerPrefs.GetInt(ItemSaveStr.reSortId, defaultValue);
                }
                else if (itemId == KeyDefine.entityId)
                {
                    return self.entityNum;
                }
                else if (itemId == KeyDefine.GameCoin)
                {
                    return self.gameCoin;
                }
                else if (itemId == KeyDefine.PlatFormCoin)
                {
                    return self.platFormCoin;
                }
                else if (itemId == KeyDefine.unlockId)
                {
                    return self.UnLockNum;
                }
                else if (itemId == KeyDefine.star)
                {
                    return self.star;
                }
                else if(AppInfoComponent.Instance.singleState != SingleState.singled)
                {
                    int num = 0;
                    for (int i = 0; i < self.bagInfoList.ItemList.Count; i++)
                    {
                        if (self.bagInfoList.ItemList[i].ItemId == itemId)
                        {
                            num = (int)self.bagInfoList.ItemList[i].ItemNum;
                            break;
                        }
                    }
                    return num;
                }
                else
                {
                    return 0;
                }
            }

             
        }
        
        public static void ChgItemNum(this HallInfoComponent self, int itemId, int num, int chgNum = 0)
        {
            if (chgNum != 0 && num == 0)
            {
                num = self.GetItemNum(itemId) + chgNum;
                Log.Console("增加道具Id:" + itemId +" 数量：" + chgNum + " 总数：" + num);
            }
            if (num < 0)
                num = 0;
            switch (itemId)
            {
                case KeyDefine.reLastId:
                    PlayerPrefs.SetInt("reLastId", num);
                    break;
                case KeyDefine.addGridId:
                    PlayerPrefs.SetInt("addGridId", num);
                    break;
                case KeyDefine.reSortId:
                    PlayerPrefs.SetInt("reSortId", num);
                    break;
                case KeyDefine.entityId:
                    self.entityNum = num;
                    break;
                case KeyDefine.GameCoin:
                    self.gameCoin = num;
                    EventDispatcher.PostEvent(EventName.UpdateGameCoin, null, HallInfoComponent.Instance.gameCoin, HallInfoComponent.Instance.platFormCoin);
                    break;
                case KeyDefine.PlatFormCoin:
                    self.platFormCoin = num;
                    EventDispatcher.PostEvent(EventName.UpdateGameCoin, null, HallInfoComponent.Instance.gameCoin, HallInfoComponent.Instance.platFormCoin);
                    break;
                case KeyDefine.unlockId:
                    self.UnLockNum = num;
                    break;
            }
            //if (AppInfoComponent.Instance.singleState == SingleState.singled)
            //{
                // if (itemId == KeyDefine.reLastId)
                // {
                //     int defaultValue = CommonFuc.GetDefaultValue(itemId, InitConfigCategory.Instance.Get(1).initItem);
                //     int itemNum = PlayerPrefs.GetInt("reLastId", defaultValue);
                //     PlayerPrefs.SetInt("reLastId", num);
                // }
                // else if (itemId == KeyDefine.addGridId)
                // {
                //     int defaultValue = CommonFuc.GetDefaultValue(itemId, InitConfigCategory.Instance.Get(1).initItem);
                //     int itemNum = PlayerPrefs.GetInt("addGridId", defaultValue);
                //     PlayerPrefs.SetInt("addGridId", num);
                // }else if (itemId == KeyDefine.reSortId)
                // {
                //     int defaultValue = CommonFuc.GetDefaultValue(itemId, InitConfigCategory.Instance.Get(1).initItem);
                //     int itemNum = PlayerPrefs.GetInt("reSortId", defaultValue);
                //     PlayerPrefs.SetInt("reSortId", num);
                // }else if (itemId == KeyDefine.entityId)
                // {
                //     int defaultValue = CommonFuc.GetDefaultValue(itemId, InitConfigCategory.Instance.Get(1).initItem);
                //     int itemNum = PlayerPrefs.GetInt(ItemSaveStr.Entity, defaultValue);
                //     PlayerPrefs.SetInt(ItemSaveStr.Entity, num);
                // }
                // else if (itemId == KeyDefine.GameCoin)
                // {
                //     self.gameCoin = num;
                // }
                // else if (itemId == KeyDefine.GameCoin)
                // {
                //     self.gameCoin = num;
                // }
            //}
            if (AppInfoComponent.Instance.singleState == SingleState.singled)
            {
                return;
            }
            else
            {
                if (self.bagInfoList == null)
                {
                    return;
                }

                for (int i = 0; i < self.bagInfoList.ItemList.Count; i++)
                {
                    if (self.bagInfoList.ItemList[i].ItemId == itemId)
                    {
                        self.bagInfoList.ItemList[i].ItemNum = num;
                        break;
                    }
                }
            }
        }

        public static bool IsNetWork(this HallInfoComponent self)
        {
            if (HallHelper.gateSession == null || HallHelper.gateSession.IsDisposed || AppInfoComponent.Instance.photoConnect == false)
            {
                return false;
            }
            return true;
        }

        private static async void Shake()
        {
            await TimerComponent.Instance.WaitAsync(200);
            Handheld.Vibrate();
        }

        public static Dictionary<int, int> GetFreeItem(this HallInfoComponent self, int skinId)
        {
            Dictionary<int, int> freeItemDic = new Dictionary<int, int>();
            string getFreeStr = FireBComponent.Instance.GetRemoteString(FireBRemoteName.skin_property);
            if (string.IsNullOrEmpty(getFreeStr))
            {
                freeItemDic.Add(KeyDefine.reLastId, 1);
                freeItemDic.Add(KeyDefine.reSortId, 1);
                freeItemDic.Add(KeyDefine.addGridId, 1);
                return freeItemDic;
            }

            string[] freeArr = getFreeStr.Split(',');
            for (int i = 0; i < freeArr.Length; i++)
            {
                string[] freeLv = freeArr[i].Split(':');
//                Log.Console("freeLv:" + freeLv);
                int skinId0 = int.Parse(freeLv[0]);
                if (skinId0 == skinId)
                {
                    freeItemDic.Add(KeyDefine.reLastId, int.Parse(freeLv[1]));
                    freeItemDic.Add(KeyDefine.reSortId, int.Parse(freeLv[2]));
                    freeItemDic.Add(KeyDefine.addGridId, int.Parse(freeLv[3]));
                }
            }

            if (freeItemDic.Count == 0)
            {
                Log.Console("获取免费道具失败,skinId = " + skinId);
                freeItemDic.Add(KeyDefine.reLastId, 1);
                freeItemDic.Add(KeyDefine.reSortId, 1);
                freeItemDic.Add(KeyDefine.addGridId, 1);
            }

            return freeItemDic;
        }
        
        public static int GetSkinByLv(this HallInfoComponent self, int lv)
        {
            string getFreeStr = FireBComponent.Instance.GetRemoteString(FireBRemoteName.skin_lv_unlock);
            string[] freeArr = getFreeStr.Split(',');
            int skinId = 0;
            for (int i = 0; i < freeArr.Length; i++)
            {
                string[] freeLv = freeArr[i].Split(':');
                int lv0 = int.Parse(freeLv[0]);
                if (lv == lv0)
                {
                    skinId = int.Parse(freeLv[1]);
                }
            }

            return skinId;
        }
    }
    
    [ObjectSystem]
    public class UIHallInfoDestroySystem: DestroySystem<HallInfoComponent>
    {
        public override void Destroy(HallInfoComponent self)
        {
            EventDispatcher.RemoveObserver(EventName.UseItemNum);
            EventDispatcher.RemoveObserver(EventName.InitHallInfo);
            EventDispatcher.RemoveObserver(EventName.UpdateReRead);
            EventDispatcher.RemoveObserver(EventName.ChangeItemNum);
        }
    }
}