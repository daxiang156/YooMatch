using UnityEngine;
using System;
using System.Collections.Generic;

namespace ET
{
    public class HallInfoComponent : Entity, IAwake,IDestroy
    {
        public static HallInfoComponent Instance;
        private int _gameCoin = -1;
        private int _platFormCoin = -1;
        private int _star = 0;
        private int _entityNum = -1;
        /// <summary>
        /// 是否震屏
        /// </summary>
        private int vibrationState = 1;
        public int star
        {
            set
            {
                this._star = value;
                PlayerPrefs.SetInt(ItemSaveStr.Star, _star);
            }
            get
            {
                this._star = PlayerPrefs.GetInt(ItemSaveStr.Star, 0);
                return this._star;
            }
        }
        
        public int entityNum
        {
            set
            {
                this._entityNum = value;
                PlayerPrefs.SetInt("Entity", _entityNum);
            }
            get
            {
                //if (this._entityNum == -1)
                {
                    int defaultValue = InitConfigCategory.Instance.Get(1).physical;
                    this._entityNum = PlayerPrefs.GetInt("Entity", defaultValue);
                }

                return this._entityNum;
            }
        }
        
        public string country
        {
            set
            {
                PlayerPrefs.SetString("country", value);
            }
            get
            {
                return PlayerPrefs.GetString("country", "");
            }
        }

        public int pvpTime
        {
            set
            {
                PlayerPrefs.SetInt("pvpTime", value);
            }
            get
            {
                return PlayerPrefs.GetInt("pvpTime", 0);
            }
        }

        private int unLockNum = 0;
        public int UnLockNum
        {
            set
            {
                this.unLockNum = value;
                PlayerPrefs.SetInt(ItemSaveStr.unLockNum, unLockNum);
            }
            get
            {
                //if (this._entityNum == -1)
                {
                    this.unLockNum = PlayerPrefs.GetInt(ItemSaveStr.unLockNum, unLockNum);
                }
                return this.unLockNum;
            }
        }

        public int platFormCoin
        {
            set
            {
                this._platFormCoin = value;
                PlayerPrefs.SetInt("platFormCoin", this._platFormCoin);
            }
            get
            {
                if (this._platFormCoin == -1)
                {
                    int defaultValue = InitConfigCategory.Instance.Get(1).PlatformCoin;
                    this._platFormCoin = PlayerPrefs.GetInt("platFormCoin", defaultValue);
                }

                return this._platFormCoin;
            }
        }
        public int gameCoin
        {
            set
            {
                this._gameCoin = value;
                PlayerPrefs.SetInt("gameCoin", this.gameCoin);
            }
            get
            {
                if (this._gameCoin == -1)
                {
                    int defaultValue = InitConfigCategory.Instance.Get(1).GameCoin;
                    this._gameCoin = PlayerPrefs.GetInt("gameCoin", defaultValue);
                }

                return this._gameCoin;
            }
        }
        
        //英雄之路等级
        private int _level = -1;
        public int level
        {
            set
            {
                this._level = value;
                PlayerPrefs.SetInt("level", this._level);
            }
            get
            {
                if (this._level == -1)
                    this._level = PlayerPrefs.GetInt("level", 1);
                return this._level;
            }
        }

        private int _mblv = 1;
        public int mbLv
        {
            set
            {
                this._mblv = value;
                Log.Console("Server 消消乐关卡等级：" + value);
                PlayerPrefs.SetInt("mblv", this._mblv);
            }
            get
            {
                if (this._mblv == -1)
                    this._mblv = PlayerPrefs.GetInt("mblv", 1);
                return this._mblv;
            }
        }

        private int _skinId;
        public int SkinId
        {
            set
            {
                this._skinId = value;
                PlayerPrefs.SetInt("skinId",this._skinId);
            }
            get
            {
                if (this._skinId != -1)
                {
                    this._skinId = PlayerPrefs.GetInt("skinId", 1);
                }
                return this._skinId;
            }
        }
        
