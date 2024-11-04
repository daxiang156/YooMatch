using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class MBDataSystem: AwakeSystem<MBDataComponent>
    {
        public override void Awake(MBDataComponent self)
        {
            MBDataComponent.Instance = self;
            GameDataMgr.Instance.mbLv = self.level;
            self.failTime = 0;
            AddListen();
            
            self.freeNumList.Add(0);
            self.freeNumList.Add(0);
            self.freeNumList.Add(0);
            
            self.InitMoreFreeItemNum();
        }

        private void AddListen()
        {
            EventDispatcher.AddObserver(this, EventName.EnterFunGame, (object[] userInfo) =>
            {
                bool isEnter = (bool)userInfo[0];
                int lv = (int)userInfo[1];

                String enterMb = PlayerPrefs.GetString(ItemSaveStr.EnterMb, "");
                if (isEnter)
                {
                    if (enterMb == "")
                        enterMb = lv.ToString();
                    else
                    {
                        enterMb = enterMb + ":" + lv;
                    }
                }
                else
                {
                    List<string> lvs = enterMb.Split(':').ToList();
                    for (int i = lvs.Count - 1; i >= 0; i--)
                    {
                        if (lvs[i] == lv.ToString())
                        {
                            lvs.RemoveAt(i);
                            break;
                        }
                    }

                    enterMb = "";
                    for (int i = 0; i < lvs.Count; i++)
                    {
                        if (i != lvs.Count - 1)
                        {
                            enterMb += lvs[i] + ":";
                        }
                        else
                        {
                            enterMb += lvs[i];
                        }
                    }
                }
                PlayerPrefs.SetString(ItemSaveStr.EnterMb, enterMb);

                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, EventName.EnterFunGameClear, (object[] userInfo) =>
            {
                int count = (int)userInfo[0];
                Log.Console("差额Entity：" + count);
                String enterMb = PlayerPrefs.GetString(ItemSaveStr.EnterMb, "");
                List<string> lvs = enterMb.Split(':').ToList();
                for (int i = 0; i < lvs.Count; i++)
                {
                    int lv = 0;
                    if (int.TryParse(lvs[i], out lv))
                    {
                        lv = int.Parse(lvs[i]);
                        HallHelper.EnterFunGame(GlobalComponent.Instance.scene, lv, FireBComponent.Instance.GetRemoteString(FireBRemoteName.free_item_level),
                            FireBComponent.Instance.GetRemoteString(FireBRemoteName.level_difficulty), null, true);
                    }
                }
                PlayerPrefs.SetString(ItemSaveStr.EnterMb, "");
                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, "ADAlreadyLoad", (object[] userInfo) =>
            {
                int pathType = (int) userInfo[0];
                HallHelper.ADLog(GlobalComponent.Instance.scene, 15,  -1, MBDataComponent.Instance.curPlayLevel,  (long)TimeInfo.Instance.ClientNow(), false, pathType);
                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, ETEventName.GetFunyGameFailData, (object[] userInfo) =>
            {
                HallHelper.GetFunyGameEvent(GlobalComponent.Instance.scene, MBDataComponent.Instance.level, () =>
                {
                    var list = MBDataComponent.Instance.funyLevelList;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].event_id == MBSignType.FailTime)
                        {
                            MBDataComponent.Instance.failTime = list[i].event_value;
                        }
                    }
                });
                return false;
            }, null);
        }
    }

    public static class UIMBDataSystem
    {
        // public static void IsNowItemLv(this MBDataComponent self)
        // {
        //     Log.Console("IsNowItemLv");
        //     self.curFruitItem = 0;
        //     int rangNum = UnityEngine.Random.Range(0, 100);
        //     string strRemote = FireBComponent.Instance.GetRemoteString(FireBRemoteName.MB_Fruit_Type);
        //     // if (GameDataMgr.Instance.Platflam() == PlatForm.Win)
        //     // {
        //     //     strRemote = "5:11:20,6:12:20";
        //     // }
        //
        //     Log.Console(strRemote);
        //     if (string.IsNullOrEmpty(strRemote))
        //     {
        //         self.curFruitItem = 0;
        //         return;
        //     }
        //
        //     Log.Console(strRemote);
        //     string[] levels = strRemote.Split(',');
        //     for (int i = 0; i < levels.Length; i++)
        //     {
        //         if(string.IsNullOrEmpty(levels[i]))
        //             break;
        //         string[] str = levels[i].Split(':');
        //         int lv = int.Parse(str[0]);
        //         if (lv == self.curPlayLevel)
        //         {            
        //             self.curFruitRangNum = int.Parse(str[2]);
        //             self.curFruitItem = int.Parse(str[1]);
        //             return;
        //         }
        //     }
        //     self.curFruitItem = 0;
        //     return;
        // }
        
        public static int IsItemFruit(this MBDataComponent self, int itemId)
        {
            if (self.curFruitItem == 0)
                return 0;
            if (self.level < 5)
                return 0;
            int rangNum = UnityEngine.Random.Range(0, 100);
            if (rangNum <= self.curFruitRangNum)
                return MBItemType.BlindBox;// self.curFruitItem;
            return 0;
        }
    
        public static MBLvAll CloneMbConfig(this MBDataComponent self, int Id, int Row, int Num, int NumOffset, string ItemList)
        {
            MBLvAll mbLvAll = new MBLvAll();
            mbLvAll.Id = Id;
            mbLvAll.Row = Row;
            mbLvAll.Num = Num;
            mbLvAll.NumOffset = NumOffset;
            mbLvAll.ItemList = ItemList;
            return mbLvAll;
        }

        public static int GetFreeItemNum(this MBDataComponent self, int itemId)
        {
            if (itemId == KeyDefine.reLastId)
                return self.freeNumList[0];
            else if (itemId == KeyDefine.reSortId)
                return self.freeNumList[1];
            else if (itemId == KeyDefine.addGridId)
                return self.freeNumList[2];
            return 1;
        }
        
        public static int GetMoreFreeItemNum(this MBDataComponent self, int itemId)
        {
            if (itemId == KeyDefine.reLastId)
                return self.mbMoreFreeNumList[0];
            else if (itemId == KeyDefine.reSortId)
                return self.mbMoreFreeNumList[1];
            else if (itemId == KeyDefine.addGridId)
                return self.mbMoreFreeNumList[2];
            return 0;
        }
        
        public static void InitMoreFreeItemNum(this MBDataComponent self)
        {
            // if (self.mbMoreFreeNumList.Count == 0)
            // {
            //     self.mbMoreFreeNumList.Add(1);
            //     self.mbMoreFreeNumList.Add(1);
            //     self.mbMoreFreeNumList.Add(1);
            // }
            // else
            // {
            //     self.mbMoreFreeNumList[0] = 1;
            //     self.mbMoreFreeNumList[1] = 1;
            //     self.mbMoreFreeNumList[2] = 1;
            // }
        }
        
        public static void ChgMoreFreeItemNum(this MBDataComponent self, int itemId, int itemNum = -1)
        {
            if (itemId == KeyDefine.reLastId)
                self.mbMoreFreeNumList[0] += itemNum;
            else if (itemId == KeyDefine.reSortId)
                self.mbMoreFreeNumList[1] += itemNum;
            else if (itemId == KeyDefine.addGridId)
                self.mbMoreFreeNumList[2] += itemNum;
        }
        
        public static void ChgFreeItemNum(this MBDataComponent self, int itemId, int itemNum = -1)
        {
            if (itemId == KeyDefine.reLastId)
                self.freeNumList[0] -= 1;
            else if (itemId == KeyDefine.reSortId)
                self.freeNumList[1] -= 1;
            else if (itemId == KeyDefine.addGridId)
                self.freeNumList[2] -= 1;
        }

        public static void SetFreeNum(this MBDataComponent self, int curLv, bool setLv = true)
        {
            // if (setLv)
            // {
            //     self.curPlayLevel = curLv;
            //     GameDataMgr.Instance.mbLv = curLv;
            // }
            //
            // Dictionary<int, int> freeNumDic = HallInfoComponent.Instance.GetFreeItem(HallInfoComponent.Instance.curSkinId);
            // foreach (var item in freeNumDic)
            // {
            //     if (item.Key == KeyDefine.reLastId)
            //         self.freeNumList[0] = item.Value;
            //     else if (item.Key == KeyDefine.reSortId)
            //         self.freeNumList[1] = item.Value;
            //     else if (item.Key == KeyDefine.addGridId)
            //         self.freeNumList[2] = item.Value;
            // }
            
            // if(setLv)
            //     self.curPlayLevel = curLv;
            // string freeNumStr = FireBComponent.Instance.GetRemoteString(FireBRemoteName.free_item_level);
            // if (string.IsNullOrEmpty(freeNumStr))
            // {
            //     self.freeNumList[0] = 2;
            //     self.freeNumList[1] = 2;
            //     self.freeNumList[2] = 2;
            //     return 2;
            // }
            // string[] freeNumArr = freeNumStr.Split(',');
            // for (int i = 0; i < freeNumArr.Length; i++)
            // {
            //     string[] freeLvNum = freeNumArr[i].Split(':');
            //     int level = int.Parse(freeLvNum[0]);
            //     if (level == curLv)
            //     {
            //         self.freeNumList[0] = int.Parse(freeLvNum[1]);
            //         self.freeNumList[1] = int.Parse(freeLvNum[2]);
            //         self.freeNumList[2] = int.Parse(freeLvNum[3]);
            //         return int.Parse(freeLvNum[1]);
            //     }
            // }
            // return 2;
        }

        public static Dictionary<int, int> GetFreeStr(this MBDataComponent self, int skinId = 0)
        {
            if (skinId == 0)
                skinId = HallInfoComponent.Instance.curSkinId;
            Dictionary<int, int> freeNumStr = HallInfoComponent.Instance.GetFreeItem(skinId);
            //string str = "";
            // string[] freeNumArr = freeNumStr.Split(',');
            // for (int i = 0; i < freeNumArr.Length; i++)
            // {
            //     string[] freeLvNum = freeNumArr[i].Split(':');
            //     int level = int.Parse(freeLvNum[0]);
            //     if (level == MBDataComponent.Instance.curPlayLevel)
            //     {
            //         str = KeyDefine.reLastId + ":" + freeLvNum[1] + "," + KeyDefine.reSortId + ":" + freeLvNum[2] + "," + KeyDefine.addGridId + ":" +
            //                 freeLvNum[3];
            //     }
            // }
            return freeNumStr;
        }

        public static bool IsJumpLevel(this MBDataComponent self)
        {
            string[] clientVersion = GameDataMgr.Instance.version.Split('.');
            if (int.Parse(clientVersion[2]) <= 32 && int.Parse(clientVersion[1]) == 0 && int.Parse(clientVersion[0]) == 1)
            {
                return false;
            }
            
            bool jumpLevel = false;
            string jumpLv = FireBComponent.Instance.GetRemoteString(FireBRemoteName.jump_level);
            string[] jump_level = jumpLv.Split(':');
            for (int i = 0; i < jump_level.Length; i++)
            {
                if(string.IsNullOrEmpty(jump_level[i]))
                    break;
                if (int.Parse(jump_level[i]) == MBDataComponent.Instance.curPlayLevel)
                {
                    jumpLevel = true;
                    break;
                }
            }
            EventDispatcher.PostEvent(ETEventName.SetJump_level, null, jumpLv);
            return jumpLevel;
        }

        public static Dictionary<int, MBLvAll> GetConfig(this MBDataComponent self, int curLv)
        {
            Dictionary<int, MBLvAll> configs1 = new Dictionary<int, MBLvAll>();
            // if (curLv == 1 || GlobalComponent.Instance.mbDic != null)
            // {
            //     return null;
            // }

            //EventDispatcher.PostEvent(ETEventName.FireBProperty, null, PropertyName.mb_level_ab, mbLvAB.ToString());
            //int mbLvAB = 1;
            //curLv = 1;
            switch (@curLv)
            {
            case 1:
                foreach (var item in MBLv1Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }

                break;
            case 2:
                foreach (var item in MBLv2Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }

                break;
            case 3:
                foreach (var item in MBLv3Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }

                break;
            case 4:
                foreach (var item in MBLv4Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 5:
                foreach (var item in MBLv5Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 6:
                foreach (var item in MBLv6Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 7:
                foreach (var item in MBLv7Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 8:
                foreach (var item in MBLv8Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 9:
                foreach (var item in MBLv9Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 10:
                foreach (var item in MBLv10Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 11:
                foreach (var item in MBLv11Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 12:
                foreach (var item in MBLv12Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 13:
                foreach (var item in MBLv13Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 14:
                foreach (var item in MBLv14Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 15:
                foreach (var item in MBLv15Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 16:
                foreach (var item in MBLv16Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 17:
                foreach (var item in MBLv17Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 18:
                foreach (var item in MBLv18Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 19:
                foreach (var item in MBLv19Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 20:
                foreach (var item in MBLv20Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 21:
                foreach (var item in MBLv21Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 22:
                foreach (var item in MBLv22Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 23:
                foreach (var item in MBLv23Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 24:
                foreach (var item in MBLv24Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 25:
                foreach (var item in MBLv25Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 26:
                foreach (var item in MBLv26Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 27:
                foreach (var item in MBLv27Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 28:
                foreach (var item in MBLv28Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 29:
                foreach (var item in MBLv29Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 30:
                foreach (var item in MBLv30Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 31:
                foreach (var item in MBLv31Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 32:
                foreach (var item in MBLv32Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 33:
                foreach (var item in MBLv33Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 34:
                foreach (var item in MBLv34Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }

                break;
            case 35:
                foreach (var item in MBLv35Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 36:
                foreach (var item in MBLv36Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 37:
                foreach (var item in MBLv37Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 38:
                foreach (var item in MBLv38Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 39:
                foreach (var item in MBLv39Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 40:
                foreach (var item in MBLv40Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            
            case 41:
                foreach (var item in MBLv41Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 42:
                foreach (var item in MBLv42Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 43:
                foreach (var item in MBLv43Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 44:
                foreach (var item in MBLv44Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }

                break;
            case 45:
                foreach (var item in MBLv45Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 46:
                foreach (var item in MBLv46Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 47:
                foreach (var item in MBLv47Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 48:
                foreach (var item in MBLv48Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 49:
                foreach (var item in MBLv49Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            
            
            case 50:
                foreach (var item in MBLv50Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            
            case 51:
                foreach (var item in MBLv51Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 52:
                foreach (var item in MBLv52Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 53:
                foreach (var item in MBLv53Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 54:
                foreach (var item in MBLv54Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }

                break;
            case 55:
                foreach (var item in MBLv55Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 56:
                foreach (var item in MBLv56Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 57:
                foreach (var item in MBLv57Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 59:
                foreach (var item in MBLv59Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 60:
                foreach (var item in MBLv60Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            
            case 61:
                foreach (var item in MBLv61Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            case 62:
                foreach (var item in MBLv62Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
            default:
                foreach (var item in MBLv58Category.Instance.GetAll())
                {
                    var config = item.Value;
                    MBLvAll mbLvAll =
                            MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                break;
        }
            return configs1;
        }
    }

    [ObjectSystem]
    public class UIMBDataDestroySystem: DestroySystem<MBDataComponent>
    {
        public override void Destroy(MBDataComponent self)
        {
            EventDispatcher.RemoveObserver(EventName.EnterFunGame);
            EventDispatcher.RemoveObserver(EventName.EnterFunGameClear);
            EventDispatcher.RemoveObserver(ETEventName.GetFunyGameFailData);
        }
    }
}