        private string _nickName;
        public string NickName
        {
            set
            {
                GameDataMgr.Instance.nickName = value;
                this._nickName = value;
                PlayerPrefs.SetString("nickName",this._nickName);
            }
            get
            {
                if (string.IsNullOrEmpty(this._nickName))
                {
                    this._nickName = PlayerPrefs.GetString("nickName", "");
                }
                return this._nickName;
            }
        }
        

        public int exp = 0;
        public int curSkinId = 0;

        private int _curMap;
        public int curMap {
            set
            {
                this._curMap = value;
            }
            get
            {
                return this._curMap;
            }
        }
        //public long gameGold;
        public int wearCapId;

        public SkinTabPage skinTabPage = SkinTabPage.skin;
        
        public GameObject cameraObj;

        public List<ActivityGoods> goodActivityDetail;
        /// <summary>
        /// 临时需要保存的Id
        /// </summary>
        public int goodId = -1;

        public string rewardList = "";
        public Action rewardCallBack = null;
        //public List<AchievevmentReward> itemList;

        public string playerName
        {
            set
            {
                GameDataMgr.Instance.playerName = value;
            }
            get
            {
                return GameDataMgr.Instance.playerName;
            }
        }

        //public SkinInfo curSKinInfo;
        private M2C_SkinInfo_List _skinInfoList;
        public M2C_SkinInfo_List skinInfoList
        {
            set
            {
                string skinInfoJs = LitJson.JsonMapper.ToJson(value);
                PlayerPrefs.SetString("skinInfo",skinInfoJs);
                
                this._skinInfoList = value;
                string skinStr = "";
                for (int i = 0; i < this._skinInfoList.SkinList.Count; i++)
                {
                    SkinInfo skinInfo = _skinInfoList.SkinList[i];
                    skinStr += skinInfo.SkinId + ":";
                    for (int kk = 0; kk < skinInfo.AppearanceList.Count; kk++)
                    {
                        skinStr += skinInfo.AppearanceList[kk] + ".";
                    }
                    if (skinInfo.AppearanceList.Count > 0)
                        skinStr = skinStr.Substring(0, skinStr.Length - 1);
                    skinStr += ":";
                    for (int kk = 0; kk < skinInfo.WearList.Count; kk++)
                    {
                        skinStr += skinInfo.WearList[kk] + ".";
                    }
                    if (skinInfo.WearList.Count > 0)
                        skinStr = skinStr.Substring(0, skinStr.Length - 1);
                    skinStr += ",";

                    if (skinInfo.SkinId == this.curSkinId)
                    {
                        var wearList = skinInfo.WearList;
                        for (int k = 0; k < wearList.Count; k++)
                        {
                            SetBagUid(k, skinInfo.WearList[k]);
                            //BagUId[k] = this._skinInfoList.SkinList[i].WearList[k];
                        }
                        this.ClothedUId = skinInfo.CurAppearanceId;
                        
                        
                        for (int j = 0; j < this.weaponList.Count; j++)
                        {
                            if (this.weaponUidList[j] == skinInfo.WearList[2])
                            {
                                int k = this.weaponList[j];
                                this.weaponList.RemoveAt(j);
                                this.weaponList.Insert(0, k);
                                
                                ItemConfig itemConfig0 = ItemConfigCategory.Instance.Get(k);
                                string weaponIndex = itemConfig0.modelName.Substring(2, 1);
                                if(itemConfig0.stype == ItemSmallType.Weapon)
                                    GameDataMgr.Instance.currentWeapon_Hammer = int.Parse(weaponIndex) - 1;
                                if (itemConfig0.stype == ItemSmallType.Balloon)
                                {
                                    weaponIndex = itemConfig0.modelName.Substring(4, 1);
                                    GameDataMgr.Instance.currentWeapon_RPG = int.Parse(weaponIndex) - 1;
                                }

                                GameDataMgr.Instance.weaponList.Clear();
                                for (int w = 0; w < this.weaponList.Count; w++)
                                {
                                    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(weaponList[w]);
                                    GameDataMgr.Instance.weaponList.Add(itemConfig.modelName);
                                }
                                break;
                            }
                        }
                    }
                }
                skinStr = skinStr.Substring(0, skinStr.Length - 1);
                //Log.Error(skinStr);
                PlayerPrefs.SetString(ItemSaveStr.skinList, skinStr);
            }
            get
            {
                string skinInfoJs = PlayerPrefs.GetString("skinInfo","");
                this._skinInfoList = LitJson.JsonMapper.ToObject<M2C_SkinInfo_List>(skinInfoJs);
                return this._skinInfoList;
            }
        }

        private M2C_GetBag_List _bagInfoList;
        public bool _isFirstBag = true;//第一次登录需要对比本地数据
        private List<int> weaponList = new List<int>();
        private List<long> weaponUidList = new List<long>();

        public M2C_GetBag_List bagInfoList
        {
            set
            {
                if (value == null)
                    return;
                string bagInfoJs = LitJson.JsonMapper.ToJson(value);
                PlayerPrefs.SetString("bagInfo",bagInfoJs);
                
                // if(AppInfoComponent.Instance.isWaitingSer)
                //     return;
                bool isFirstLogin = AppInfoComponent.Instance.loginStep < 2;
                bool isMustSerData = (AppInfoComponent.Instance.isNewDevice == 1 || AppInfoComponent.Instance.isNewDevice == 2) && isFirstLogin;
                AppInfoComponent.Instance.loginStep++;
                PlayerPrefs.SetInt(ItemSaveStr.accountFirstLogin, 1);
                _bagInfoList = value;
                GameDataMgr.Instance.weaponList.Clear();

                // //判断是否第一次连接服务器，区别换账号
                // bool isMustSerData = false;
                // for (int i = 0; i < this._bagInfoList.ItemList.Count; i++)
                // {
                //     int itemId = this._bagInfoList.ItemList[i].ItemId;
                //     if (itemId == KeyDefine.reLastId || itemId == KeyDefine.addGridId ||
                //         itemId == KeyDefine.reSortId)
                //     {
                //         int defaultValue = CommonFuc.GetDefaultValue(_bagInfoList.ItemList[i].ItemId, 
                //             InitConfigCategory.Instance.Get(1).initItem);
                //         if (this._bagInfoList.ItemList[i].ItemNum != defaultValue)
                //         {
                //             isMustSerData = true;
                //         }
                //     }
                // }
                // if (isFirstConnectServer && isMustSerData)
                // {
                //     Log.Console("玩家第一次连上服务器,且数据不是初始值");
                //     isMustSerData = true;
                // }
                // else
                // {
                //     isMustSerData = false;
                // }

                string itemStr = "";
                for (int i = 0; i < this._bagInfoList.ItemList.Count; i++)
                {
                    int itemId = this._bagInfoList.ItemList[i].ItemId;
                    itemStr += itemId + ":";
                    itemStr += this._bagInfoList.ItemList[i].ItemNum + ",";
                    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemId);
                    if (itemConfig.type == (int) ItemBigType.ItemType_Wear && itemConfig.stype == (int) ItemSmallType.Weapon)
                    {
                        weaponList.Add(itemId);
                        this.weaponUidList.Add(this._bagInfoList.ItemList[i].ItemUId);
                        GameDataMgr.Instance.weaponList.Add(itemConfig.modelName);
                    }

                    if (_bagInfoList.ItemList[i].ItemId == KeyDefine.reLastId)
                    {
                        int defaultValue = CommonFuc.GetDefaultValue(_bagInfoList.ItemList[i].ItemId, 
                            InitConfigCategory.Instance.Get(1).initItem);
                        if (AppInfoComponent.Instance.isNewDevice == 3)
                        {
                            //defaultValue = (int)FirebaseSetup.Instance.RCJ.GetRemoteLong(FireBRemoteName.init_undo);
                        }

                        int localNum = PlayerPrefs.GetInt(ItemSaveStr.reLastId, defaultValue);
                        if (this._isFirstBag)
                        {
                            if (localNum != defaultValue && _bagInfoList.ItemList[i].ItemNum > localNum)
                            {
                                EventDispatcher.PostEvent(EventName.UseItemNum, this, KeyDefine.reLastId,
                                    (int) _bagInfoList.ItemList[i].ItemNum - localNum);
                            }
                            if(localNum != _bagInfoList.ItemList[i].ItemNum && !isMustSerData)
                                EventDispatcher.PostEvent(EventName.SyncItemToServer, null, KeyDefine.reLastId, localNum);
                            PlayerPrefs.SetInt("reLastId", localNum);
                        }
                        if(isMustSerData)
                            PlayerPrefs.SetInt("reLastId", (int) _bagInfoList.ItemList[i].ItemNum);
                        // if(AppInfoComponent.Instance.isNewDevice == 3)
                        //     EventDispatcher.PostEvent(EventName.SyncItemToServer, null, KeyDefine.reLastId, localNum);
                    }
                    else if (_bagInfoList.ItemList[i].ItemId == KeyDefine.addGridId)
                    {
                        if (this._isFirstBag)
                        {
                            int defaultValue = CommonFuc.GetDefaultValue(_bagInfoList.ItemList[i].ItemId, 
                                InitConfigCategory.Instance.Get(1).initItem);
                            if (AppInfoComponent.Instance.isNewDevice == 3)
                            {
                                //defaultValue = (int)FirebaseSetup.Instance.RCJ.GetRemoteLong(FireBRemoteName.init_move);
                            }
                            int localNum = PlayerPrefs.GetInt(ItemSaveStr.addGridId, defaultValue);
                            if (localNum != defaultValue && _bagInfoList.ItemList[i].ItemNum > localNum)
                            {
                                EventDispatcher.PostEvent(EventName.UseItemNum, this, KeyDefine.addGridId,
                                    (int) _bagInfoList.ItemList[i].ItemNum - localNum);
                            }
                            if(localNum != _bagInfoList.ItemList[i].ItemNum && !isMustSerData)
                                EventDispatcher.PostEvent(EventName.SyncItemToServer, null, KeyDefine.addGridId, localNum);
                            PlayerPrefs.SetInt("addGridId", localNum);
                        }
                        if(isMustSerData)
                            PlayerPrefs.SetInt("addGridId", (int) _bagInfoList.ItemList[i].ItemNum);
                    }
                    else if (_bagInfoList.ItemList[i].ItemId == KeyDefine.reSortId)
                    {
                        if (this._isFirstBag)
                        {
                            int defaultValue = CommonFuc.GetDefaultValue(_bagInfoList.ItemList[i].ItemId, 
                                InitConfigCategory.Instance.Get(1).initItem);
                            if (AppInfoComponent.Instance.isNewDevice == 3)
                            {
                                //defaultValue = (int)FirebaseSetup.Instance.RCJ.GetRemoteLong(FireBRemoteName.init_shuffle);
                            }
                            int localNum = PlayerPrefs.GetInt(ItemSaveStr.reSortId, defaultValue);
                            if (localNum != defaultValue && _bagInfoList.ItemList[i].ItemNum > localNum)
                            {
                                EventDispatcher.PostEvent(EventName.UseItemNum, this, KeyDefine.reSortId,
                                    (int) _bagInfoList.ItemList[i].ItemNum - localNum);
                            }
                            if(localNum != _bagInfoList.ItemList[i].ItemNum && !isMustSerData)
                                EventDispatcher.PostEvent(EventName.SyncItemToServer, null, KeyDefine.reSortId, localNum);
                            PlayerPrefs.SetInt("reSortId", localNum);
                        }
                        if(isMustSerData)
                            PlayerPrefs.SetInt("reSortId", (int) _bagInfoList.ItemList[i].ItemNum);
                    }
                }

                if (AppInfoComponent.Instance.loginStep >= 2)
                    AppInfoComponent.Instance.isNewDevice = 0;
                itemStr = itemStr.Substring(0, itemStr.Length - 1);
                //Log.Error(itemStr);
                PlayerPrefs.SetString(ItemSaveStr.bagList, itemStr);
                EventDispatcher.PostEvent(EventName.MBLevelUpdate, null);
                this._isFirstBag = false;
            }
            get
            {
                string bagInfoJs = PlayerPrefs.GetString("bagInfo","");
                this._bagInfoList = LitJson.JsonMapper.ToObject<M2C_GetBag_List>(bagInfoJs);
                return this._bagInfoList;
            }
        }

        public M2C_GetAchievementInfo _heroRoadInfo;

        public M2C_GetAchievementInfo heroRoadInfo
        {
            set
            {
                this._heroRoadInfo = value;
                PlayerPrefs.SetInt("curProgress", (int)this._heroRoadInfo.curProgress);
            }
            get
            {
                return this._heroRoadInfo;
            }
        }
        public M2C_GetAchievementReward getHeroRodReward;

        // public void Init(M2C_CreateMyUnit unit)
        // {
        //     UnitInfo unitInfo = unit.Unit;
        //     curSkinId = unitInfo.CurSkinId;
        //     GameDataMgr.Instance.unitId = unit.Unit.UnitId;
        //     Log.Console("Init ID: " + GameDataMgr.Instance.unitId);
        //     // GameDataMgr.Instance.moduleId = "Player_6_Modified";
        //     // if (this.curSkinId == 1002)
        //     // {
        //     //     GameDataMgr.Instance.moduleId = "Player_4_modified";
        //     // }
        //     // else
        //     // {
        //     //     GameDataMgr.Instance.moduleId = "Player_6_Modified";
        //     // }
        //     SkinConfig skinConfig = SkinConfigCategory.Instance.Get(this.curSkinId);
        //     GameDataMgr.Instance.moduleId = skinConfig.CityModel;
        //
        //     for (int i = 0; i < unitInfo.Ks.Count; i++)
        //     {
        //         if (unitInfo.Ks[i] == KeyDefine.PlatFormCoin)
        //         {
        //             this.platFormCoin = Convert.ToInt32(unitInfo.Vs[i]);
        //             GameDataMgr.Instance.coin = this.platFormCoin;
        //         }
        //         else if (unitInfo.Ks[i] == KeyDefine.GameCoin)
        //         {
        //             int gameCoinSer = Convert.ToInt32(unitInfo.Vs[i]);
        //             int localCoin = this.gameCoin;
        //             if (gameCoinSer < localCoin)
        //             {
        //                 int pickGold = PlayerPrefs.GetInt("PickGold", 0);
        //             }
        //             else
        //                 this.gameCoin = Convert.ToInt32(unitInfo.Vs[i]);
        //             GameDataMgr.Instance.gold = this.gameCoin;
        //         }
        //         else if (unitInfo.Ks[i] == KeyDefine.Exp)
        //         {
        //             this.exp = Convert.ToInt32(unitInfo.Vs[i]);
        //         }
        //         else if (unitInfo.Ks[i] == KeyDefine.Level)
        //         {
        //             this.level = Convert.ToInt32(unitInfo.Vs[i]);
        //         }
        //         // else if (unitInfo.Ks[i] == KeyDefine.GameGold)
        //         // {
        //         //     this.gameGold = unitInfo.Vs[i];
        //         // }
        //     }
        // }
        //
        
        private long[] _bagUid = new long[4];

        public long[] BagUId
        {
            get => this._bagUid;
            set
            {
                this._bagUid = value;
                for (int i = 0; i < bagInfoList.ItemList.Count; i++)
                {
                    for (int j = 0; j < this._bagUid.Length; j++)
                    {
                        if (bagInfoList.ItemList[i].ItemUId == this._bagUid[j])
                        {
                            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(bagInfoList.ItemList[i].ItemId);
                            GameDataMgr.Instance.bagModelName[itemConfig.stype] = itemConfig.modelName;
                        }
                    }
                }
            }
        }

        public void SetBagUid(long index, long bagUid)
        {
            this._bagUid[index] = bagUid;
            GameDataMgr.Instance.bagModelName.Clear();
            for (int i = 0; i < bagInfoList.ItemList.Count; i++)
            {
                for (int j = 0; j < this._bagUid.Length; j++)
                {
                    if (bagInfoList.ItemList[i].ItemUId == this._bagUid[j])
                    {
                        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(bagInfoList.ItemList[i].ItemId);
                        GameDataMgr.Instance.bagModelName[itemConfig.stype] = itemConfig.modelName;
                        
                        
                        string weaponIndex = itemConfig.modelName.Substring(2, 1);
                        if(itemConfig.stype == ItemSmallType.Weapon)
                            GameDataMgr.Instance.currentWeapon_Hammer = int.Parse(weaponIndex) - 1;
                        if (itemConfig.stype == ItemSmallType.Balloon)
                        {
                            weaponIndex = itemConfig.modelName.Substring(4, 1);
                            GameDataMgr.Instance.currentWeapon_RPG = int.Parse(weaponIndex) - 1;
                        }
                    }
                }
            }
        }

        public void ClearWear()
        {
            for (int i = 0; i < 4; i++)
            {
                this.BagUId[i] = 0;
            }

            GameDataMgr.Instance.bagModelName.Clear();
        }

        private int _clothesUid = 0;

        public int ClothedUId
        {
            get => this._clothesUid;
            set
            {
                this._clothesUid = value;
                if (_clothesUid != 0)
                {

                    string clothes = ItemConfigCategory.Instance.Get(_clothesUid).modelName;
                    //string clothes = SkinConfigCategory.Instance.Get(this.curSkinId).Clothes;
                    GameDataMgr.Instance.clothesId = "" + clothes;
                    Log.Console("GameDataMgr.Instance.clothesId :" + GameDataMgr.Instance.clothesId);
                    // for (int i = 0; i < this._skinInfoList.SkinList.Count; i++)
                    // {
                    //     if (_skinInfoList.SkinList[i].SkinId == this._clothesUid)
                    //     {
                    //         GameDataMgr.Instance.bagModelName = ItemConfigCategory.Instance.Get(bagInfoList.ItemList[i].ItemId).modelName;
                    //     }
                    // }
                }
            }
        }

        public int enterRank = 2;
        public Rank2C_GetRansInfo rankList;
        public Dictionary<string, Rank2C_GetRansInfo> allRankDic = new Dictionary<string, Rank2C_GetRansInfo>();
        public string emailAddress = "";
        public int isCheckVersion = 0;
        public string account = "";
        
        public bool isDeletingAccount = false;

        private Dictionary<int, int> _removeDic = new Dictionary<int, int>();
        public Dictionary<int, int> removeDic {
            get
            {
                return this._removeDic;
            }
            set
            {
                this._removeDic = value;
            }
        }
        
        public int VibrationState
        {
            get
            {
                if(vibrationState == - 1)
                    vibrationState= PlayerPrefs.GetInt("vibrationState",0);
                return vibrationState;
            }
            set
            {
                vibrationState = value;
                PlayerPrefs.SetInt("vibrationState",value);
            }
        }
    }

    public enum CheckVersion
    {
        none = 0,
        IOSCheck = 1,
    }
}