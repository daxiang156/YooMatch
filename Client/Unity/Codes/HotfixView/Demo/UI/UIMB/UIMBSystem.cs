using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ET.Data;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ET
{
    public class UIMBDestory: DestroySystem<UIMBComponent>
    {
        public override void Destroy(UIMBComponent self)
        {
            self.uiSetUpPanel = null;
            (self.uiAniExpand as UIAniExpand).Dispose();
            self.uiAniExpand = null;
        }
    }

    public class UIMBUpDate: UpdateSystem<UIMBComponent>
    {
        public override void Update(UIMBComponent self)
        {
            if (self.modelMgr != null)
            {
                (self.modelMgr as ModelMgr).UpDate();
            }
        }
    }

    [ObjectSystem]
    public class UIMBAwakeSystem : AwakeSystem<UIMBComponent>
    {
        private UIMBComponent self;
        private bool isEnd = false;
        private bool isCanClick = true;
        private bool _isWaitOrder = false;
        private bool isWaitOrder
        {
            set
            {
                this._isWaitOrder = value;
            }
            get
            {
                return this._isWaitOrder;
            }
        }
        private int rangNum = 0;
        private int totalFruit = 0;
        private int maxFruitNum = 8;
        private int goldLv = 0;//是否有消三个金币水果得金币

        //每个水果数据计数，便于统计是否是3的倍数
        List<int> countList = new List<int>();
        private Dictionary<int, UIMBItemComponent> itemList = new Dictionary<int, UIMBItemComponent>();
        private List<Sprite> spriteList = new List<Sprite>();
        Dictionary<int, int> ItemUseNumDic = new Dictionary<int, int>();
//        private UIMBGoTips goTipsScript;
        public override void Awake(UIMBComponent self)
        {
            Log.Console("mb awake");
            if (GameDataMgr.Instance.skipLv > 0)
                MBDataComponent.Instance.curPlayLevel = MBDataComponent.Instance.level = GameDataMgr.Instance.skipLv;
            if (MBDataComponent.Instance.level > 62)
                MBDataComponent.Instance.curPlayLevel = MBDataComponent.Instance.level % 28 + 30; 
            this.ReSetItemNum();
            this.self = self;
            self.gameSelf = self.GetParent<UI>().GameObject;
            this.self.uiAniExpand = new UIAniExpand(new List<GameObject>() { GameObjectMgr.Refer(self.gameSelf,"top"),
                                                                            null,
                                                                            GameObjectMgr.Refer(self.gameSelf,"left"),
                                                                            GameObjectMgr.Refer(self.gameSelf,"right")});
            //self.gameSelf.SetActive(false);
            ReferenceCollector rc = self.gameSelf.GetComponent<ReferenceCollector>();
            self.btnReturn = rc.Get<GameObject>("btnReturn").GetComponent<Button>();
            //   self.btnItem = rc.Get<GameObject>("btnItem").GetComponent<Button>();
            
            self.btnReLast = rc.Get<GameObject>("btnReLast").GetComponent<Button>();
            self.btnAddGrid = rc.Get<GameObject>("btnAddGrid").GetComponent<Button>();
            self.btnUpdate = rc.Get<GameObject>("btnUpdate").GetComponent<Button>();
            // self.btnShop = rc.Get<GameObject>("btnShop").GetComponent<Button>();
            // self.btnLucky = rc.Get<GameObject>("btnLucky").GetComponent<Button>();
            // self.btnQuest = rc.Get<GameObject>("btnQuest").GetComponent<Button>();

            // self.btnReLastAd = rc.Get<GameObject>("btnReLastAd").GetComponent<Button>();
            // self.btnAddGridAd = rc.Get<GameObject>("btnAddGridAd").GetComponent<Button>();
            // self.btnUpdateAd = rc.Get<GameObject>("btnUpdateAd").GetComponent<Button>();
            self.textRelast = rc.Get<GameObject>("textRelast").GetComponent<Text>();
            self.textAddgrid = rc.Get<GameObject>("textAddgrid").GetComponent<Text>();
            self.textUpdate = rc.Get<GameObject>("textUpdate").GetComponent<Text>();
            self.spineResort = rc.Get<GameObject>("spineResort").GetComponent<SkeletonGraphic>();
            self.spineResort.gameObject.SetActive(false);
            self.qipao = rc.Get<GameObject>("qipao").GetComponent<SkeletonGraphic>();
            self.paopaobaozha = rc.Get<GameObject>("paopaobaozha");
            self.qipao.gameObject.SetActive(false);
            self.oneItem = rc.Get<GameObject>("oneItem");
            self.bottomItem = rc.Get<GameObject>("bottomItem");
  //          self.lastGridRed = rc.Get<GameObject>("lastGridRed");
            self.addGridTips = rc.Get<GameObject>("addGridTips");
            self.extraGridBg = rc.Get<GameObject>("extraGridBg").GetComponent<Image>();
//            self.txtGuide = rc.Get<GameObject>("txtGuide").GetComponent<Text>();
            
            self.percent = rc.Get<GameObject>("percent").GetComponent<Slider>();
            self.percentTxt = self.percent.transform.Find("percent").GetComponent<Text>();
            self.textEntity = rc.Get<GameObject>("textEntity").GetComponent<Text>();
            self.textEntity.text = HallInfoComponent.Instance.GetItemNum(KeyDefine.entityId).ToString();
            self.txtLevel = rc.Get<GameObject>("txtLevel").GetComponent<Text>();
            self.particleGold = rc.Get<GameObject>("particleGold");
            self.particleFruit = rc.Get<GameObject>("particleFruit");
            self.particleGold.SetActive(false);
            self.upGrid1 = rc.Get<GameObject>("upGrid").transform;
            self.upGrid2 = rc.Get<GameObject>("upGrid2").transform;
            self.upGrid = self.upGrid1;
            self.txtAllFruit = rc.Get<GameObject>("txtAllFruit").GetComponent<Text>();
            self.txtNowFruit = rc.Get<GameObject>("txtNowFruit").GetComponent<Text>();
            self.menu = rc.Get<GameObject>("Menu");
            self.imgBlindBox = rc.Get<GameObject>("imgBlindBox").GetComponent<Image>();
            self.ItemLock = rc.Get<GameObject>("ItemLock").GetComponent<Image>();
            self.ItemKey = rc.Get<GameObject>("ItemKey").GetComponent<Image>();
            self.ItemIce = rc.Get<GameObject>("ItemIce").GetComponent<Image>();
            self.lockSpine = rc.Get<GameObject>("lockSpine").GetComponent<SkeletonGraphic>();
            self.iceSpine = rc.Get<GameObject>("iceSpine").GetComponent<SkeletonGraphic>();

            this.RefreshLockGrid();
            //self.txtLevel.text = "Level " + MBDataComponent.Instance.curPlayLevel;
//            self.textCoin = rc.Get<GameObject>("textCoin").GetComponent<Text>();
            

            self.goCoin = rc.Get<GameObject>("coin");
            //            (this.self.uiMbCoin as ItemShowSystem).SetItemId(3);

            self.greyUpdate = rc.Get<GameObject>("greyUpdate");
            self.greyAddGrid = rc.Get<GameObject>("greyAddGrid");
            self.greyRelast = rc.Get<GameObject>("greyRelast");
            self.free = rc.Get<GameObject>("free");
            self.mask = rc.Get<GameObject>("mask");
            self.fruitParent = rc.Get<GameObject>("fruitParent");
            self.overParent = rc.Get<GameObject>("overParent").transform;
            self.mask.SetActive(false);
            self.flyShader = rc.Get<GameObject>("flyShader").GetComponent<Image>();

            self.goUpNum = GameObjectMgr.Refer(rc.gameObject, "goUpNum");
            self.GoTips = rc.Get<GameObject>("GoTips");
//            goTipsScript = new UIMBGoTips(self.GoTips);
 //           self.btnTipsClick = rc.Get<GameObject>("btnTipsClick").GetComponent<Button>();
            

            Text txtNum = GameObjectMgr.Refer(this.self.goCoin, "txt_num").GetComponent<Text>();
            txtNum.text = UIMBDataHelper.Instance.MoneyValue.ToString();
            
            self.btnSetUp = rc.Get<GameObject>("btnSetUp").GetComponent<Button>();
            this.self.goSetPanel = rc.Get<GameObject>("goSetPanel");
            
            self.peoples = rc.Get<GameObject>("peoples").transform;
            if (self.uiSetUpPanel == null)
            {
                self.uiSetUpPanel = new UISetUpPanel(this.self.goSetPanel,this.self.btnSetUp.gameObject);
            }
            
            HallInfoComponent.Instance.curMap = MapDefine.MonkeyBusiness;
            InitItemUseNum();

            EventDispatcher.PostEvent(EventName.CameraRendMode, this, false);
 //           self.lastGridRed.SetActive(false);
 //           self.MB_Star.SetActive(false);
            self.oneItem.gameObject.SetActive(false);
            self.GetParent<UI>().GameObject.SetActive(false);
            //GameInfcoinoComponent.Instance.result = null;
            rangNum = Random.Range(0, maxFruitNum);
            if (MBDataComponent.Instance.level == 1)
                this.rangNum = 0;
            isEnd = false;
            for (int i = 1; i <= 15; i++)
            {
                Transform goDown = self.bottomItem.transform.Find(i.ToString());
                if (i == 5)
                {
                    goDown.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
                        if(this.maxNum == UIMBDataHelper.AddGridId || UIMBDataHelper.Instance.GetGridState(UIMBDataHelper.AddGridId)) 
                            return;
                        UIHelper.Create(GlobalComponent.Instance.scene,UIType.UIUnLockTips,UILayer.Mid).Coroutine();
                    });
                }

                self.downPosList.Add(goDown);
            }
            for (int i = 101; i <= 103; i++)
            {
                self.itemGridPosList.Add(self.bottomItem.transform.Find(i.ToString()));
            }
            //addGridItemNum = 3;
            spriteList.Clear();
            int fruitSelect = 1;//FireBComponent.Instance.GetRemoteLong(FireBRemoteName.fruit_select);
            if(fruitSelect == 1)
            {
                Transform tran = self.fruitParent.transform;
                for (int i = 0; i < tran.childCount; i++)
                {
                    Sprite sprite = tran.GetChild(i).GetComponent<Image>().sprite;
                    this.spriteList.Add(sprite);
                }
                try
                {
                    if (fruitSelect == 2)
                        EventDispatcher.PostEvent(ETEventName.FireBProperty, null, PropertyName.mb_level_ab, "20");
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            // if (GlobalComponent.Instance.mbDic == null)
            // {
            //     InitItemNumText();
            // }

            this.asideDic.Clear();
            GameDataMgr.Instance.curConnectType = ConnectType.MBGame;
            this.AddListner();
            this.SortItem().Coroutine();
 //           SetGuideInfo();
            //SetItemBtnState();
            SetGoogleLoginEvent().Coroutine();
            PurchaseInit().Coroutine();
            
            // if (ETCommonFunc.IsTablet())
            // {
            //     // self.btnLucky.gameObject.SetActive(false);
            //     // self.btnShop.transform.parent.gameObject.SetActive(false);
            //     // self.btnQuest.gameObject.SetActive(false);
            //     //self.btnAddGrid.transform.parent.gameObject.SetActive(false);
            // }
        }

        void InitItemUseNum()
        {
            //GameObject paizi = GameObject.Find("scene/fbx_01/02_08/1");
            // if (paizi)
            // {
            //     var camera = GlobalComponent.Instance.mainCamera.GetComponent<Camera>();
            //     Vector3 playerPos = camera.WorldToScreenPoint(paizi.transform.position);
            //     Vector2 screenPos = camera.WorldToScreenPoint(paizi.transform.position);
            //     RectTransformUtility.ScreenPointToWorldPointInRectangle(self.txtAllFruit.transform.parent.GetComponent<RectTransform>(), screenPos, GlobalComponent.Instance.UICamera.GetComponent<Camera>(), out Vector3 beginPos);
            //
            //     this.self.txtNowFruit.transform.parent.position = beginPos;
            // }

//            int ad_times_per_game = FireBComponent.Instance.GetRemoteLong(FireBRemoteName.ad_times_per_game);
            int ad_times_per_game = 1;
            if (ItemUseNumDic.ContainsKey(KeyDefine.reLastId))
            {
                ItemUseNumDic[KeyDefine.reLastId] = ad_times_per_game;
                ItemUseNumDic[KeyDefine.reSortId] = ad_times_per_game;
                ItemUseNumDic[KeyDefine.addGridId] = ad_times_per_game;
            }
            else
            {
                ItemUseNumDic.Add(KeyDefine.reLastId, ad_times_per_game);
                ItemUseNumDic.Add(KeyDefine.reSortId, ad_times_per_game);
                ItemUseNumDic.Add(KeyDefine.addGridId, ad_times_per_game);
            }
        }

        private void SetItemBtnState()
        {
            this.self.greyUpdate.SetActive(MBDataComponent.Instance.level < GuideLv.resort);
            this.self.greyAddGrid.SetActive(MBDataComponent.Instance.level < GuideLv.addGrid);
            if (MBDataComponent.Instance.level == GuideLv.addGrid)
            {
                this.self.greyAddGrid.SetActive(AppInfoComponent.Instance.guideStep != (int)GuideStep.EndAddGrid);
            }if (MBDataComponent.Instance.level == GuideLv.resort)
            {
                this.self.greyUpdate.SetActive(AppInfoComponent.Instance.guideStep != (int)GuideStep.EndUpdate);
            }

            // if (MBDataComponent.Instance.freeNumList[0] > 0 || HallInfoComponent.Instance.GetItemNum(KeyDefine.reLastId) > 0 ||
            //     MBDataComponent.Instance.curPlayLevel == KeyDefine.guideLevel)
            // {
            //     this.self.btnReLastAd.gameObject.SetActive(false);
            // }
            // else
            // {
            //     this.self.btnReLastAd.gameObject.SetActive(true);
            // }
            // if (MBDataComponent.Instance.freeNumList[1] > 0 || HallInfoComponent.Instance.GetItemNum(KeyDefine.reSortId) > 0 ||
            //     MBDataComponent.Instance.curPlayLevel == KeyDefine.guideLevel)
            // {
            //     this.self.btnUpdateAd.gameObject.SetActive(false);
            // }
            // else
            // {
            //     this.self.btnUpdateAd.gameObject.SetActive(true);
            // }
            // if (MBDataComponent.Instance.freeNumList[2] > 0 || HallInfoComponent.Instance.GetItemNum(KeyDefine.addGridId) > 0 ||
            //     MBDataComponent.Instance.curPlayLevel == KeyDefine.guideLevel)
            // {
            //     this.self.btnAddGridAd.gameObject.SetActive(false);
            // }
            // else
            // {
            //     this.self.btnAddGridAd.gameObject.SetActive(true);
            // }
        }

        private async ETTask PurchaseInit()
        {
            await TimerComponent.Instance.WaitAsync(2000);
            EventDispatcher.PostEvent(EventName.PurchaseInit, null);
            await ETTask.CompletedTask;
        }

        private async ETTask SetGoogleLoginEvent()
        {
            await TimerComponent.Instance.WaitAsync(5000);
            if (AppInfoComponent.Instance.AppStartSign % 10 == 0)
            {
                // EventDispatcher.PostEvent(EventName.Play_MB_LoginGoogle, null, MBDataComponent.Instance.curPlayLevel);
                // AppInfoComponent.Instance.AppStartSign += 1;
            }
        }

        private async void SetGuideInfo()
        {
            // GameObject parent = this.self.txtGuide.transform.parent.gameObject;
            // parent.SetActive(false);
            // while (this.self.gameSelf != null)
            // {
            //     int timeWait = Random.Range(50000, 120000);
            //     if(parent == null)
            //         return;
            //     parent.SetActive(true);
            //     int id = timeWait % 7 + 2001;
            //     this.self.txtGuide.text = LanguageComponent.Instance.GetLanguage(id);
            //     await TimerComponent.Instance.WaitAsync(5000);
            //     if(parent != null)
            //             parent.SetActive(false);
            //     await TimerComponent.Instance.WaitAsync(timeWait);
            //}
            await ETTask.CompletedTask;
        }

        private string order = "";
        //private bool isInMBedit = false;
        private  async ETTask SortItem()
        {
            InitItemNumText();
            if (MBDataComponent.Instance.level == GuideLv.resort && AppInfoComponent.Instance.guideStep != (int) GuideStep.EndUpdate)
            {
                AppInfoComponent.Instance.guideStep = (int)GuideStep.UpdateUse;
                this.self.greyUpdate.SetActive(false);
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGuide, UILayer.High).Coroutine();
            }//8
            else if (MBDataComponent.Instance.level == GuideLv.useBlind && AppInfoComponent.Instance.guideStep != (int) GuideStep.EndBlind)
            {
                AppInfoComponent.Instance.guideStep = (int)GuideStep.UseBlind;
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGuide, UILayer.High).Coroutine();
            }//16
            else if (MBDataComponent.Instance.level == GuideLv.useIce && AppInfoComponent.Instance.guideStep != (int) GuideStep.EndIce)
            {
                AppInfoComponent.Instance.guideStep = (int)GuideStep.UseIce;
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGuide, UILayer.High).Coroutine();
            }//20
            else if (MBDataComponent.Instance.level == GuideLv.UseKey && AppInfoComponent.Instance.guideStep != (int) GuideStep.End3)
            {
                AppInfoComponent.Instance.guideStep = (int)GuideStep.UseKey;
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGuide, UILayer.High).Coroutine();
            }

            this.RefreshLockGrid();
            SoundComponent.Instance.PlayBgMusic_Res("Music/bgm");
            this.self.textRelast.transform.parent.gameObject.SetActive(MBDataComponent.Instance.curPlayLevel != KeyDefine.guideLevel);
            this.self.textAddgrid.transform.parent.gameObject.SetActive(MBDataComponent.Instance.curPlayLevel != KeyDefine.guideLevel);
            this.self.textUpdate.transform.parent.gameObject.SetActive(MBDataComponent.Instance.curPlayLevel != KeyDefine.guideLevel);

            if (GlobalComponent.Instance.mbDic != null)
            {
                MBDataComponent.Instance.reveiveNum = 3001;
            }
            else
  //              MBDataComponent.Instance.reveiveNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.game_ad_revive_num);

            //MBDataComponent.Instance.reveiveNum = 3999;
            MBDataComponent.Instance.alreadyReveiveNum = 0;
            EventDispatcher.AddObserver(this, EventName.SendGoogleAdsReward, (object[] info) =>
            {
                GoogleAdsEnum adsType = (GoogleAdsEnum) info[0];
                GoogleAdsPath adsPath = (GoogleAdsPath) info[1];
                EventDispatcher.PostEvent(EventName.NetWaitUI, null, false);
                if (adsPath == GoogleAdsPath.MBRevive)
                {
                    UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIRevive).Coroutine();
                    EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Ad_Complete_Revive);
                    MBDataComponent.Instance.reveiveNum--;
                    this.Reveive();
                    return false;
                }
                if (adsPath != GoogleAdsPath.MBGameRelast && adsPath != GoogleAdsPath.MBGameAddGrid && adsPath != GoogleAdsPath.MBGameResort)
                    return false;
                
                EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Ad_Complete_Game);
                List<int> itemIdList = new List<int>();
                List<int> itemNumList = new List<int>();

                string reward = "1:";
                if (adsPath == GoogleAdsPath.MBGameRelast)
                {
                    if(ItemUseNumDic[KeyDefine.reLastId] > 0)
                        ItemUseNumDic[KeyDefine.reLastId]--;
                    int adItemNum = 3;
                    //if (GameDataMgr.Instance.Platflam() != PlatForm.Win)
 //                       adItemNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.game_ad_undo);
                    reward = reward + KeyDefine.reLastId + ":" + adItemNum;
                    HallInfoComponent.Instance.ChgItemNum(KeyDefine.reLastId, 0, adItemNum);
                    itemIdList.Add(KeyDefine.reLastId);
                    itemNumList.Add(adItemNum);
                }
                else if (adsPath == GoogleAdsPath.MBGameResort)
                {
                    if(ItemUseNumDic[KeyDefine.reSortId] > 0)
                        ItemUseNumDic[KeyDefine.reSortId]--;
                    int adItemNum = 3;
                    //if (GameDataMgr.Instance.Platflam() != PlatForm.Win)
 //                       adItemNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.game_ad_shuffle);
                    reward = reward + KeyDefine.reSortId + ":" + adItemNum;
                    HallInfoComponent.Instance.ChgItemNum(KeyDefine.reSortId, 0, adItemNum);
                    itemIdList.Add(KeyDefine.reSortId);
                    itemNumList.Add(adItemNum);
                }
                else if (adsPath == GoogleAdsPath.MBGameAddGrid)
                {
                    if(ItemUseNumDic[KeyDefine.addGridId] > 0)
                        ItemUseNumDic[KeyDefine.addGridId]--;
                    int adItemNum = 3;
                    //if (GameDataMgr.Instance.Platflam() != PlatForm.Win)
//                        adItemNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.game_ad_move);
                    reward = reward + KeyDefine.addGridId + ":" + adItemNum;
                    HallInfoComponent.Instance.ChgItemNum(KeyDefine.addGridId, 0, adItemNum);
                    itemIdList.Add(KeyDefine.addGridId);
                    itemNumList.Add(adItemNum);
                }
                
                //InitItemNumText();
                HallInfoComponent.Instance.rewardList = reward;
                UIHelper.Create(self.ZoneScene(), UIType.UIReward, UILayer.Mid).Coroutine();
                if (HallHelper.gateSession == null || HallHelper.gateSession.IsDisposed)
                {
                    EventDispatcher.PostEvent(EventName.ConnectMySerInGame, this);
                }
                EventDispatcher.PostEvent(EventName.UpdateItemInMB, null);
                HallHelper.GetADAward(GlobalComponent.Instance.scene, itemIdList, itemNumList);
                HallHelper.ADLog(GlobalComponent.Instance.scene, 16, 0, MBDataComponent.Instance.level, (long)TimeInfo.Instance.ClientNow());
                return false;
            }, null);
            EventDispatcher.PostEvent(EventName.AppsFlyEvent, null, AppsEventName.Play_MB);
            EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Play_MB);
            EventDispatcher.PostEvent(ETEventName.VoodooEvent, null, MBMode.NormalStart, 0, MBDataComponent.Instance.level);
            if(GameDataMgr.Instance.version != "1.0.34")
                EventDispatcher.PostEvent(ETEventName.FaceBookEvent, null, FaceBookEventName.Play_MB);
            EventDispatcher.PostEvent(ETEventName.Dev2Dev, this.self, "Play_MB", MBDataComponent.Instance.curPlayLevel);

            if (MBDataComponent.Instance.curPlayLevel > 1)
            {
                EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Puzzle_Play);
            }

            EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBaseEventName.Puzzle_Level_ + MBDataComponent.Instance.curPlayLevel);
           
            await TimerComponent.Instance.WaitAsync(1);
            for (int i = 0; i <= 18; i++)
                this.countList.Add(0);
            int curLv = MBDataComponent.Instance.curPlayLevel;
            if (curLv == -1)
            {
                MBDataComponent.Instance.curPlayLevel = MBDataComponent.Instance.level;
                curLv = MBDataComponent.Instance.level;
            }

            Dictionary<int, MBLv1> configs = null;
            if (curLv == 1)
            {
                configs = MBLv1Category.Instance.GetAll();
            }
            if (GlobalComponent.Instance.mbDic != null)
            {
                GameDataMgr.Instance.CanPassMB = 1;
                rangNum = 0;
                configs = GlobalComponent.Instance.mbDic;

                AppInfoComponent.Instance.guideStep = (int) GuideStep.UpdateUse + 10;
                MBDataComponent.Instance.curPlayLevel = KeyDefine.guideLevel;
            }

//            int lvIndex = 0;
            Dictionary<int, MBLvAll> configs1 = null;//new Dictionary<int, MBLvAll>();
            if (GlobalComponent.Instance.mbDic != null)
            {
                configs1 = new Dictionary<int, MBLvAll>();
                foreach (var item in configs)
                {
                    var config = item.Value;
                    MBLvAll mbLvAll = MBDataComponent.Instance.CloneMbConfig(config.Id, config.Row, config.Num, config.NumOffset, config.ItemList);
                    configs1.Add(item.Key, mbLvAll);
                }
                //isInMBedit = true;
                //this.self.btnReLast.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                configs1 = MBDataComponent.Instance.GetConfig(curLv);
            }
            Dictionary<int, MBLvAll> configs0 = SimpleConfig(configs1);

            goldLv = 0;
            // string[] level_gold = FireBComponent.Instance.GetRemoteString(FireBRemoteName.gold_level).Split(':');
            // for (int i = 0; i < level_gold.Length; i++)
            // {
            //     if(string.IsNullOrEmpty(level_gold[i]))
            //         break;
            //     if (int.Parse(level_gold[i]) == MBDataComponent.Instance.curPlayLevel)
            //     {
            //         goldLv = 1;
            //         break;
            //     }
            // }
            
            Dictionary<int, Dictionary<int, List<MBLvOneItem>>> listConfig = new Dictionary<int, Dictionary<int, List<MBLvOneItem>>>();
            for(int i = 0; i < 20; i++)
                listConfig.Add(i, new Dictionary<int, List<MBLvOneItem>>());
            foreach (var VARIABLE in configs0)
            {
                var config = VARIABLE.Value;
                if (config.Id == 1)
                {
                    order = config.ItemList;
                    continue;
                }

                string[] items = config.ItemList.Split(',');
                for (int i = 0; i < items.Length; i++)
                {
                    if(string.IsNullOrEmpty(items[i]))
                        continue;
                    int itemId = int.Parse(items[i]);
                    int level = i;
                    if(itemId == 0)
                        continue;
                    MBLvOneItem mbLvOneItem = new MBLvOneItem(config.Id, config.Row, config.Num, config.RowOffset, config.NumOffset,itemId, level);
                    if (!listConfig[i].ContainsKey(config.Num))
                    {
                        listConfig[i].Add(config.Num, new List<MBLvOneItem>());
                    }
                    listConfig[i][config.Num].Add(mbLvOneItem);
                }
            }
            CreateUpList();

            for (int i = 0; i <= maxLayer; i++)
            {
                if(!listConfig.ContainsKey(i))
                    continue;
                for (int j = 19; j > -19; j--)
                {
                    if(!listConfig[i].ContainsKey(j))
                        continue;
                    List<MBLvOneItem> list = listConfig[i][j];
                    for (int k = 0; k < list.Count; k++)
                    {
                        InstallGrid(list[k].Row, list[k].Num, "", list[k].Id, i, list[k].RowOffset, list[k].NumOffset, list[k].itemId, list[k].level);

                    }
                }
            }
            SortAllItem(null);

            // for (int i = 0; i <= maxLayer; i++)
            // {
            //     lvIndex++;
            //     //if (lvIndex % 1 == 0)
            //         await TimerComponent.Instance.WaitAsync(1);
            //     
            //     foreach (var VARIABLE in configs0)
            //     {
            //         var config = VARIABLE.Value;
            //         InstallGrid(config.Row, config.Num, config.ItemList, config.Id, i, config.RowOffset, config.NumOffset, goldLv);
            //     }
            // }

            totalFruit = this.self.allGridDic.Count;

            //Log.Console("每个水果数量");
            // for (int i = 0; i < this.countList.Count; i++)
            // {
            //     if (this.countList[i] % 3 != 0)
            //     {
            //         Log.Console((i + this.rangNum) + " 配置表有水果数量不是3的倍数，数量为"  + this.countList[i].ToString());
            //     }
            // }
            
            InitItemUseNum();
            self.gameSelf.SetActive(true);

            self.percent.enabled = true;
            self.percent.value = 0;
            self.percent.enabled = false;
            self.percentTxt.text = "0%";
            self.txtLevel.text = string.Format(LanguageComponent.Instance.GetLanguage(2160),MBDataComponent.Instance.level);
//            this.goTipsScript.SetCurLv(MBDataComponent.Instance.level);
            EventDispatcher.PostEvent(EventName.MBAgainAlready, this);
            EventDispatcher.PostEvent(EventName.DefaultBg, this, false);
            //if (AppInfoComponent.Instance.guideStep == (int) GuideStep.MainCityFirst) 
            if (MBDataComponent.Instance.level == GuideLv.first && AppInfoComponent.Instance.guideStep != (int) GuideStep.End)
            {
                AppInfoComponent.Instance.guideStep = (int) GuideStep.MainCityFirst;
                // async void func()
                // {
                //     goTipsScript.SetShow(true,true);
                //     await TimerComponent.Instance.WaitAsync(1500);
                //     await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGuide, UILayer.Mid2);
                //     await TimerComponent.Instance.WaitAsync(1);
                //     if (this.self.GoTips.activeSelf)
                //     {
                //         goTipsScript.SetShow(true,false);
                //     }
                // }
                //
                // func();
                
                await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGuide, UILayer.High);
            }
            if (UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIMBLoading) != null)
            {
                UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIMBLoading).Coroutine();
            }
            if(UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIMBResult) != null)
            {
                UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIMBResult).Coroutine();
            }
            if(UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIMBRank) != null)
            {
                UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIMBRank).Coroutine();
            }
            if(UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIMBMap) != null)
            {
                UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIMBMap).Coroutine();
            }
            if(UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIMBLoadingNew) != null)
            {
                UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIMBLoadingNew).Coroutine();
            }
            
            DynamicDownLoadMgr.GetInstance().PreDownLoadBg();
            await TimerComponent.Instance.WaitAsync(1);

            //留存事件
            DateTime dataNow = DateTime.Now;
            if (PlayerPrefs.GetInt(ItemSaveStr.Retained, 0) != dataNow.Month * 100 + dataNow.Day)
            {
                AppInfoComponent.Instance.playMBTimes++;
                string account = UnityEngine.PlayerPrefs.GetString(ItemSaveStr.account);
                long timeSign = 0;
                if (long.TryParse(account, out timeSign) && account.StartsWith("1") && account.Length == 13)
                {
                    DateTime data = TimeInfo.Instance.ToDateTime(timeSign);
                    if ((data.Month == dataNow.Month && data.Day != dataNow.Day) || (data.Month != dataNow.Month))
                    {
                        if (AppInfoComponent.Instance.playMBTimes == 1)
                        {
                            EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBaseEventName.Retained_Play5Times);
                            EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBaseEventName.Retained);
                            if(data.Month == dataNow.Month && data.Day == dataNow.Day - 1)
                            {
                                EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBaseEventName.Retained_NextDay);
                            }

                            if ((data.Month == dataNow.Month && data.Day <= dataNow.Day - 4)
                                || (data.Month != dataNow.Month - 1 && 
                                    (dataNow.Day > 3 || data.Day < 27   || (data.Day < 29 && dataNow.Day > 2) || (data.Day < 28 && dataNow.Day > 1)))
                                )
                            {
                                EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBaseEventName.Retained_4DaysPlus);
                            }

                            PlayerPrefs.SetInt(ItemSaveStr.Retained, dataNow.Month * 100 + dataNow.Day);
                        }
                    }
                    else if (AppInfoComponent.Instance.playMBTimes == 6 && data.Month == dataNow.Month && data.Day == dataNow.Day) //当日
                    {
                        EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBaseEventName.RePlay5Times);
                        EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBaseEventName.Retained_Play5Times);
                        PlayerPrefs.SetInt(ItemSaveStr.Retained, dataNow.Month * 100 + dataNow.Day);
                    }
                }
            }
        }

        private int maxLayer = 0;
        private Dictionary<int, MBLvAll> SimpleConfig(Dictionary<int, MBLvAll> configDic)
        {
            maxLayer = 0;
            int totalItem = 0;
            foreach (var config in configDic)
            {
                if (config.Value.Id == 1)
                {
                    this.order = config.Value.ItemList;
                    continue;
                }
                string itemStr = config.Value.ItemList;
                string[] items = itemStr.Split(',');
                for (int k = 0; k < items.Length; k++)
                {
                    int itemId = 0;
                    if (!int.TryParse(items[k], out itemId))
                    {
                        Log.Error("ItemId error:" + items[k]);
                        continue;
                    }

                    if (itemId != 0 && itemId < 1000)
                    {
                        totalItem += 1;
//                        countList[itemId] = countList[itemId] + 1;
                    }
                }
                if (maxLayer < items.Length)
                    maxLayer = items.Length;
            }

            float difficulty = 1;//GetDifficult(MBDataComponent.Instance.curPlayLevel) / 1000f;
            if(AppInfoComponent.Instance.percentMBEdit > 0)
                difficulty = AppInfoComponent.Instance.percentMBEdit / 1000f;

            int killNum = (int)(totalItem / 3 * (1 - difficulty));

            //change ItemId
            if(MBDataComponent.Instance.level == MBDataComponent.Instance.curPlayLevel)
                configDic = ChangeFruitByFailTime(configDic);

            if (killNum == 0 || difficulty == 1)
                return configDic;
            int alreadyKillNum = 0;
            Dictionary<string, MBConfigItem> killDic = new Dictionary<string, MBConfigItem>();
            for (int i = 0; i < maxLayer; i++)
            {
                bool isFinish = false;
                foreach (var config in configDic)
                {
                    string itemStr = config.Value.ItemList;
                    string[] items = itemStr.Split(',');
                    if(items.Length <= i)
                        continue;
                    else
                    {
                        string key = items[i];
                        if(key == "0")
                            continue;
                        if (killDic.ContainsKey(key))
                        {
                            //killDic[key].num += 1;
                            killDic[key].Add(config.Key, i);

                            if (killDic[key].num >= 3)
                            {
                                //kill
                                for (int m = killDic[key].itemPosDic.Count - 1; m >= 0; m--)
                                {
                                    var killItem = killDic[key].itemPosDic[m];
                                    int killConfigId = killItem.configId;
                                    int pos = killItem.index;
                                    List<string> killItems = configDic[killConfigId].ItemList.Split(',').ToList();
//                                    Log.Console("kill configId:" + killConfigId + " pos:" + pos);
                                    if (killItems.Count <= pos)
                                    {
                                        Log.Error("kill configId:" + killConfigId + " pos:" + pos);
                                    }
                                    killItems[pos] = "0";
                                    //killItems.RemoveAt(pos);
                                    configDic[killConfigId].ItemList = "";
                                    for (int k = 0; k < killItems.Count; k++)
                                    {
                                        if(k == killItems.Count - 1)
                                            configDic[killConfigId].ItemList += killItems[k];
                                        else
                                            configDic[killConfigId].ItemList += killItems[k] + ",";
                                    }
                                    killDic[key].itemPosDic.Remove(killItem);
                                    killDic[key].num = 0;
                                }
                                alreadyKillNum++;
                                if (alreadyKillNum >= killNum)
                                {
                                    isFinish = true;
                                    killDic[key].itemPosDic.Clear();
                                    killDic[key].num = 0;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            MBConfigItem configItem = new MBConfigItem();
                            configItem.Init(config.Key, i);
                            killDic.Add(key, configItem);
                        }
                    }
                }
                if(isFinish)
                    break;
            }
            //Log.Console(configDic.ToString());
            return configDic;
        }

        private Dictionary<int, MBLvAll> ChangeFruitByFailTime(Dictionary<int, MBLvAll> configDic)
        {
            // int itemTypeNum = 0;
            // List<int> itemConfigList = new List<int>();
            // for (int i = 0; i < this.countList.Count; i++)
            // {
            //     if (this.countList[i] > 0)
            //     {
            //         itemTypeNum++;
            //         itemConfigList.Add(i);
            //     }
            // }
            
            // int fail_reduce_num = FireBComponent.Instance.GetRemoteLong(FireBRemoteName.fail_reduce_num);
            // if (MBDataComponent.Instance.curPlayLevel < MBDataComponent.Instance.maxlevel - 50)
            //     fail_reduce_num = fail_reduce_num / 1000;
            // else
            // {
            //     fail_reduce_num = fail_reduce_num % 1000;
            // }
            //
            // int reduceNum = MBDataComponent.Instance.failTime / fail_reduce_num;
            // if (itemTypeNum - reduceNum < MBDataComponent.Instance.maxItemType)
            //     reduceNum = itemTypeNum - MBDataComponent.Instance.maxItemType;
            // reduceNum = reduceNum >= 0 ? reduceNum : 0;
            
            //Log.Console("水果种数：" + itemTypeNum + ",  减去水果种类：" + reduceNum + "， 失败次数：" + MBDataComponent.Instance.failTime);
            //if (MBDataComponent.Instance.failTime <= 0)
                return configDic;
            // if (reduceNum == 0)
            //     return configDic;
            // EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBaseEventName.Reduce_Rruit, "num", fail_reduce_num);
            // Dictionary<int, int> changeFruitDic = new Dictionary<int, int>();
            // int count = itemConfigList.Count;
            // for (int k = 0; k < reduceNum; k++)
            // {
            //     changeFruitDic.Add(itemConfigList[k], itemConfigList[count - 1 - k]);
            // }
            //
            // foreach (var config in configDic)
            // {
            //     string itemStr = config.Value.ItemList;
            //     string[] items = itemStr.Split(',');
            //     for (int k = 0; k < items.Length; k++)
            //     {
            //         foreach (var changeItem in changeFruitDic)
            //         {
            //             if (items[k] == changeItem.Key.ToString())
            //             {
            //                 items[k] = changeItem.Value.ToString();
            //             }
            //         }
            //     }
            //
            //     config.Value.ItemList = "";
            //     for (int k = 0; k < items.Length; k++)
            //     {
            //         if (k == items.Length - 1)
            //             config.Value.ItemList += items[k];
            //         else
            //             config.Value.ItemList += items[k] + ",";
            //     }
            // }
            // return configDic;
        }

        private int GetDifficult(int lv)
        {
            //string levelDifficulty = FireBComponent.Instance.GetRemoteString(FireBRemoteName.level_difficulty);
            int difficulty = 1000;
            // if (!string.IsNullOrEmpty(levelDifficulty))
            // {
            //     string[] levels = levelDifficulty.Split(',');
            //     for (int k = 0; k < levels.Length; k++)
            //     {
            //         string[] levelOne = levels[k].Split(':');
            //         int level = int.Parse(levelOne[0]);
            //         if (level == lv)
            //         {
            //             difficulty = int.Parse(levelOne[1]);
            //             break;
            //         }
            //     }
            // }
            return difficulty;
        }
        
        void InstallGrid(int row, int num, string itemStr, int configId, int level, int offsetX, int offsetY, int configItemId, int lv)
        {
            // string[] items = itemStr.Split(',');
            // if(items.Length <= level)
            //     return;
            //
            // int itemId = int.Parse(items[level]);
            // if(itemId == 0)
            //     return;
            // int offsetNum = 0;
            // for (int i = 0; i < items.Length && i < level; i++)
            // {
            //     if (items[i] != "0")
            //     {
            //         offsetNum++;
            //     }
            // }
            // var pos = new Vector3(row * 41,num * 41,0);
            // if (offsetX != 0)
            // {
            //     pos.x += (offsetNum - 1) * offsetX;
            // }else if (offsetY != 0)
            // {
            //     pos.y += (offsetNum - 1) * offsetY;
            // }
            int itemId = configItemId;
            var pos = new Vector3(row * 58,num * 57 + 200,0);
            
            UIMBItemComponent mbItemComponent = CreateItem(row, num, level, itemId, pos, configId);
            int dic2dId = CommonFuc.SetItemId(row, num, 0);
            if (!self.allOneGridDic.ContainsKey(dic2dId))
            {
                Dictionary<int, UIMBItemComponent> gridDic = new Dictionary<int, UIMBItemComponent>();
                gridDic.Add(level, mbItemComponent);
                this.self.allOneGridDic.Add(dic2dId, gridDic);
            }
            else
            {
                this.self.allOneGridDic[dic2dId].Add(level, mbItemComponent);
            }
            
            ///
            List<int> idList = CommonFuc.GetRoundId(dic2dId);
            for (int k = 0; k < idList.Count; k++)
            {
                int id0 = idList[k];
                if (this.self.allOneGridDic.ContainsKey(id0))
                {
                    Dictionary<int, UIMBItemComponent> onGrids = self.allOneGridDic[id0];
                    foreach (var oneGrid in onGrids)
                    {
                        UIMBItemComponent grid0 = oneGrid.Value;
                        if (grid0.level < level)
                        {
                            grid0.AddOverGrid(mbItemComponent.itemUnitId, mbItemComponent);
                        }
                    }
                }
            }
        }


        private UIMBItemComponent CreateItem(int row, int num, int level, int itemId, Vector3 pos, int configId, int mBItemType = -100, int isIce = 0)
        {
            int dicId = CommonFuc.SetItemId(row, num, level);
            GameObject item = GameObject.Instantiate(this.self.oneItem.gameObject, this.self.oneItem.transform.parent);
            item.name = dicId.ToString();
            item.SetActive(true);
            //item.transform.localScale = Vector3.one;

            UI ui = self.AddChild<UI,string,GameObject>(UIType.UIMBItem, item);
            UIMBItemComponent mbItemComponent = ui.AddComponent<UIMBItemComponent>();
            mbItemComponent.isDown = false;
            item.transform.localPosition = pos;
            if (itemId > 100 && itemId <= 100 + this.maxFruitNum)
            {
                itemId = itemId - 100;
                mbItemComponent.itemEffectNum = MBItemType.BlindBox;
            }else if (itemId > 200 && itemId <= 200 + this.maxFruitNum)
            {
                itemId = itemId - 200;
                mbItemComponent.itemEffectNum = MBItemType.ItemIce;
            }

            Button btn = item.transform.Find("btnItem").GetComponent<Button>();
            Button btnBg = item.transform.Find("bgImg").GetComponent<Button>();
            Image img = btn.gameObject.GetComponent<Image>();
            int imgId = itemId;
            if (itemId < 100)
            {
                imgId = itemId + this.rangNum;
                if (imgId > maxFruitNum)
                    imgId -= maxFruitNum;
                if (goldLv > 0 && (Math.Abs(itemId - this.rangNum + 1) <= 1 || (this.rangNum + 1 == this.maxFruitNum && itemId == 1) ||
                    (this.rangNum + 1 == 1 && itemId == this.maxFruitNum)))
                {
                    itemId = this.rangNum;
                }
                else
                {
                    img.sprite = this.spriteList[imgId - 1];
                }
            }
            else
            {
                img.gameObject.SetActive(false);
            }
            img.SetNativeSize();
            mbItemComponent.SetMBItemInfo(row, num, configId, level, itemId, item.transform, dicId);
            if (itemId == this.rangNum)
            {
                mbItemComponent.GoldRotate(true);
            }
            btn.onClick.AddListener(()=>
            {
                OnClickbtnItem(mbItemComponent);
            });
            btnBg.onClick.AddListener(()=>
            {
                OnClickbtnItem(mbItemComponent);
            });
            if(!this.itemList.ContainsKey(itemId))
                this.itemList.Add(itemId, mbItemComponent);

//            item.AddComponent<MBGridItem>().itemId = dicId;
            if (this.self.allGridDic.ContainsKey(dicId))
            {
                Log.Error("有相同Id：" +configId + " row:" + row + " no:" + num + " lv:" + level);
                //return;
            }
            this.self.allGridDic.Add(dicId, mbItemComponent);
            
            if (itemId > 1000)
            {
                GameObject imgLock = GameObject.Instantiate(item.transform.Find("grey").gameObject, item.transform);
                imgLock.name = itemId.ToString();
                imgLock.SetActive(true);
                imgLock.transform.SetSiblingIndex(2);
                //imgLock.GetComponent<Image>().raycastTarget = false;
                Image img0 = imgLock.GetComponent<Image>();
                if (itemId == MBItemType.ItemLock)
                {
                    Image imgLock0 = imgLock.GetComponent<Image>();
                    imgLock0.sprite = this.self.ItemLock.sprite;
                    imgLock0.SetNativeSize();
                }
                else if (itemId == MBItemType.ItemKey)
                {
                    img0.sprite = this.self.ItemKey.sprite;
                }
                else if (itemId == MBItemType.ItemIce)
                {
                    img0.sprite = this.self.ItemIce.sprite;
                }
                img0.raycastTarget = false;
            }

            if (mbItemComponent.itemEffectNum >= MBItemType.BlindBox && mbItemComponent.itemEffectNum <= MBItemType.max)
            {
                GameObject obj = GameObject.Instantiate(mbItemComponent.obj.Find("grey").gameObject, mbItemComponent.obj);
                obj.name = mbItemComponent.itemEffectNum.ToString();
                obj.SetActive(true);
                obj.transform.SetSiblingIndex(2);
                if (mbItemComponent.itemEffectNum == MBItemType.BlindBox)
                {
                    obj.GetComponent<Image>().raycastTarget = false;
                    obj.GetComponent<Image>().sprite = this.self.imgBlindBox.sprite;
                }
                else if (mbItemComponent.itemEffectNum == MBItemType.ItemIce)
                {
                    obj.GetComponent<Image>().sprite = this.self.ItemIce.sprite;
                }
            }

            //Log.Error("有相同Id：" +configId + " row:" + row + " no:" + num + " lv:" + level);
            int effectId = MBDataComponent.Instance.IsItemFruit(mbItemComponent.itemId);
            // if (MBDataComponent.Instance.curPlayLevel == MBDataComponent.Instance.BombLv)
            // {
            //     if (mbItemComponent.itemId == 11)
            //         effectId = MBItemType.MBIce;
            //     else if (level == 2 || level == 1)
            //         effectId = 0;
            // }

            // if (effectId != 0)
            // {
            //     // if (level > this.maxLayer - 5)
            //     //     return mbItemComponent;
            //     GameObject obj = GameObject.Instantiate(item.transform.Find("grey").gameObject, item.transform);
            //     obj.name = effectId.ToString();
            //     obj.SetActive(true);
            //     obj.transform.SetSiblingIndex(2);
            //     if (effectId == MBItemType.MBIce)
            //     {
            //         //obj.GetComponent<Image>().raycastTarget = false;
            //         obj.GetComponent<Image>().sprite = this.self.imgIce.sprite;
            //     }
            //     else if(effectId == MBItemType.Lock)
            //     {
            //         //obj.GetComponent<Image>().raycastTarget = false;
            //         obj.GetComponent<Image>().sprite = this.self.imgLock.sprite;
            //     }
            //     else
            //     {
            //         obj.GetComponent<Image>().raycastTarget = false;
            //         obj.GetComponent<Image>().sprite = this.self.imgBlindBox.sprite;
            //     }
            //     mbItemComponent.itemEffectNum = effectId;
            // }

            // if (mBItemType == -100)
            // {
            //     int itemEffectNum = ETCommonFunc.IsItemFruit(mbItemComponent.itemId);
            //     CreateSpecialItem(mbItemComponent, itemEffectNum, img);
            // }
            // else
            // {
            //     CreateSpecialItem(mbItemComponent, mBItemType, img, isIce);
            // }
            return mbItemComponent;
        }

        List<UIMBItemComponent> SortAllItem(List<UIMBItemComponent> overList, bool isFindOverList = false)
        {
            if (overList == null)
            {
                overList = new List<UIMBItemComponent>();
                //Dictionary<int, List<UIMBItemComponent>> lowList = new Dictionary<int, List<UIMBItemComponent>>();
                foreach (var item in this.self.allGridDic)
                {
                    var mbItem = item.Value;
                    if (mbItem.overGrid.Count == 0)
                    {
                        overList.Add(mbItem);
                        if (mbItem.itemId == MBItemType.ItemLock)
                        {
                            if(!this.keyLockDic.ContainsKey(MBItemType.ItemLock))
                                keyLockDic.Add(MBItemType.ItemLock, new List<UIMBItemComponent>());
                            keyLockDic[MBItemType.ItemLock].Add(mbItem);
                        }
                    }
                    else
                    {
                        // int effectId = MBDataComponent.Instance.IsItemFruit(mbItem.itemId);;
                        // if (MBDataComponent.Instance.level == 4 || MBDataComponent.Instance.level == 6 ||  MBDataComponent.Instance.level == 10 )
                        // {
                        //     effectId = MBItemType.BlindBox;
                        // }
                        // if (mbItem.itemId > 1000 )
                        // {
                        //     effectId = 0;
                        // }
                        // mbItem.itemEffectNum = effectId;
                        // if (effectId == MBItemType.BlindBox)
                        // {
                        //     GameObject obj = GameObject.Instantiate(mbItem.obj.Find("grey").gameObject, mbItem.obj);
                        //     obj.name = effectId.ToString();
                        //     obj.SetActive(true);
                        //     obj.transform.SetSiblingIndex(2);
                        //     obj.GetComponent<Image>().raycastTarget = false;
                        //     obj.GetComponent<Image>().sprite = this.self.imgBlindBox.sprite;
                        // }
                    }
                }
            }
            if (!isFindOverList)
            {
                for (int i = 0; i < overList.Count; i++)
                {
                    overList[i].obj.SetParent(this.self.overParent.Find(overList[i].num.ToString()));
                }
            }
            return overList;
        }

        public void AddListner()
        {
            self.btnReturn.onClick.AddListener(() =>
            {
                SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
                EventDispatcher.PostEvent(EventName.MBLevelUpdate, this);
                if (GlobalComponent.Instance.mbDic != null)
                {
                    UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIMB).Coroutine();      
                    return;
                }

                TipInfoComponent data = Game.Scene.GetComponent<TipInfoComponent>();
                if (data == null)
                {
                    data = Game.Scene.AddComponent<TipInfoComponent>();
                }

                data.isNet = false;
                data.text = "Are you a quiter?";
                data.titleStr = "Leave Game";
                data.sureCallback = this.OnClickReturn;
                data.cancelCallback = null;
                UIHelper.Create(this.self.ZoneScene(), UIType.UIPopQuit, UILayer.Mid).Coroutine();
                //UIHelper.ShowPop(this.self.ZoneScene(), "Are you a quiter?", this.OnClickReturn, false, null, "Leave Game");
            });
            this.self.btnReLast.onClick.AddListener(()=>
            {
                if(!this.isCanClick) return;
                ResetBottomAni();
//                this.self.lastGridRed.SetActive(false);
                if (AppInfoComponent.Instance.guiding && AppInfoComponent.Instance.guideStep == (int) GuideStep.RelastUse && MBDataComponent.Instance.curPlayLevel == KeyDefine.guideLevel)
                {
                    this.self.free.SetActive(true);
                    Transform free = this.self.free.transform.Find("free1");
                    free.gameObject.SetActive(true);
                }
                EventDispatcher.PostEvent(EventName.GuideClick, this);
                    OnReLastClick();
            });
            this.self.btnAddGrid.onClick.AddListener(()=>
            {
                if(!this.isCanClick) return;
                ResetBottomAni();
//                this.self.lastGridRed.SetActive(false);
                if (AppInfoComponent.Instance.guiding && AppInfoComponent.Instance.guideStep == (int) GuideStep.AddGridUse && MBDataComponent.Instance.curPlayLevel == KeyDefine.guideLevel)
                {
                    this.self.free.SetActive(true);
                    Transform free = this.self.free.transform.Find("free2");
                    free.gameObject.SetActive(true);
                }
                EventDispatcher.PostEvent(EventName.GuideClick, this);
                OnAddGridClick();
            });
            this.self.btnUpdate.onClick.AddListener(()=>
            {
                if(!this.isCanClick) return;
                ResetBottomAni();
                //this.self.greyUpdate.SetActive(true);
                if (AppInfoComponent.Instance.guiding && AppInfoComponent.Instance.guideStep == (int) GuideStep.UpdateUse && MBDataComponent.Instance.curPlayLevel == KeyDefine.guideLevel)
                {
                    this.self.free.SetActive(true);
                    Transform free = this.self.free.transform.Find("free3");
                    free.gameObject.SetActive(true);
                }
                EventDispatcher.PostEvent(EventName.GuideClick, this);
                    this.ReSortItem();
            });
            // this.self.btnReLastAd.onClick.AddListener(() =>
            // {
            //     HallHelper.SendBILogByClient(LogEvent.adAdd, KeyDefine.reLastId, MBDataComponent.Instance.curPlayLevel);
            //     int adItemNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.game_ad_undo);
            //     TipInfoComponent.Instance.InitTipInfo(KeyDefine.reLastId, adItemNum, ItemUseNumDic[KeyDefine.reLastId]);
            //     //UIHelper.ShowPop(GlobalComponent.Instance.scene, "View ad to get "+adItemNum+" 'Undo' Power-Ups", () =>
            //     UIHelper.ShowPop(GlobalComponent.Instance.scene, LanguageComponent.Instance.GetLanguage(2172), () =>
            //     {
            //         this.PlayAd(GoogleAdsPath.MBGameRelast);
            //     }, false, () =>
            //     {
            //         HallHelper.GetGoodList(GlobalComponent.Instance.scene, () =>
            //         {
            //             ShopHelper.GetInstance().openType = ShopType.MbShop;
            //         });
            //     });
            // });
            // this.self.btnAddGridAd.onClick.AddListener(() =>
            // {
            //     BtnAddGridAdClick();
            // });
            // this.self.btnUpdateAd.onClick.AddListener(() =>
            // {
            //     HallHelper.SendBILogByClient(LogEvent.adAdd, KeyDefine.reSortId, MBDataComponent.Instance.curPlayLevel);
            //     int adItemNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.game_ad_shuffle);
            //     TipInfoComponent.Instance.InitTipInfo(KeyDefine.reSortId, adItemNum, ItemUseNumDic[KeyDefine.reSortId]);
            //     //UIHelper.ShowPop(GlobalComponent.Instance.scene, "View ad to get "+adItemNum+" 'Shuffle' Power-Ups", () =>
            //     UIHelper.ShowPop(GlobalComponent.Instance.scene, LanguageComponent.Instance.GetLanguage(2172), () =>
            //     {
            //         PlayAd(GoogleAdsPath.MBGameResort);
            //     }, false, () =>
            //     {
            //         HallHelper.GetGoodList(GlobalComponent.Instance.scene, () =>
            //         {
            //             ShopHelper.GetInstance().openType = ShopType.MbShop;
            //         });
            //     });
            // });
            //
            // this.self.btnShop.onClick.AddListener(() =>
            // {
            //     SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            //     HallHelper.GetGoodList(GlobalComponent.Instance.scene, () =>
            //     {
            //         ShopHelper.GetInstance().openType = ShopType.MbShop;
            //     });
            // });
            // this.self.btnLucky.onClick.AddListener(() =>
            // {
            //     SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            //     UIHelper.Create(this.self.ZoneScene(), UIType.UIBigTurnTable, UILayer.Mid).Coroutine();
            // });
            // this.self.btnQuest.onClick.AddListener(() =>
            // {
            //     if (QuestHelper.GetInstance().taskList == null || QuestHelper.GetInstance().taskList.Count == 0 
            //         || HallHelper.gateSession == null || HallHelper.gateSession.IsDisposed)
            //     {
            //         EventDispatcher.PostEvent(EventName.ConnectMySerInGame, null);
            //         UIHelper.ShowTip(GlobalComponent.Instance.scene, "No internet connection, you are in offline mode");
            //         return;
            //     }
            //     SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            //     GameDataMgr.Instance.isClickBtn = true;
            //     UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIQuest, UILayer.Mid).Coroutine();
            // });
            EventDispatcher.AddObserver(this, EventName.MBAgain, (object[] paras) =>
            {
                maxNum = UIMBDataHelper.BaseGridNum;
                order = "";
                if (MBDataComponent.Instance.level > 62)
                    MBDataComponent.Instance.curPlayLevel = MBDataComponent.Instance.level % 28 + 30; 
                keyLockDic.Clear();
                ReSetItemNum();
                rangNum = Random.Range(0, maxFruitNum);
                MBDataComponent.Instance.SetFreeNum(MBDataComponent.Instance.curPlayLevel);
                //InitItemNumText();
                this.isEnd = false;
                foreach (var VARIABLE in this.self.allGridDic)
                {
                    if(VARIABLE.Value.obj != null)
                        GameObject.Destroy(VARIABLE.Value.obj.gameObject);
                }
                foreach (var VARIABLE in this.self.downList)
                {
                    if(VARIABLE.obj != null)
                        GameObject.Destroy(VARIABLE.obj.gameObject);
                }
                foreach (var VARIABLE in this.self.reviveList)
                {
                    if(VARIABLE.Value != null && VARIABLE.Value.obj != null)
                        GameObject.Destroy(VARIABLE.Value.obj.gameObject);
                }
                foreach (var VARIABLE in this.self.addList)
                {
                    if(VARIABLE != null && VARIABLE.obj != null)
                        GameObject.Destroy(VARIABLE.obj.gameObject);
                }
                
                this.self.allGridDic.Clear();
                this.self.allOneGridDic.Clear();
                this.self.downList.Clear();
                lastMoveGrid = null;
                
                for (int i = 0; i < this.lastDestroyList.Count; i++)
                {
                    if(this.lastDestroyList[i] != null && this.lastDestroyList[i].obj != null)
                        GameObject.Destroy(this.lastDestroyList[i].obj.gameObject);
                }
                this.lastDestroyList.Clear();
                this.asideDic.Clear();
                SortItem().Coroutine();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.NetWaitLongSingleResult, (object[] paras) =>
            {
                int rank = (int) paras[0];
                //ResultSingle(rank);
                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, EventName.RechargeItem, (object[] paras) =>
            {
                int itemId = (int) paras[0];
                this.InitItemNumText();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.UpdateItemInMB, (object[] paras) =>
            {
                if (self == null || this.self.gameSelf == null)
                    return false;
                this.InitItemNumText();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, ETEventName.ShowItemUnlock, (object[] paras) =>
            {
                // int showItemBtn = (int) paras[0];
                // if(showItemBtn == 1)    this.self.greyRelast.SetActive(false);
                // if(showItemBtn == 2)    this.self.greyAddGrid.SetActive(false);
                // if(showItemBtn == 3)    this.self.greyUpdate.SetActive(false);
                return false;
            }, null);
            EventDispatcher.AddObserver(this, ETEventName.CloseUIMB, (object[] paras) =>
            {
                this.OnClickReturn();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, "ClickFruit", (object[] paras) =>
            {
                int unitId = (int) paras[0];
                if (!self.allGridDic.ContainsKey(unitId))
                {
                    Log.Error("Click Fruit do not find, fruit id is :" + unitId);
                    return false;
                }
                UIMBItemComponent go = this.self.allGridDic[unitId];
                OnClickbtnItem(go);
                return false;
            }, null);
            EventDispatcher.AddObserver(this, ETEventName.SecondTipYes, (object[] paras) =>
            {
                Text txtNum = GameObjectMgr.Refer(this.self.goCoin, "txt_num").GetComponent<Text>();
                txtNum.text = UIMBDataHelper.Instance.MoneyValue.ToString();
                this.maxNum = UIMBDataHelper.BaseGridNum + UIMBDataHelper.Instance.UnLockGrids().Count;
                RefreshLockGrid(true);
                return false;
            }, null);
        }

        private void BtnAddGridAdClick()
        {
            HallHelper.SendBILogByClient(LogEvent.adAdd, KeyDefine.addGridId, MBDataComponent.Instance.curPlayLevel);
            UIHelper.ShowPop(GlobalComponent.Instance.scene, LanguageComponent.Instance.GetLanguage(2172), () =>
            {
                //PlayAd(GoogleAdsPath.MBGameAddGrid);
            }, false, () =>
            {
                HallHelper.GetGoodList(GlobalComponent.Instance.scene, () =>
                {
                    ShopHelper.GetInstance().openType = ShopType.MbShop;
                });
            });
        }

        void ReSetItemNum()
        {
            HallInfoComponent.Instance.ChgItemNum(KeyDefine.reLastId, 1);
            HallInfoComponent.Instance.ChgItemNum(KeyDefine.reSortId, 1);
            HallInfoComponent.Instance.ChgItemNum(KeyDefine.addGridId, 1);
        }

        private void InitItemNumText()
        {
            if (HallInfoComponent.Instance.GetItemNum(KeyDefine.reLastId) >= 1)
            {
                // this.self.btnReLastAd.transform.DOKill();
                // await TimerComponent.Instance.WaitAsync(100);
                // this.self.btnReLastAd.transform.localScale = Vector3.one * 0.7f;
                this.self.textRelast.text = HallInfoComponent.Instance.GetItemNum(KeyDefine.reLastId).ToString();
            }
            else
            {
                // if(this.self.btnReLastAd.transform)
                //     this.self.btnReLastAd.transform.DOKill();
                // await TimerComponent.Instance.WaitAsync(100);
                // this.self.btnReLastAd.transform.localScale = Vector3.one * 0.7f;
                // this.self.btnReLastAd.transform.DOScale(0.9f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                this.self.textRelast.text = "0";
            }

            int addGridItemNum = HallInfoComponent.Instance.GetItemNum(KeyDefine.addGridId);
            this.self.textAddgrid.text = addGridItemNum.ToString();
            // if (addGridItemNum >= 1)
            // {
            //     this.self.btnAddGridAd.transform.DOKill();
            //     await TimerComponent.Instance.WaitAsync(100);
            //     this.self.btnAddGridAd.transform.localScale = Vector3.one * 0.7f;
            // }
            // else
            // {
            //     this.self.btnAddGridAd.transform.DOKill();
            //     await TimerComponent.Instance.WaitAsync(100);
            //     this.self.btnAddGridAd.transform.localScale = Vector3.one * 0.7f;
            //     this.self.btnAddGridAd.transform.DOScale(0.9f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            // }

            int itemNum3 = HallInfoComponent.Instance.GetItemNum(KeyDefine.reSortId);
            if (itemNum3 < 1)
            {
                this.self.textUpdate.text = "0";
                // this.self.btnUpdateAd.transform.DOKill();
                // await TimerComponent.Instance.WaitAsync(100);
                // this.self.btnUpdateAd.transform.localScale = Vector3.one * 0.7f;
                // this.self.btnUpdateAd.transform.DOScale(0.9f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                // this.self.btnUpdateAd.transform.DOKill();
                // await TimerComponent.Instance.WaitAsync(100);
                // this.self.btnUpdateAd.transform.localScale = Vector3.one * 0.7f;
                this.self.textUpdate.text = HallInfoComponent.Instance.GetItemNum(KeyDefine.reSortId).ToString(); //"1";
            }
            SetItemBtnState();
        }

        private async void OnAddGridClick()
        {
            if (this.self.downList.Count == 0)
            {
                SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
                UIHelper.ShowTip(this.self.ZoneScene(), LanguageComponent.Instance.GetLanguage(2171));
                return;
            }

            if (HallInfoComponent.Instance.GetItemNum(KeyDefine.addGridId) <= 0 )//&& MBDataComponent.Instance.curPlayLevel != 2 && MBDataComponent.Instance.freeNumList[2] <= 0)
            {
                SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
                //UIHelper.ShowTip(this.self.ZoneScene(), "Oops..You've used them all");
                // MBDataComponent.Instance.itemBuyId = KeyDefine.addGridId;
                // //UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIShopInGame, UILayer.Mid).Coroutine();
                //
                // int adItemNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.game_ad_move);
                // TipInfoComponent.Instance.InitTipInfo(KeyDefine.addGridId, adItemNum, ItemUseNumDic[KeyDefine.addGridId]);
                // //UIHelper.ShowPop(GlobalComponent.Instance.scene, "View ad to get "+adItemNum+" 'Move' Power-Ups", () =>
                // UIHelper.ShowPop(GlobalComponent.Instance.scene, LanguageComponent.Instance.GetLanguage(2172), () =>
                // {
                //     PlayAd(GoogleAdsPath.MBGameAddGrid);
                // }, false, () =>
                // {
                //     HallHelper.GetGoodList(GlobalComponent.Instance.scene, () =>
                //     {
                //         ShopHelper.GetInstance().openType = ShopType.MbShop;
                //     });
                // });
                return;
            }

            SoundComponent.Instance.PlayActionSound("Music","MBMove");
            int num = 0;
            for (int i = 0; i < 3; i++)
            {
                if (this.asideDic.ContainsKey(i) && this.asideDic[i] != null)
                {
                    num++;
                }
            }
            if (num == 1)
            {
                //this.self.addGridTips.SetActive(true);
                UIHelper.ShowTip(this.self.ZoneScene(), "Extra slot full");
                return;
                // var obj = self.downList[0].obj;                
                // float tweenTime = 0.5f;
                //
                // Sequence tw = DOTween.Sequence();
                // tw.Append(obj.DOMoveX(self.itemGridPosList[2].position.x + 0.9f, tweenTime)).SetEase(Ease.Linear);
                // tw.Insert(0, obj.DOMoveY(self.itemGridPosList[2].position.y + 0.6f, tweenTime / 2).SetEase(Ease.OutCirc));
                // tw.Insert(tweenTime / 2, obj.DOMoveY(self.itemGridPosList[2].position.y, tweenTime / 2).SetEase(Ease.OutCirc));
                // //var tweenPos =self.downList[0].obj.DOLocalMove(self.itemGridPosList[2] + new Vector3(50, 0, 0), 0.5f);
                // tw.onComplete = () =>
                // {
                //     Sequence tw2 = DOTween.Sequence();
                //     tw2.Append(obj.DOMoveX(self.downPosList[0].position.x, tweenTime)).SetEase(Ease.Linear);
                //     tw2.Insert(0, obj.DOMoveY(self.downPosList[0].position.y + 0.6f, tweenTime / 2).SetEase(Ease.OutCirc));
                //     tw2.Insert(tweenTime / 2, obj.DOMoveY(self.downPosList[0].position.y, tweenTime / 2).SetEase(Ease.OutCirc));
                //     tw2.Play();
                // };
                // tw.Play();
                // var tweenIn = this.asideDic[0].obj.DOMoveX(self.itemGridPosList[0].position.x - 0.3f, tweenTime);
                // this.asideDic[1].obj.DOMoveX(self.itemGridPosList[1].position.x - 0.3f, tweenTime);
                // this.asideDic[2].obj.DOMoveX(self.itemGridPosList[2].position.x - 0.3f, tweenTime);
                // tweenIn.onComplete = () =>
                // {
                //     this.asideDic[0].obj.DOMoveX(self.itemGridPosList[0].position.x, tweenTime);
                //     this.asideDic[1].obj.DOMoveX(self.itemGridPosList[1].position.x, tweenTime);
                //     this.asideDic[2].obj.DOMoveX(self.itemGridPosList[2].position.x, tweenTime);
                // };
                //
                // var tweenColor = this.self.extraGridBg.DOColor(Color.red, tweenTime);
                // tweenColor.onComplete = () =>
                // {
                //     self.extraGridBg.DOColor(Color.white, tweenTime);
                // };
                //return;
            }

            if (MBDataComponent.Instance.curPlayLevel != 2)
            {
                if (MBDataComponent.Instance.GetFreeItemNum(KeyDefine.addGridId) > 0)
                {
                    MBDataComponent.Instance.ChgFreeItemNum(KeyDefine.addGridId);
                    HallHelper.UseItem(this.self.ZoneScene(), KeyDefine.addGridId, () => { }, 1, MBDataComponent.Instance.curPlayLevel, false, 1);
                }
                else
                {
                    HallInfoComponent.Instance.ChgItemNum(KeyDefine.addGridId, 0, -1);
                    HallHelper.UseItem(this.self.ZoneScene(), KeyDefine.addGridId, () => { }, 1, MBDataComponent.Instance.curPlayLevel);
                }
            }
            else
            {
                HallHelper.UseItem(this.self.ZoneScene(), KeyDefine.addGridId, () => { }, 1, MBDataComponent.Instance.curPlayLevel, false, 1);
            }

            this.InitItemNumText();
            //this.self.textAddgrid.text = HallInfoComponent.Instance.GetItemNum(KeyDefine.addGridId).ToString();
            Log.Console("addGridNum:" + self.textAddgrid.text);
            for (int k = 0; k < 3; k++)
            {
                if (!this.asideDic.ContainsKey(k) || this.asideDic[k] == null)
                {
                    this.AddGridItemClick(k);
                    break;
                }
            }

            // this.AddGridItemClick(3 - addGridItemNum);
            // addGridItemNum--;
            HallInfoComponent.Instance.ChgItemNum(KeyDefine.addGridId, 0);
            this.self.textAddgrid.text = "0";
            await ETTask.CompletedTask;
        }

        private void OnReLastClick()
        {
            if (this.self.textRelast.text == "0")// &&  MBDataComponent.Instance.freeNumList[0] <= 0)
            {
                // SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
                // //UIHelper.ShowTip(this.self.ZoneScene(), "Oops..You've used them all");
                // MBDataComponent.Instance.itemBuyId = KeyDefine.reLastId;
                // //UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIShopInGame, UILayer.Mid).Coroutine();
                // int adItemNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.game_ad_undo);
                // TipInfoComponent.Instance.InitTipInfo(KeyDefine.reLastId, adItemNum, ItemUseNumDic[KeyDefine.reLastId]);
                // //UIHelper.ShowPop(GlobalComponent.Instance.scene, "View ad to get "+adItemNum+" 'Undo' Power-Ups", () =>
                // UIHelper.ShowPop(GlobalComponent.Instance.scene, LanguageComponent.Instance.GetLanguage(2172), () =>
                // {
                //     PlayAd(GoogleAdsPath.MBGameRelast);
                // }, false, () =>
                // {
                //     HallHelper.GetGoodList(GlobalComponent.Instance.scene, () =>
                //     {
                //         ShopHelper.GetInstance().openType = ShopType.MbShop;
                //     });
                // });
                return;
            }
            if (this.lastMoveGrid == null)
            {
                SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
                UIHelper.ShowTip(this.self.ZoneScene(), LanguageComponent.Instance.GetLanguage(2170));
                return;
            }
            if (this.lastInsertIndex >= self.downList.Count)
            {
                SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
                UIHelper.ShowTip(this.self.ZoneScene(), LanguageComponent.Instance.GetLanguage(2177));
                return;
            }

            if (MBDataComponent.Instance.curPlayLevel != 2)
            {
                if (MBDataComponent.Instance.GetFreeItemNum(KeyDefine.reLastId) > 0)
                {
                    MBDataComponent.Instance.ChgFreeItemNum(KeyDefine.reLastId);                    
                    HallHelper.UseItem(this.self.ZoneScene(), KeyDefine.reLastId, () => { }, 1, MBDataComponent.Instance.curPlayLevel, false, 1);

                }
                else
                {
                    HallInfoComponent.Instance.ChgItemNum(KeyDefine.reLastId, 0, -1);
                    HallHelper.UseItem(this.self.ZoneScene(), KeyDefine.reLastId, () =>
                    { 
                    }, 1, MBDataComponent.Instance.curPlayLevel);
                }
            }
            else
            {
                HallHelper.UseItem(this.self.ZoneScene(), KeyDefine.reLastId, () => { }, 1, MBDataComponent.Instance.curPlayLevel, false, 1);
            }

            SoundComponent.Instance.PlayActionSound("Music","MBRelast");

            this.InitItemNumText();
            //this.self.textRelast.text = (HallInfoComponent.Instance.GetItemNum(KeyDefine.reLastId)).ToString();
            HallInfoComponent.Instance.ChgItemNum(KeyDefine.reLastId, 0);
            this.self.textRelast.text = "0";
            
            this.SetUnlockGridAgain(lastMoveGrid);
            this.lastMoveGrid = null;
            EventDispatcher.PostEvent(EventName.AppsFlyEvent, null, AppsEventName.Puzzle_Use_Undo);
            EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Puzzle_Use_Undo);
        }
        
        private async void ReSortItem()
        {
            if (this.self.textUpdate.text == "0"  && MBDataComponent.Instance.freeNumList[1] <= 0)
            {
                // SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
                // //UIHelper.ShowTip(this.self.ZoneScene(), "Oops..You've used them all");
                // MBDataComponent.Instance.itemBuyId = KeyDefine.reSortId;
                // //UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIShopInGame, UILayer.Mid).Coroutine();
                //
                // int adItemNum = (int)FireBComponent.Instance.GetRemoteLong(FireBRemoteName.game_ad_shuffle);
                // TipInfoComponent.Instance.InitTipInfo(KeyDefine.reSortId, adItemNum, ItemUseNumDic[KeyDefine.reSortId]);
                // //UIHelper.ShowPop(GlobalComponent.Instance.scene, "View ad to get "+adItemNum+" 'Shuffle' Power-Ups", () =>
                // UIHelper.ShowPop(GlobalComponent.Instance.scene, LanguageComponent.Instance.GetLanguage(2172), () =>
                // {
                //     PlayAd(GoogleAdsPath.MBGameResort);
                // }, false, () =>
                // {
                //     HallHelper.GetGoodList(GlobalComponent.Instance.scene, () =>
                //     {
                //         ShopHelper.GetInstance().openType = ShopType.MbShop;
                //     });
                // });
                // this.self.greyUpdate.SetActive(false);
                return;
            }
            SoundComponent.Instance.PlayActionSound("Music2","resort");
            if (MBDataComponent.Instance.curPlayLevel != KeyDefine.guideLevel)
            {
                if (MBDataComponent.Instance.GetFreeItemNum(KeyDefine.reSortId) > 0)
                {
                    MBDataComponent.Instance.ChgFreeItemNum(KeyDefine.reSortId);                    
                    HallHelper.UseItem(this.self.ZoneScene(), KeyDefine.reSortId, () => { }, 1, MBDataComponent.Instance.curPlayLevel, false, 1);

                }
                else
                {
                    HallInfoComponent.Instance.ChgItemNum(KeyDefine.reSortId, 0, -1);
                    HallHelper.UseItem(this.self.ZoneScene(), KeyDefine.reSortId, () => { }, 1, MBDataComponent.Instance.curPlayLevel);
                }
            }
            else
            {                    
                HallHelper.UseItem(this.self.ZoneScene(), KeyDefine.reSortId, () => { }, 1, MBDataComponent.Instance.curPlayLevel, false, 1);
            }

            this.InitItemNumText();
            await TimerComponent.Instance.WaitAsync(200);
            int itemNum = self.allGridDic.Count;
            int chgRandNum = Random.Range(1, itemNum / 2 - 1);
            //chgRandNum = itemNum / 2 - 1;
            Log.Console("打乱系数：" + chgRandNum.ToString());
            List<UIMBItemComponent> list = self.allGridDic.Values.ToList();
            for (int i = 0; i < itemNum / 2 - 1; i++)
            {
                int chgIndex = itemNum / 2 + chgRandNum + i;
                if (chgIndex >= itemNum)
                    chgIndex -= itemNum / 2;

                //Log.Console("list[i].itemEffectNum:" + list[i].itemEffectNum);
                if (list[i].itemId != MBItemType.ItemLock && list[i].itemId != MBItemType.ItemKey &&
                    list[chgIndex].itemId != MBItemType.ItemLock && list[chgIndex].itemId != MBItemType.ItemKey)
                {
                    int effectId = list[i].itemEffectNum;
                    list[i].itemEffectNum = list[chgIndex].itemEffectNum;
                    list[chgIndex].itemEffectNum = effectId;

                    Transform effectImg1 = list[i].obj.GetChild(2);
                    Transform effectImg2 = list[chgIndex].obj.GetChild(2);
                    if (effectImg1.name != "grey")
                    {
                        effectImg1.parent = list[chgIndex].obj;
                        effectImg1.localPosition = Vector3.zero;
                        CommonFuc.SetAsLast(list[chgIndex].obj.Find("grey"));
                        //list[chgIndex].obj.Find("grey").SetAsLastSibling();
                        //ETCommonFunc.Instance.DelayAction(200, () => { effectImg1.SetSiblingIndex(2);});
                    }
                    if (effectImg2.name != "grey")
                    {
                        effectImg2.parent = list[i].obj;
                        effectImg2.localPosition = Vector3.zero;
                        CommonFuc.SetAsLast(list[i].obj.Find("grey"));
                        //list[i].obj.Find("grey").SetAsLastSibling();
                        //ETCommonFunc.Instance.DelayAction(200, () => { effectImg1.SetSiblingIndex(2);});
                    }

                    if (list[i].overGrid.Count == 0 && list[i].itemEffectNum == MBItemType.BlindBox)
                    {
                        list[i].obj.Find(MBItemType.BlindBox.ToString()).gameObject.SetActive(false);
                    }
                    if (list[chgIndex].overGrid.Count == 0 && list[chgIndex].itemEffectNum == MBItemType.BlindBox)
                    {
                        list[chgIndex].obj.Find(MBItemType.BlindBox.ToString()).gameObject.SetActive(false);
                    }

                    int itemId = list[i].itemId;
                    Image img = list[i].obj.Find("btnItem").GetComponent<Image>();
                    Image img2 = list[chgIndex].obj.Find("btnItem").GetComponent<Image>();
                    Sprite sp = img.sprite;
                    img.sprite = img2.sprite;
                    list[i].itemId = list[chgIndex].itemId;
                
                    img2.sprite = sp;
                    list[chgIndex].itemId = itemId;
                    if (list[i].IsGold())
                    {
                        list[i].GoldRotate(true);
                    }
                    else
                    {
                        list[i].GoldRotate(false);
                    }
                    if (list[chgIndex].IsGold())
                    {
                        list[chgIndex].GoldRotate(true);
                    }
                    else
                    {
                        list[chgIndex].GoldRotate(false);
                    }
                    CheckIceBreak(list[i]);
                    CheckIceBreak(list[chgIndex]);
                }
            }

            SortAni();

            EventDispatcher.PostEvent(EventName.AppsFlyEvent, null, AppsEventName.Puzzle_Use_PowerUp_Shuffle);
            EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Puzzle_Use_PowerUp_Shuffle);
            await TimerComponent.Instance.WaitAsync(900);
            //this.self.greyUpdate.SetActive(false);
        }

        private void CheckIceBreak(UIMBItemComponent go)
        {
            if(go.itemEffectNum != MBItemType.ItemIce)
                return;
            int iceDicId = CommonFuc.SetItemId(go.row, go.num, go.level);
            List<int> idIceList0 = CommonFuc.GetIceId(iceDicId);
            bool iceBreak = true;
            for (int m = 0; m < idIceList0.Count; m++)
            {
                int iceRoundId = idIceList0[m];
                if (this.self.allOneGridDic.ContainsKey(iceRoundId))
                {
                    var iceRoundList = this.self.allOneGridDic[iceRoundId];
                    if (iceRoundList.Count > 0)
                    {
                        iceBreak = false;
                    }

                }
            }
            if (iceBreak)
            {
                IceBreak(go);
            }
        }

        private async void SortAni()
        {
            self.mask.SetActive(true);
            List<UIMBItemComponent> list = self.allGridDic.Values.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Transform tran = list[i].obj;
                Vector3 pos = tran.position;
                tran.DOLocalMove(new Vector3(0, 0, 0), 0.5f).OnComplete(()=>
                {
                    tran.DOMove(pos, 0.5f);
                });
            }
            await TimerComponent.Instance.WaitAsync(1020);
            self.mask.SetActive(false);
            await ETTask.CompletedTask;
        }

        // private async void PlayAd(GoogleAdsPath googleAdsPath)
        // {
        //     HallHelper.ADLog(GlobalComponent.Instance.scene, 15, (int)googleAdsPath, MBDataComponent.Instance.curPlayLevel, (long)TimeInfo.Instance.ClientNow());
        //     EventDispatcher.PostEvent(EventName.NetWaitUI, null, true, 8);
        //     // if (GameDataMgr.Instance.Platflam() == PlatForm.Win)
        //     // {
        //     //     await TimerComponent.Instance.WaitAsync(500);
        //     //     Log.Console("领奖励");
        //     //     EventDispatcher.PostEvent(EventName.SendGoogleAdsReward, null, GoogleAdsEnum.Google_RewardAd, googleAdsPath);
        //     // }
        //     // else
        //     // {
        //     //     EventDispatcher.PostEvent(EventName.ShowGoolgeAds,null,GoogleAdsEnum.Google_RewardAd,googleAdsPath,FireBComponent.Instance.AdSelect(FireBRemoteName.ad_max_game_percent));
        //     // }
        //
        //     await ETTask.CompletedTask;
        // }

        private async void OnClickReturn()
        {
            EventDispatcher.PostEvent(ETEventName.VoodooEvent, null, MBMode.NormalFinish, 0, MBDataComponent.Instance.level);
            DestroyLastItem();
            EventDispatcher.PostEvent(EventName.AppsFlyEvent, null, AppsEventName.Puzzle_Use_Life);
            EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Puzzle_Use_Life);
            UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIMB).Coroutine();
            await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMBMap, (UILayer)ETUILayer.Low3);
        }

        private void ResetBottomAni()
        {
            if(this.self.downList.Count != 6)
                return;
            //self.bottomItem.transform.DOKill();
            //self.bottomItem.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            for (int i = 0; i < this.self.downList.Count; i++)
            {
                self.downList[i].obj.SetParent(self.bottomItem.transform.parent);
                
                
                // self.downList[i].obj.DOKill(true);
                // Vector3 destPos2 = GetMovePos(i);
                // self.downList[i].obj.DOMove(destPos2, 0.2f);
            }
        }
        

        private int maxNum = UIMBDataHelper.BaseGridNum;
        private void OnClickbtnItem(UIMBItemComponent go)
        {
            //this.Success(true);
            if (go.isDown)
            {
                return;
            }
            if (this.isEnd || !this.isCanClick)
            {
                return;
            }
            if (this.self.downList.Count >= this.maxNum && GameDataMgr.Instance.CanPassMB == 0)
            {
                return;
            }
            if(go.itemEffectNum == MBItemType.BlindBox)
                go.obj.Find(MBItemType.BlindBox.ToString()).gameObject.SetActive(false);
            if (go.itemId == MBItemType.ItemLock)
            {
                UIHelper.ShowTip(GlobalComponent.Instance.scene, "Find Keys To Unlock");
                return;
            }if (go.itemId == MBItemType.ItemKey && (!keyLockDic.ContainsKey(MBItemType.ItemLock) || keyLockDic[MBItemType.ItemLock].Count == 0))
            {
                UIHelper.ShowTip(GlobalComponent.Instance.scene, "Can Not Use Now");
                return;
            }
            EventDispatcher.PostEvent(EventName.GuideClick, this);
            VibrationHelper.GetInstance().SendVibration(1,1,2);
            if (keyLockDic.ContainsKey(MBItemType.ItemLock) && keyLockDic[MBItemType.ItemLock].Count > 0 && go.itemId == MBItemType.ItemKey)
            {
                isCanClick = false;
                UIMBItemComponent keyObj = go;
                UIMBItemComponent lockObj = keyLockDic[MBItemType.ItemLock][0];
                keyObj.obj.parent = keyObj.obj.parent.parent;
                //keyObj.obj.SetAsLastSibling();
                CommonFuc.SetAsLast(keyObj.obj);
                self.lockSpine.transform.position = lockObj.obj.position;
                keyObj.bgImg.gameObject.SetActive(false);
                keyObj.obj.DOMove(lockObj.obj.position, 0.6f).onComplete = () =>
                {
                    SoundComponent.Instance.PlayActionSound("Music4","unlock");
                    self.lockSpine.gameObject.SetActive(true);
                    self.lockSpine.AnimationState.SetAnimation(0, "open", false);
                };
                ETCommonFunc.Instance.DelayAction(1500, () =>
                {
                    OverGridUnLock(keyObj);
                    OverGridUnLock(lockObj);
                    DestroyGridSelf(keyObj);
                    DestroyGridSelf(lockObj);
                    GameObject.Destroy(keyObj.obj.gameObject);
                    GameObject.Destroy(lockObj.obj.gameObject);
                    this.lastDestroyList.Clear();
                    keyLockDic[MBItemType.ItemLock].RemoveAt(0);
                    isCanClick = true;
                }).Coroutine();
                // ETCommonFunc.Instance.DelayAction(1300, () =>
                // {
                //     //
                // });
                return;
            }
            float myprocess = 1 - (this.self.allGridDic.Count - 1) / (float) this.totalFruit;
            self.percent.enabled = true;
            this.self.percent.value = myprocess;
            self.percent.enabled = false;
            this.self.percentTxt.text = (int) (myprocess * 100) + "%";
            
            this.ResetBottomAni();
            OverGridUnLock(go);
            DestroyGridSelf(go);
            foreach (var VARIABLE in this.asideDic)
            {
                if (VARIABLE.Value != null &&  VARIABLE.Value.itemUnitId == go.itemUnitId)
                {
                    asideDic[VARIABLE.Key] = null;
                    lastUnLockList.Clear();
                    break;
                }
            }

            SoundComponent.Instance.PlayActionSound("Music4","MonekBClick");
            go.isDown = true;
            int itemGuide = AppInfoComponent.Instance.guideStep;
            DestroyLastItem();
            int itemId = go.itemId;
            int inserIndex = self.downList.Count;
            for (int i = 0; i < this.self.downList.Count; i++)
            {
                if (this.self.downList[i].itemId == itemId)
                {
                    inserIndex = i + 1;
                }
            }
            
            this.self.downList.Insert(inserIndex, go);
            lastInsertIndex = inserIndex;
            
            // int sameNum = 0;
            // for (int i = 0; i < this.self.downList.Count; i++)
            // {
            //     if (self.downList[i].CanMatch() && self.downList[i].itemId == go.itemId)
            //         sameNum++;
            // }
            // go.canKill = sameNum >= 3 ? true : false;
            int moveUpIndex = IsDestItemId(go.itemId);
            go.downnum = this.self.downList.Count;

            if (moveUpIndex > 0)
            {
                lastDestroyList.Add(go);
            }
            if (this.self.downList.Count == 6)
            {
                if (itemGuide == (int) GuideStep.AddGrid - 1)
                {
                    AppInfoComponent.Instance.guideStep = (int) GuideStep.AddGrid;
                    //Log.Console("再次进入引导 :" + itemGuide);
                    UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGuide, UILayer.High).Coroutine();
                    // DelayAction(3000, () =>
                    // {
                    //     this.self.greyAddGrid.SetActive(false);
                    // }).Coroutine();
                    this.isEnd = true;
                    ETCommonFunc.Instance.DelayAction(1000, () =>
                    {
                        this.isEnd = false;
                    }).Coroutine();
                }

  //              this.self.lastGridRed.SetActive(true);
                HideItemEffect();
                
                // for (int i = 0; i < this.self.downList.Count; i++)
                // {
                //     self.downList[i].obj.SetParent(self.bottomItem.transform);
                // }
                // self.bottomItem.transform.DOScale(1f, 1f).SetLoops(6, LoopType.Yoyo).OnComplete(() =>
                // {
                //     this.ResetBottomAni();
                // });
            }

            for (int k = 0; k < this.self.downList.Count; k++)
            {
                if (self.downList[k].itemId != itemId)
                {
                    Vector3 destPos2 = GetMovePos(k);
                    self.downList[k].obj.DOKill(true);
                    self.downList[k].obj.DOMove(destPos2, 0.1f);
                }
            }

            if (moveUpIndex > 0)
            {
                MoveToContainer(go, false, moveUpIndex);
                SortDownItem();
                ClickMatchItem(300, go.itemId).Coroutine();
            }
            else
            {
                // if (MBDataComponent.Instance.level == GuideLv.addGrid && AppInfoComponent.Instance.guideStep != (int) GuideStep.EndAddGrid 
                //     && self.downList.Count >= 1)//5
                // {
                //     this.self.greyAddGrid.SetActive(false);
                //     AppInfoComponent.Instance.guideStep = (int)GuideStep.AddGridUse;
                //     UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIGuide, UILayer.Mid2).Coroutine();
                // }//12
                var item = go;
                int upIndex = moveUpIndex;
                bool isWait = this.isWaitOrder;
                Vector3 destPos = GetMovePos(inserIndex);
                go.bgImg.gameObject.SetActive(false);
                var obj = go.obj;
                Action callback = () =>
                {
                    obj.DOShakeScale(0.5f, Vector3.one * 0.1f);
                    if (upIndex > 0)
                    {
                        MoveToContainer(item, false, upIndex);
                        SortDownItem();
                    }
                    else
                    {
                        int process = (int)(100 - this.self.allGridDic.Count / (float) this.totalFruit * 100);
                        if (go.downnum >= this.maxNum  && !this.isEnd && GameDataMgr.Instance.CanPassMB == 0)
                        {
                            if(!isWait)
                                this.Failed();
                            else
                            {
                                ETCommonFunc.Instance.DelayAction(1000, () =>
                                {
                                    if (this.self.downList.Count >= this.maxNum && !this.isEnd && !isWaitOrder && GameDataMgr.Instance.CanPassMB == 0)
                                    {
                                        this.Failed();
                                    }
                                }).Coroutine();
                            }
                        }
                    }
                };
                FlyFruitEffect(go.obj, destPos, callback).Coroutine();
            }

            //ETCommonFunc.Instance.DelayAction(1000, this.FindNextMatch);
            int reveiveNum = MBDataComponent.Instance.reveiveNum % 100;
            reveiveNum = 0;
            //Log.Console(MBDataComponent.Instance.reveiveNum + " reveive Num:" + reveiveNum);
            if (this.self.downList.Count >= this.maxNum && reveiveNum > 0 && GameDataMgr.Instance.CanPassMB == 0 && !this.isWaitOrder)
            {
                this.isEnd = true;

                ShowReveive();

                // UIHelper.ShowPop(GlobalComponent.Instance.scene, "WatAd to move 3 tile aside up", () =>
                // {
                //     PlayAd(GoogleAdsPath.MBRevive);
                //     //Revive();
                // }, false, () =>
                // {
                //     this.Failed();
                // });
            }
            this.Success();
        }
        
        void SortDownItem()
        {
            for (int k = 0; k < this.self.downList.Count; k++)
            {
                self.downList[k].obj.DOKill(true);
                Vector3 destPos2 = GetMovePos(k);
                self.downList[k].obj.DOMove(destPos2, 0.2f);
            }
        }

        private async void ShowReveive()
        {
            TipInfoComponent data = Game.Scene.GetComponent<TipInfoComponent>();
            if (data == null)
            {
                data = Game.Scene.AddComponent<TipInfoComponent>();
            }
            data.isNet = false;
            data.text = "Watch ad to continue?";
            data.titleStr = "Keep Playing?";
            data.sureCallback = () =>
            {
                MBDataComponent.Instance.reveiveNum--;
                HallHelper.SendBILogByClient(17, MBDataComponent.Instance.curPlayLevel);
                this.Reveive();
                // if(FireBComponent.Instance.GetRemoteLong(FireBRemoteName.coin_shop_reveive) == 1)
                //     EventDispatcher.PostEvent(EventName.ChangeItemNum, null, KeyDefine.GameCoin, 0, -1000);
                // else
                // {
                //     EventDispatcher.PostEvent(EventName.ChangeItemNum, null, KeyDefine.PlatFormCoin, 0, -1);
                // }
                UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIRevive).Coroutine();
            };
            data.cancelCallback = () =>
            {
                UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIRevive).Coroutine();
                Failed();
            };
                
            await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIRevive, UILayer.Mid);
            await TimerComponent.Instance.WaitAsync(100);
            if (UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIGuide) != null)
            {
                await UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIGuide);
            }
            await ETTask.CompletedTask;
        }

        private async ETTask DelayAction(int waitTime, Action callback)
        {
            await TimerComponent.Instance.WaitAsync(waitTime);
            callback();
            await ETTask.CompletedTask;
        }

        private void Reveive()
        {
            int moveNum = MBDataComponent.Instance.reveiveNum / 1000;
            int moveIndex = MBDataComponent.Instance.reveiveNum % 100;
            UIMBItemComponent go1 = this.self.downList[0];
            go1.isDown = false;
            this.self.downList.RemoveAt(0);
            //go1.obj.SetAsLastSibling();
            CommonFuc.SetAsLast(go1.obj);
            int dicId = CommonFuc.SetItemId(go1.row, go1.num, go1.level);
            if(!this.self.reviveList.ContainsKey(dicId))
                this.self.reviveList.Add(dicId, go1);
            if (moveNum == 1)
            {
                //go1.obj.DOMove(this.self.itemGridPosList[0].position + new Vector3(0, 130 + (3 -moveIndex) * 10, 0), 1f);
                go1.obj.DOMove(this.self.itemGridPosList[2 -moveIndex].position + new Vector3(0, 1.2f + (3 -3) * 0.1f, 0), 1f);
            }
            if (moveNum == 3)
            {
                go1.obj.DOMove(this.self.itemGridPosList[0].position + new Vector3(0, 1.2f + (MBDataComponent.Instance.alreadyReveiveNum) * 0.07f, 0), 1f);
                UIMBItemComponent go2 = this.self.downList[0];
                go2.isDown = false;
                this.self.downList.RemoveAt(0);
                
                UIMBItemComponent go3 = this.self.downList[0];
                go3.isDown = false;
                this.self.downList.RemoveAt(0);
                go2.obj.DOMove(this.self.itemGridPosList[1].position + new Vector3(0, 1.2f + (MBDataComponent.Instance.alreadyReveiveNum) * 0.07f, 0), 1f);
                go3.obj.DOMove(this.self.itemGridPosList[2].position + new Vector3(0, 1.2f + (MBDataComponent.Instance.alreadyReveiveNum) * 0.07f, 0), 1f);

                // go2.obj.SetAsLastSibling();
                // go3.obj.SetAsLastSibling();
                CommonFuc.SetAsLast(go2.obj);
                CommonFuc.SetAsLast(go3.obj);
                dicId = CommonFuc.SetItemId(go2.row, go2.num, go2.level);
                if(!this.self.reviveList.ContainsKey(dicId))
                    this.self.reviveList.Add(dicId, go2);
                dicId = CommonFuc.SetItemId(go3.row, go3.num, go3.level);
                if(!this.self.reviveList.ContainsKey(dicId))
                    this.self.reviveList.Add(dicId, go3);
            }
            for (int k = 0; k < this.self.downList.Count; k++)
            {
                Vector3 destPos2 = GetMovePos(k);
                self.downList[k].obj.DOMove(destPos2, 0.1f);
            }

            MBDataComponent.Instance.alreadyReveiveNum++;
            this.isEnd = false;
        }

        private void Failed()
        {
            int process = (int)(100 - this.self.allGridDic.Count / (float) this.totalFruit * 100);
                    //if (go.downnum == 7 && GameDataMgr.Instance.CanPassMB == false && !this.isEnd)
            //{
            this.isEnd = true;
  //          this.self.lastGridRed.SetActive(false);
            MBDataComponent.Instance.isWin = false;
            ResultSingle(2, process); 
            
            EventDispatcher.PostEvent(EventName.AppsFlyEvent, null, AppsEventName.Puzzle_Use_Life);
            EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Puzzle_Use_Life);
            
            string[] rankProcess = PlayerPrefs.GetString(ItemSaveStr.rankProcess, "0:0").Split(':');
            int lv = int.Parse(rankProcess[0]);
            int pro = int.Parse(rankProcess[1]);
            if (lv < MBDataComponent.Instance.level || (
                lv == MBDataComponent.Instance.level && pro < process))
            {
                PlayerPrefs.SetString(ItemSaveStr.rankProcess, MBDataComponent.Instance.level + ":" + process);
            }
            EventDispatcher.PostEvent(EventName.MBLevelUpdate, this);
            //}
        }

        private async void Success(bool success = false)
        {
            if (success || (this.self.allGridDic.Count == 0 || GameDataMgr.Instance.CanPassMB == 2 || GameDataMgr.Instance.CanPassMB == 3) && !this.isEnd)
            {
                await TimerComponent.Instance.WaitAsync(400);
                for (int i = 0; i < 3; i++)
                {
                    if (this.asideDic.ContainsKey(i) && this.asideDic[i] != null)
                    {
                        OnClickbtnItem(asideDic[i]);
                        break;
                    }
                }
                this.isEnd = true;
                MBDataComponent.Instance.isWin = true;
                //isMoving = false;
                ETCommonFunc.Instance.DelayAction(500, () =>
                {
                    DestroyLastItem();
                }).Coroutine();

                EventDispatcher.PostEvent(EventName.ChangeItemNum, null, KeyDefine.entityId, 0, 1);
                if(MBDataComponent.Instance.curPlayLevel == MBDataComponent.Instance.level)
                    MBDataComponent.Instance.failTime = 0;
                ResultSingle(1, 100);

                //if (MBDataComponent.Instance.curPlayLevel == MBDataComponent.Instance.level)
                {
                    MBDataComponent.Instance.level++;
                    string rankProcess = MBDataComponent.Instance.level + ":0";
                    PlayerPrefs.SetString(ItemSaveStr.rankProcess, rankProcess);
                }
                MBDataComponent.Instance.curPlayLevel++;
                EventDispatcher.PostEvent(EventName.MBLevelUpdate, this);
            }
        }

        private async void HideItemEffect()
        {
            await TimerComponent.Instance.WaitAsync(5000);
            if (self.btnUpdate != null)
            {
                this.self.btnUpdate.transform.Find("tipSp").gameObject.SetActive(false);
                this.self.btnAddGrid.transform.Find("tipSp").gameObject.SetActive(false);
                this.self.btnReLast.transform.Find("tipSp").gameObject.SetActive(false);
            }
            await ETTask.CompletedTask;
        }

        private bool DestroyLastItem()
        {
            if (this.lastDestroyList != null && this.lastDestroyList.Count > 0)
            {
                for (int i = 0; i < this.lastDestroyList.Count; i++)
                {
                    if (lastDestroyList[i] != null && lastDestroyList[i].obj != null)
                    {
                        int removeId = GetImgId(lastDestroyList[i].itemId);
                        if(HallInfoComponent.Instance.removeDic.ContainsKey(removeId))
                            HallInfoComponent.Instance.removeDic[removeId] += 1;
                        else
                        {
                            HallInfoComponent.Instance.removeDic.Add(removeId, 1);
                        }
                        GameObject.Destroy(this.lastDestroyList[i].obj.gameObject);
                    }
                }
                this.lastDestroyList.Clear();
                return true;
            }
            return false;
        }

        private int GetImgId(int itemId)
        {
            int imgId = itemId + this.rangNum;
            if (imgId > maxFruitNum)
                imgId -= maxFruitNum;
//            Log.Console("itemId:" + itemId + " random:" + this.rangNum + " imgId:" + imgId);
            return imgId;
        }
        private async void ResultSingle(int rank, int process)
        {
            EventDispatcher.PostEvent(ETEventName.VoodooEvent, null, MBMode.NormalFinish, rank, MBDataComponent.Instance.level);
            UIMBDataHelper.Instance.ResetGridState();
            string rankList = PlayerPrefs.GetString("ResultList", "");
            SingleLevel singleLevel = null;
            if (rankList != "")
            {
                singleLevel = LitJson.JsonMapper.ToObject<SingleLevel>(rankList);
            }
            else
            {
                {
                    singleLevel = new SingleLevel();
                    singleLevel.levelList = new List<LevelResult>();
                }
            }

            LevelResult levelResult = new LevelResult();
            levelResult.level = MBDataComponent.Instance.curPlayLevel;
            levelResult.rank = rank;
            levelResult.process = process;
            singleLevel.levelList.Add(levelResult);
            if (rank == 1)
            {
                UIMBDataHelper.Instance.MonyValueCtr(UIMBDataHelper.ResultMoney,1);
                Text txtNum = GameObjectMgr.Refer(this.self.goCoin, "txt_num").GetComponent<Text>();
                txtNum.text = UIMBDataHelper.Instance.MoneyValue.ToString();
                
                await TimerComponent.Instance.WaitAsync(600);
                //await TimerComponent.Instance.WaitAsync(500);
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMBResult, UILayer.Mid).Coroutine();
                if(MBDataComponent.Instance.curPlayLevel == MBDataComponent.Instance.level)
                    EventDispatcher.PostEvent(ETEventName.Dev2Dev, this.self, ETEventName.Dev2LvUp, MBDataComponent.Instance.level);
            }
            else
            {
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMBResult, UILayer.Mid).Coroutine();
            }
            

            await ETTask.CompletedTask;
        }

        private void ResultRemoveLocal(int rank, int level, int process)
        {
            string rankList = PlayerPrefs.GetString("ResultList", "");
            if (rankList != "")
            {
                SingleLevel singleLevel = LitJson.JsonMapper.ToObject<SingleLevel>(rankList);
                //string[] rankRarry = rankList.Split(',');
                for (int i = 0; i < singleLevel.levelList.Count; i++)
                {
                    if (singleLevel.levelList[i].rank == rank && singleLevel.levelList[i].level == level
                        && singleLevel.levelList[i].process == process)
                    {
                        singleLevel.levelList.RemoveAt(i);
                        break;
                    }
                }
                PlayerPrefs.SetString("ResultList", JsonHelper.ToJson(singleLevel));
            }
        }

        private List<UIMBItemComponent> lastUnLockList = new List<UIMBItemComponent>();
        private UIMBItemComponent lastMoveGrid;
        private Vector3 lastPos;
        private int lastInsertIndex = -1;
        private List<UIMBItemComponent> lastDestroyList = new List<UIMBItemComponent>();
        void OverGridUnLock(UIMBItemComponent go)
        {
            lastUnLockList.Clear();
            lastMoveGrid = go;
            lastPos = go.obj.position;
            int dicId = CommonFuc.SetItemId(go.row, go.num, go.level);
            List<int> idList = CommonFuc.GetRoundId(dicId);
            List<UIMBItemComponent> newShowList = new List<UIMBItemComponent>();
            for (int k = 0; k < idList.Count; k++)
            {
                int id0 = idList[k];
                if (this.self.allOneGridDic.ContainsKey(id0))
                {
                    var gridList = this.self.allOneGridDic[id0];
                    //foreach (var grids in this.self.allOneGridDic)
                    {
                        foreach (var oneGrid in gridList)
                        {
                            UIMBItemComponent grid0 = oneGrid.Value;
                            bool isShow = grid0.RemoveGrid(dicId);
                            if (isShow)
                            {
                                newShowList.Add(grid0);
                                if (grid0.itemEffectNum == MBItemType.BlindBox)
                                {
                                    float scaleX = grid0.obj.localScale.x;
                                    grid0.btnItem.gameObject.SetActive(false);
                                    float moveTime = 0.1f;
                                    Vector3 scaleRotate = new Vector3(0, 1, 1);
                                    if(grid0.obj.transform.position.y > go.obj.transform.position.y)
                                        scaleRotate = new Vector3(1, 0, 1);
                                    grid0.obj.DOScale(scaleRotate, moveTime ).onComplete = () =>
                                    {
                                        SoundComponent.Instance.PlayActionSound("Music4","fanpai");
                                        grid0.obj.Find(MBItemType.BlindBox.ToString()).gameObject.SetActive(false);
                                        grid0.obj.DOScale(new Vector3(1, 1, 1), moveTime ).onComplete = () =>
                                        {
                                            grid0.btnItem.gameObject.SetActive(true);
                                            grid0.btnItem.transform.localScale = Vector3.one;// * 0.3f;
                                            //grid0.btnItem.transform.DOScale(1, moveTime).SetEase(Ease.OutBack);
                                            grid0.btnItem.transform.DOScale(new Vector3(1.3f, 1.15f, 1), moveTime * 0.6f).onComplete = () => { 
                                                grid0.btnItem.transform.DOScale(1, moveTime).SetEase(Ease.OutBack);
                                            };
                                        };
                                    };
                                }
                                if (grid0.itemEffectNum == MBItemType.ItemIce)
                                {
                                    bool iceBreak = true;
                                    int iceDicId = CommonFuc.SetItemId(grid0.row, grid0.num, grid0.level);
                                    List<int> idIceList0 = CommonFuc.GetIceId(iceDicId);
                                    for (int m = 0; m < idIceList0.Count; m++)
                                    {
                                        int iceRoundId = idIceList0[m];
                                        if (this.self.allOneGridDic.ContainsKey(iceRoundId))
                                        {
                                            var iceRoundList = this.self.allOneGridDic[iceRoundId];
                                            if (iceRoundList.Count > 0)
                                            {
                                                // for (int n = 0; n < iceRoundList.Count; n++)
                                                // {
                                                //     if (iceRoundList[n].level >= grid0.level)
                                                //         iceBreak = false;
                                                // }
                                                iceBreak = false;

                                            }

                                        }
                                    }

                                    if (iceBreak)
                                    {
                                        IceBreak(grid0);
                                    }
                                }
                            }
                            lastUnLockList.Add(grid0);
                        }
                    }
                }
            }
            
            
            List<int> idIceList = CommonFuc.GetIceId(dicId);
            for (int k = 0; k < idIceList.Count; k++)
            {
                int id0 = idIceList[k];
                if (this.self.allOneGridDic.ContainsKey(id0))
                {
                    var gridList = this.self.allOneGridDic[id0];
                    foreach (var oneGrid in gridList)
                    {
                        UIMBItemComponent grid0 = oneGrid.Value;
                        if (grid0.itemEffectNum == MBItemType.ItemIce && grid0.overGrid.Count == 0)
                        {
                            IceBreak(grid0);
                        }
                    }
                }
            }
            
            this.SortAllItem(newShowList);
            for (int i = 0; i < newShowList.Count; i++)
            {
                if (newShowList[i].itemId > 1000)
                {
                    if(!this.keyLockDic.ContainsKey(newShowList[i].itemId))
                        keyLockDic.Add(newShowList[i].itemId, new List<UIMBItemComponent>());
                    keyLockDic[newShowList[i].itemId].Add(newShowList[i]);
                    // if (keyLockDic.ContainsKey(MBItemType.ItemLock) && keyLockDic.ContainsKey(MBItemType.ItemKey))
                    // {
                    //     UIMBItemComponent keyObj = keyLockDic[MBItemType.ItemKey][0];
                    //     UIMBItemComponent lockObj = keyLockDic[MBItemType.ItemLock][0];
                    //     keyObj.obj.parent = keyObj.obj.parent.parent;
                    //     keyObj.obj.SetAsLastSibling();
                    //     self.lockSpine.transform.position = lockObj.obj.position;
                    //     keyObj.bgImg.gameObject.SetActive(false);
                    //     keyObj.obj.DOMove(lockObj.obj.position, 1f).onComplete = () =>
                    //     {
                    //         Log.Console("钥匙和锁配对:");
                    //         OverGridUnLock(keyObj);
                    //         OverGridUnLock(lockObj);
                    //         DestroyGridSelf(keyObj);
                    //         DestroyGridSelf(lockObj);
                    //         GameObject.Destroy(keyObj.obj.gameObject);
                    //         GameObject.Destroy(lockObj.obj.gameObject);
                    //         this.lastDestroyList.Clear();
                    //     };
                    //     ETCommonFunc.Instance.DelayAction(900, () =>
                    //     {
                    //         self.lockSpine.gameObject.SetActive(true);
                    //         self.lockSpine.AnimationState.SetAnimation(0, "open", false);
                    //     });
                    // }
                }
            }
        }

        void IceBreak(UIMBItemComponent go)
        {
            go.obj.Find(MBItemType.ItemIce.ToString()).gameObject.SetActive(false);
            go.itemEffectNum = 0;
            GameObject spObj = GameObject.Instantiate(this.self.iceSpine.gameObject, this.self.iceSpine.transform.parent);
            spObj.SetActive(true);
            SkeletonGraphic sp = spObj.GetComponent<SkeletonGraphic>();
            sp.transform.position = go.obj.position;
            sp.AnimationState.SetAnimation(0, "ice_bomb", false);
            SoundComponent.Instance.PlayActionSound("Music4","iceBreaking");
            ETCommonFunc.Instance.DelayAction(1000, () => { GameObject.Destroy(spObj);}).Coroutine();
        }

        Dictionary<int, List<UIMBItemComponent>> keyLockDic = new Dictionary<int, List<UIMBItemComponent>>();

        void SetUnlockGridAgain(UIMBItemComponent go)
        {
            go.isDown = false;
            for (int i = 0; i < this.lastDestroyList.Count; i++)
            {
                this.lastDestroyList[i].obj.gameObject.SetActive(true);
                this.lastDestroyList[i].obj.localScale = Vector3.one;
                this.self.downList.Insert(lastInsertIndex - 2 + i, this.lastDestroyList[i]);
            }
            lastDestroyList.Clear();

            int asideIndex = -1;
            for (int i = 0; i < self.itemGridPosList.Count; i++)
            {
                if ((this.lastPos - self.itemGridPosList[i].position).magnitude < 0.1f)
                {
                    asideIndex = i;
                }
            }

            if (asideIndex > -1)
            {
                if (this.asideDic.ContainsKey(asideIndex))
                    this.asideDic[asideIndex] = go;
                else
                    this.asideDic.Add(asideIndex, go);
            }
            else
            {
                this.self.allGridDic.Add(go.itemUnitId, go);
            }

            //else
                this.self.downList.RemoveAt(this.lastInsertIndex);
                
            //go.obj.DOMove(this.lastPos, 0.1f);
            go.obj.DOScale(1, 0.1f);
            
            FlyFruitEffect(go.obj, this.lastPos, () =>
            {
                var obj = go.obj;
                obj.DOShakeScale(0.5f, Vector3.one * 0.1f);
                if (asideIndex == -1)
                {
                    go.bgImg.gameObject.SetActive(true);
                }
            }, 0.2f).Coroutine();
            int dicId = CommonFuc.SetItemId(go.row, go.num, go.level);

            if (!this.self.reviveList.ContainsKey(dicId))
            {
                for (int i = 0; i < this.lastUnLockList.Count; i++)
                {
                    if (lastUnLockList[i] != go)
                    {
                        lastUnLockList[i].AddOverGrid(dicId, go);
                        lastUnLockList[i].obj.parent = this.self.oneItem.transform.parent;
                    }
                }
            }

            for (int k = 0; k < this.self.downList.Count; k++)
            {
                Vector3 destPos2 = GetMovePos(k);
                self.downList[k].obj.DOMove(destPos2, 0.1f);
            }

            lastUnLockList.Clear();
        }

        Dictionary<int, UIMBItemComponent> asideDic = new Dictionary<int, UIMBItemComponent>();
        public void AddGridItemClick(int index)
        {
            UIMBItemComponent go = this.self.downList[0];
            if(go == null)
                Log.Error("go == null");
            if (asideDic.ContainsKey(index))
                this.asideDic[index] = go;
            else
                asideDic.Add(index, go);
            //go.obj.Find("btnItem").GetComponent<Button>().enabled = true;
            //go.obj.Find("bgImg").GetComponent<Button>().enabled = true;
            go.isDown = false;
            this.self.downList.RemoveAt(0);
            //go.obj.DOMove(this.self.itemGridPosList[index].position, 0.1f);
            FlyFruitEffect(go.obj, this.self.itemGridPosList[index].position, () =>
            {
                var obj = go.obj;
                obj.DOShakeScale(0.5f, Vector3.one * 0.1f);
            }, 0.1f).Coroutine();
            this.self.addList.Add(go);
            for (int k = 0; k < this.self.downList.Count; k++)
            {
                Vector3 destPos2 = GetMovePos(k);
                self.downList[k].obj.DOMove(destPos2, 0.1f);
            }
            EventDispatcher.PostEvent(EventName.AppsFlyEvent, null, AppsEventName.Puzzle_Use_PowerUp_Move);
            EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Puzzle_Use_PowerUp_Move);
        }

        void DestroyGridSelf(UIMBItemComponent go)
        {
            //GameObject.Destroy(go.obj.gameObject);
            int dicId = CommonFuc.SetItemId(go.row, go.num, go.level);
            this.self.allGridDic.Remove(dicId);
            int dic2dId = CommonFuc.SetItemId(go.row, go.num, 0);
            if (self.allOneGridDic.ContainsKey(dic2dId))
            {
                Dictionary<int, UIMBItemComponent> oneGrids = this.self.allOneGridDic[dic2dId];
                if (oneGrids.ContainsKey(go.level))
                {
                    oneGrids.Remove(go.level);
                }
                else
                {
                   
                }
            }
            else
            {
                
            }
            //GameObject.Destroy(go.obj.gameObject);
        }
        
        Vector3 GetMovePos(int index)
        {
            // if (index < 7)
            // {
                return this.self.downPosList[index].position;
            // }
            // else
            // {
            //     MBDataComponent.Instance.isWin = false;
            //     return  this.self.downPosList[6].position;
            // }
        }
        
        private async void RefreshLockGrid(bool isUnLockAni = false)
        {
            bool isUnLock = UIMBDataHelper.Instance.GetGridState(UIMBDataHelper.AddGridId);
            Transform unlockTran = self.bottomItem.transform.Find(UIMBDataHelper.AddGridId.ToString());
            if (isUnLock)
            {
                GameObjectMgr.Refer(unlockTran.gameObject,"imgLock").SetActive(false);
                if (isUnLockAni)
                {
                    unlockTran.GetComponent<Image>().color = Color.white;
                    await TimerComponent.Instance.WaitAsync(100);
                    SoundComponent.Instance.PlayActionSound("music4","unlock");
                    GameObject goldBoom = GameObjectMgr.Refer(unlockTran.gameObject, "Gold_Boom");
                    goldBoom.SetActive(true);
                    await TimerComponent.Instance.WaitAsync(2000);
                    goldBoom.SetActive(false);
                }else
                    unlockTran.GetComponent<Image>().color = Color.grey;
            }
            else
            {
                unlockTran.GetComponent<Image>().color = Color.grey;
                GameObjectMgr.Refer(self.bottomItem.transform.Find(UIMBDataHelper.AddGridId.ToString()).gameObject,"imgLock").SetActive(true);
            }
        }

        private int IsDestItemId(int itemId, bool igoreWait = false)
        {
            if (self.upList.Count == 0)
            {
                return 0;
            }
            if (this.isWaitOrder && !igoreWait)
                return 0;

            for (int j = 0; j < self.upList[0].itemList.Count; j++)
            {
                if (itemId == self.upList[0].itemList[j].itemId && self.upList[0].itemList[j].num > 0)
                    return j + 1;
            }
            return 0;
        }

        private async void FindNextMatch()
        {
            bool isHaveMove = false;
            for (int i = self.downList.Count - 1; i >= 0; i--)
            {
                int index = IsDestItemId(this.self.downList[i].itemId, true);
                if (index > 0)
                {
                    isHaveMove = true;
                    var item = self.downList[i];
                    MoveToContainer(item, true, index);
                    ClickMatchItem(200, item.itemId).Coroutine();
                }
            }
            if (isHaveMove)
            {
                await TimerComponent.Instance.WaitAsync(300);
                this.SortDownItem();
            }
        }

        private void MoveToContainer(UIMBItemComponent uimbItemComponent, bool isDestroy, int index)
        {
            if(self.upList[0].itemList[index - 1].num <= 0)
                return;
            if ((int) AppInfoComponent.Instance.guideStep < (int) GuideStep.End && AppInfoComponent.Instance.guiding)
            {
                Transform parent = GlobalComponent.Instance.UI.Find("Mid2/YinDao(Clone)");
                if(parent != null) uimbItemComponent.obj.parent = parent;
            }
            CommonFuc.SetAsLast(uimbItemComponent.obj);
            //uimbItemComponent.obj.SetAsLastSibling();
            uimbItemComponent.bgImg.gameObject.SetActive(false);
            
            Vector3 destPos = this.self.upGrid.Find(index.ToString()).position;
            var obj0 = uimbItemComponent.obj;
            FlyFruitEffect(obj0, destPos, () =>
            {
                if (obj0 != null)
                {
                    obj0.gameObject.SetActive(false);
                    if (AppInfoComponent.Instance.guideStep >= (int) GuideStep.End)
                    {
                        //this.self.upList[0].itemList[index - 1].mbItemComponent.obj.DOShakeScale(0.7f, Vector3.one * 0.1f);
                        var obj = this.self.upList[0].itemList[index - 1].mbItemComponent.obj;
                        obj.DOShakeScale(0.5f, Vector3.one * 0.1f); //.onComplete = () =>
                        //                    {
                        //                        obj.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                        //                   };
                    }
                }
            }).Coroutine();
            this.self.downList.Remove(uimbItemComponent);
            if (isDestroy)
            {
                this.lastDestroyList.Add(uimbItemComponent);
            }
        }

        private async ETTask FlyFruitEffect(Transform obj0, Vector3 destPos, Action callback, float moveTime = 0.2f)
        {
            Transform obj = obj0;
            Vector3 originPos = obj.position;
            Vector3[] path = ETCommonFunc.Instance.BezierPath(originPos, (originPos + destPos) * 2 / 3 + new Vector3(0, 3, 0), destPos, 30);

            GameObject particle = GameObject.Instantiate(this.self.particleFruit, obj.parent);
            particle.transform.position = originPos;
            particle.SetActive(true);
            particle.transform.DOPath(path, moveTime);

            Image grey = GameObject.Instantiate(this.self.flyShader.gameObject, obj.parent).GetComponent<Image>();
            grey.transform.position = originPos;
            grey.transform.localScale = new Vector3(1f, 1f, 1);
            grey.gameObject.SetActive(true);
            grey.transform.DOMove(destPos, moveTime);
            grey.DOFade(0.5f, moveTime * 0.39f).onComplete = () => grey.DOFade(1, moveTime * 0.39f);
            Vector3 scale = obj.localScale;
            ETCommonFunc.Instance.DelayAction((int)(moveTime * 1100), () =>
            {
                if(grey != null) obj.localScale = scale;
                if(grey != null) GameObject.Destroy(grey.gameObject);
                if(particle != null) GameObject.Destroy(particle);
            }).Coroutine();

            obj.DOPath(path, moveTime);
            ETCommonFunc.Instance.DelayAction((int)(moveTime * 1000), ()=>
            {
                if(grey != null)
                    GameObject.Destroy(grey.gameObject);
                callback();
            }).Coroutine();
            
            obj.DOScale(scale * 1.6f, moveTime * 0.6f);
            ETCommonFunc.Instance.DelayAction((int)(moveTime * 600),() =>
            {
                grey.transform.SetAsFirstSibling();
                obj.DOScale(scale, moveTime * 0.4f);
            }).Coroutine();
            await ETTask.CompletedTask;
            //var tweenMoveDown = obj.DOPath(path,moveTime);//.SetEase(Ease.InBack);
        }

        private int[] orderPosX = { 200, -100, -80, -80, -80 };
        private async ETTask ClickMatchItem(int waitTime, int itemId)
        {
            bool isAll = true;
            List<UpOneItem> firstItemList = self.upList[0].itemList;
            UpOneItem upOneItem = null;
            for (int j = 0; j < firstItemList.Count; j++)
            {
                if (firstItemList[j].itemId == itemId)
                {
                    firstItemList[j].num--;
                    if (firstItemList[j].num < 1)
                    {
                        upOneItem = firstItemList[j];
                    }
                }

                if (firstItemList[j].num > 0)
                    isAll = false;
            }
            if (isAll)
            {
                self.upList.RemoveAt(0);
                this.isWaitOrder = true;
                ETCommonFunc.Instance.DelayAction(3000, () =>
                {
                    isWaitOrder = false;
                    this.FindNextMatch();
                    if(this.self.downList.Count >= this.maxNum && GameDataMgr.Instance.CanPassMB == 0 && !this.isWaitOrder)
                        this.Failed();
                }).Coroutine();
            }

            if (waitTime > 0)
                await TimerComponent.Instance.WaitAsync(waitTime);
            if (upOneItem != null && upOneItem.num == 0)
            {
                AddGou(upOneItem);
            }
            
            if (isAll)
            {
                Transform curUpGrid = self.upGrid;
                VibrationHelper.GetInstance().SendVibration(1,1,1);
                await TimerComponent.Instance.WaitAsync(200);
                this.self.upGrid.DOScaleX(0, 0.12f).onComplete = () =>
                {
                    self.upGrid.gameObject.SetActive(true);
                    self.upGrid.localScale = Vector3.one;
                    self.upGrid.localPosition = new Vector3(-880, 435, 0);
                };
                await TimerComponent.Instance.WaitAsync(130);
                self.qipao.gameObject.SetActive(true);
                self.qipao.transform.localPosition = new Vector3(Random.Range(320, 420), Random.Range(650, 850), 0);
                self.qipao.AnimationState.SetAnimation(0, "animation", false);
                ETCommonFunc.Instance.DelayAction(800, () =>
                {
                    self.qipao.gameObject.SetActive(false);
                    GameObject baoza = GameObject.Instantiate(this.self.paopaobaozha, this.self.fruitParent.transform);
                    baoza.SetActive(true);
                    baoza.transform.position = this.self.qipao.transform.position - new Vector3(0, 0.3f, 0);
                    baoza.transform.DOLocalMoveY(baoza.transform.localPosition.y - 600, 0.5f);
                    SoundComponent.Instance.PlayActionSound("Music3", "coin");
                    SoundComponent.Instance.PlayActionSound("Music4","fanpai");
                    ETCommonFunc.Instance.DelayAction(3000, () => { GameObject.Destroy(baoza);}).Coroutine();
                    UIMBDataHelper.Instance.MonyValueCtr(UIMBDataHelper.AddSMoney,1);
                    int lastMoney = UIMBDataHelper.Instance.MoneyValue - UIMBDataHelper.AddSMoney;
                    int newMoney = UIMBDataHelper.Instance.MoneyValue;
                    Text txtNum = GameObjectMgr.Refer(this.self.goCoin, "txt_num").GetComponent<Text>();
                    DOTween.To(delegate(float value)
                    {
                        double tempValue = (int)value;
                        
                        txtNum.text = tempValue.ToString();
                    }, lastMoney, newMoney, 0.7f);
                    //this.PlayMoneyAni2(firstItemList.Count);
                }).Coroutine();
                this.self.txtNowFruit.text = upItemIndex.ToString();
                for (int j = firstItemList.Count -1; j >=0 ; j--)
                {
                    var item = firstItemList[j].mbItemComponent;
                    if (j == firstItemList.Count - 1)
                    {
                        this.modelMgr.PlayMenuStarAni();
                    }
                    item.obj.transform.localScale = Vector3.one;
                    //item.obj.DOScale(0.3f, 0.2f);
                    //item.obj.DOLocalMove(new Vector3(73,700,0), 0.2f).SetEase(Ease.InCubic).onComplete = () =>
                    {
                        GameObject.Destroy(item.obj.gameObject);
                    };
                    //await TimerComponent.Instance.WaitAsync(100);
                }
                // modelMgr.PlayMenuAniOut();
                // await TimerComponent.Instance.WaitAsync(200);
                EventDispatcher.PostEvent(EventName.FirstOrderGuide, this.self, false);
                if (this.self.upList.Count == 0)
                {
                    modelMgr.BuyFinished(true);
                    return;
                }
                modelMgr.BuyFinished(upItemList.Count == upItemIndex);
                CreateNextOrder(curUpGrid, upItemIndex + 1);
                Transform nextUpGrid = NextUpGrid();
                int newOrderNum = 2;
                if(upItemIndex < upItemList.Count)
                    newOrderNum = upItemList[this.upItemIndex].itemList.Count;
                //Log.Console("newOrderNum:" + newOrderNum);
                Log.Console("5555555555");
                nextUpGrid.DOLocalMoveX(orderPosX[newOrderNum], 0.2f).SetEase(Ease.Linear);
                ETCommonFunc.Instance.DelayAction(200, () =>
                {
                    var items = self.upList[0].itemList;
                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i].mbItemComponent.btnItem.material = null;
                    }
                }).Coroutine();
                firstItemList = self.upList[0].itemList;
                for (int j = 0; j < firstItemList.Count; j++)
                {
                    firstItemList[j].mbItemComponent.obj.gameObject.SetActive(true);
                }
                if (upItemList.Count == upItemIndex)
                {
                    //modelMgr.BuyFinished(true);
                    return;
                }
                float moveX = -820;
                if(newOrderNum == 1) moveX = -680;
                if(upItemIndex + 1 < upItemList.Count)
                    self.upGrid.DOLocalMoveX(moveX, 0.2f);
                Log.Console("666666666");
                // ETCommonFunc.Instance.DelayAction(600, () => {
                //     if (upItemIndex < upItemList.Count)
                //     {
                //         curUpGrid.localScale = new Vector3(1, 1, 1);
                //         curUpGrid.gameObject.SetActive(true);
                //         //this.self.upGrid.DOScaleY(1, 0.2f).onComplete = () => 
                //         curUpGrid.localPosition = new Vector3(-540, 486, 0);
                //         SetOrderBg(self.upGrid.Find("bg").GetComponent<RectTransform>(), self.upList[0].itemList.Count);
                //         EventDispatcher.PostEvent(EventName.FirstOrderGuide, this.self, true);
                //     }
                // }).Coroutine();
                await TimerComponent.Instance.WaitAsync(210);
                self.upGrid = nextUpGrid;
                await ResortUpGrid(firstItemList);
                upItemIndex++;
                //CreateNextOrder(curUpGrid, upItemIndex);
            }else 
            {
                for (int j = 0; j < firstItemList.Count; j++)
                {
                    if (itemId == firstItemList[j].itemId)
                    {
                        //if (firstItemList[j].num > 0)
                        GameObject goUpNum = GameObjectMgr.Refer(firstItemList[j].mbItemComponent.obj.gameObject,"goUpNum");
                        Text txtUpNum = GameObjectMgr.Refer(goUpNum,"upNum").GetComponent<Text>();
                        txtUpNum.text = (firstItemList[j].num).ToString();
                    }
                }

            }
            await ETTask.CompletedTask;
        }
        
        private void CreateNextOrder(Transform curUpGrid, int upIndex)
        {
            UpItemGroup group = new UpItemGroup();
            this.self.upList.Add(group);
            //生成下个预览订单
            if (upIndex < upItemList.Count)
            {
                List<UpOneItem> oneItemList = upItemList[upIndex].itemList;
                for (int j = 0; j < oneItemList.Count; j++)
                {
                    UIMBItemComponent mbItemComponent = CreateOneUpItem(j, oneItemList[j].itemId, oneItemList[j].num, curUpGrid);
                    mbItemComponent.btnItem.material = AppInfoComponent.Instance.grey;
                    group.AddItem(oneItemList[j].itemId, oneItemList[j].num, mbItemComponent);
                }
                SetOrderBg(curUpGrid.Find("bg").GetComponent<RectTransform>(), oneItemList.Count);
            }
            else
            {
                curUpGrid.gameObject.SetActive(false);
            }
        }

        private Transform NextUpGrid()
        {
            Transform upGrid = null;
            if (this.self.upGrid == self.upGrid1)
                upGrid = self.upGrid2;
            else
                upGrid = self.upGrid1;
            return upGrid;
        }

        async void AddGou(UpOneItem oneItem)
        {
            // if(oneItem == null || oneItem.mbItemComponent.obj == null)
            //     return;
            // if (GameObjectMgr.Refer(oneItem.mbItemComponent.obj.gameObject,"goGou") != null)
            // {
            //     return;
            // }
            // //await TimerComponent.Instance.WaitAsync(400);
            // //SoundComponent.Instance.PlayActionSound("Music4","Money1");
            // GameObject goGou = GameObject.Instantiate(GameObjectMgr.Refer(self.goUpNum.transform.parent.gameObject, "goGou"));
            // goGou.name = "goGou";
            // goGou.transform.SetParent(oneItem.mbItemComponent.obj);
            // oneItem.mbItemComponent.btnItem.material = AppInfoComponent.Instance.grey;
            // goGou.transform.localPosition = new Vector3(1,-42,0);
            // goGou.transform.localScale = Vector3.zero;
            // GameObjectMgr.Refer(oneItem.mbItemComponent.obj.gameObject, "goUpNum").SetActive(false);
            // goGou.transform.DOScale(1.3f,0.4f).SetEase(Ease.OutBack);
            // goGou.SetActive(true);
             await ETTask.CompletedTask;
        }
        
        private async void PlayMoneyAni2(int count)
        {
            UIAniEffectData effectData = new UIAniEffectData();
            effectData.targetPos = GameObjectMgr.Refer(this.self.goCoin,"img_icon").transform.position;
            effectData.rotationPos = new Vector3(this.self.upGrid.position.x + 1, this.self.upGrid.position.y + 2, 0);
            List<Vector3> list = new List<Vector3>();
            for(int i = 0; i < 10; i++)
                list.Add(this.self.qipao.transform.position);
            effectData.resoucePos = list;
            effectData.callBack = (int num) =>
            {
                int lastMoney = UIMBDataHelper.Instance.MoneyValue - UIMBDataHelper.AddSMoney;
                int newMoney = UIMBDataHelper.Instance.MoneyValue;
                Text txtNum = GameObjectMgr.Refer(this.self.goCoin, "txt_num").GetComponent<Text>();
                DOTween.To(delegate(float value)
                {
                    double tempValue = (int)value;
                    txtNum.text = tempValue.ToString();
                }, lastMoney, newMoney, 0.7f);
            };
            UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIAniEffect, UILayer.Mid ,effectData).Coroutine();

            await TimerComponent.Instance.WaitAsync(1500);
            // SkeletonGraphic skeletonGraphic =GameObjectMgr.Refer(this.self.goCoin, "moneySpine").GetComponent<SkeletonGraphic>();
            // GameObjectMgr.Refer(this.self.goCoin, "moneySpine").SetActive(true);
            // skeletonGraphic.AnimationState.SetAnimation(0, "money_bomb", false);
            SoundComponent.Instance.PlayActionSound("MusicMb","Cash");
            await TimerComponent.Instance.WaitAsync(200);
            for(int i = 0 ;i < 3;i++)
                VibrationHelper.GetInstance().SendVibration();
        }
        
        private async void PlayMoneyAni(int count)
        {
            UIAniEffectData effectData = new UIAniEffectData();
            effectData.targetPos = GameObjectMgr.Refer(this.self.goCoin,"img_icon").transform.position;
            effectData.rotationPos = new Vector3(this.self.upGrid.position.x + 1, this.self.upGrid.position.y + 2, 0);
            List<Vector3> list = new List<Vector3>();
            for (int i = 1; i < 4; i++)
            {
                if (i <= count)
                {
                    GameObject go = GameObjectMgr.Refer(this.self.upGrid.gameObject, i.ToString());
                    list.Add(go.transform.position);
                }
            }
            effectData.resoucePos = list;
            effectData.callBack = (int num) =>
            {
                int lastMoney = UIMBDataHelper.Instance.MoneyValue - UIMBDataHelper.AddSMoney;
                int newMoney = UIMBDataHelper.Instance.MoneyValue;
                Text txtNum = GameObjectMgr.Refer(this.self.goCoin, "txt_num").GetComponent<Text>();
                DOTween.To(delegate(float value)
                {
                    double tempValue = (int)value;
                    txtNum.text = tempValue.ToString();
                }, lastMoney, newMoney, 0.7f);
            };
            UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIAniEffect, UILayer.Mid ,effectData).Coroutine();

            await TimerComponent.Instance.WaitAsync(1500);
            SkeletonGraphic skeletonGraphic =GameObjectMgr.Refer(this.self.goCoin, "moneySpine").GetComponent<SkeletonGraphic>();
            GameObjectMgr.Refer(this.self.goCoin, "moneySpine").SetActive(true);
            skeletonGraphic.AnimationState.SetAnimation(0, "money_bomb", false);
            SoundComponent.Instance.PlayActionSound("MusicMb","Cash");
            await TimerComponent.Instance.WaitAsync(200);
            for(int i = 0 ;i < 3;i++)
                VibrationHelper.GetInstance().SendVibration();
        }

        List<UpItemGroup> upItemList = new List<UpItemGroup>();
        private int _upitemIndex = 0;

        private int upItemIndex
        {
            set
            {
                this._upitemIndex = value;
            }
            get
            {
                return this._upitemIndex;
            }
        }
        async ETTask ResortUpGrid(List<UpOneItem> oneItemList)
        {
            GameObject goMenuStar = GameObjectMgr.Refer(this.self.menu, "goMenuStar");
            List<GameObject> list = new List<GameObject>();
            //for (int i = 0; i < this.self.upList.Count; i++)
            // {
            //     if(i != 0)
            //         continue;
            //    var oneItemList = self.upList[i].itemList;
                // for (int j = 0; j < oneItemList.Count; j++)
                // {
                //     oneItemList[j].mbItemComponent.obj.gameObject.SetActive(false);
                // }

                for (int j = 0; j < oneItemList.Count; j++)
                {
                    if (oneItemList[j] == null || oneItemList[j].mbItemComponent == null || oneItemList[j].mbItemComponent.obj == null)
                    {
                        continue;
                    }
                    //oneItemList[j].mbItemComponent.obj.position = this.self.upGrid.Find((i * 2 + j + 1).ToString()).position;
//                    oneItemList[j].mbItemComponent.obj.position = this.self.upGrid.Find((j + 1).ToString()).position;
//                    oneItemList[j].mbItemComponent.obj.gameObject.transform.localScale = Vector3.zero;
 //                   oneItemList[j].mbItemComponent.obj.gameObject.SetActive(true);
                    var obj = oneItemList[j].mbItemComponent.obj;
                    // obj.gameObject.transform.DOScale(0.7f, 0.2f).onComplete = () =>
                    // {
                    //     obj.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    // };
                    
                    // GameObject tempAni = GameObject.Instantiate(goMenuStar);
                    // tempAni.transform.SetParent(oneItemList[j].mbItemComponent.obj.gameObject.transform.parent);
                    // tempAni.transform.SetSiblingIndex(0);
                    // tempAni.transform.localScale = Vector3.one;
                    // tempAni.transform.position = oneItemList[j].mbItemComponent.obj.position;
                    // tempAni.SetActive(true);
                    // SkeletonGraphic skeletonGraphic = tempAni.GetComponent<SkeletonGraphic>();
                    // skeletonGraphic.AnimationState.SetAnimation(0, "star", false);
                    
                    //list.Add(tempAni);
                    await TimerComponent.Instance.WaitAsync(100);
                }
            //}
            await TimerComponent.Instance.WaitAsync(200);
            FindNextMatch();
            this.isWaitOrder = false;
            for (int i = 0; i < list.Count; i++)
            {
                GameObject.Destroy(list[i]);
            }
            if(this.self.downList.Count >= this.maxNum && GameDataMgr.Instance.CanPassMB == 0 && !this.isWaitOrder)
                this.Failed();
            list.Clear();
        }

        private ModelMgr modelMgr;
        private string[] upItems = {
            "8:3.1:1.6:2.5:3",
            "4:4.2:4.3:2,1:4.3:4.2:3,4:2.1:2.3:3",
            "1:3.2:3.3:4,5:3.6:2,3:4.1:4.5:4",
            "1:3.2:3.3:3.4:3",
            "4:2.2:2,2:3.5:3.1:3,1:1.2:3.5:2,1:3.3:2.5:3",
            "3:4.5:1.1:2,3:1.6:1,1:3.4:1,3:3.6:3.1:1,2:2.4:3.5:1.6:2",
            "6:2.2:2,2:2.6:2,4:2.3:3.1:3,3:3.1:3.6:2,2:3.1:2,3:2.2:2.5:2",//7
            "1:3.2:2,1:2.4:4,2:2.1:2",
            "2:3.3:3,1:3.2:3.4:3,4:2.3:3,3:2.2:3,3:3.1:2,2:2.1:2",
            "1:3.2:3.3:3,1:3.2:3.3:3,1:3.2:3.3:3,1:3.2:3.3:3,1:3.2:3.3:3,1:4.2:3.3:3",//10
            "4:2.5:3,5:2.3:3,2:3.1:2.3:2,5:2.2:3.4:2,5:3.2:3.4:3,4:2.2:3",
            "6:2.2:2,2:2.6:2,4:2.3:3.1:3,3:3.1:3.6:2,2:3.1:2,3:2.2:2.5:2",//换到15了
            "1:3.5:3.2:3.4:3,1:3.4:3.3:3.5:3,1:3.2:3.3:3.5:3",
            "3:3.2:3.1:3.5:3,5:3.4:3.2:3.1:3,3:3.4:3.1:3.5:3",//14
            "4:4.2:4,1:4.4:3,1:2.5:2,5:3.4:3.6:2,5:4.2:4.3:4,5:3.2:4,5:3.3:2.6:3.1:1,6:2.2:4.4:3.1:4",//15
            "5:2.2:3.4:2.1:3,5:1.3:1.2:3.4:1,1:3.5:1.3:3.2:1",
            "5:1.1:3.3:3.2:3,4:3.1:3.2:3.3:3,4:3.1:2.5:3.2:3",
            "4:2.3:3.5:3.1:1,3:3.1:3.2:2.4:3,5:2.3:2.4:3.1:3",
            "3:3.1:1.2:1.5:3,5:1.3:1.1:3.2:3,3:3.2:1.4:3.5:3",
            "1:3.3:3.4:3.2:2,4:3.5:1.2:2.1:3,2:1.5:3.3:3.4:3"
        };

        void CreateUpList()
        {
            this.self.upGrid = this.self.upGrid1;
            upItemList.Clear();
            upItemIndex = 0;
            for (int i = self.upList.Count - 1; i >= 0; i--)
            {
                var itemList0 = this.self.upList[i].itemList;
                for (int j = 0; j < itemList0.Count; j++)
                {
                    if (itemList0[j] == null)
                    {
                        continue;
                    }
                    if (itemList0[j].mbItemComponent.obj == null)
                    {
                        continue;
                    }
                    GameObject.Destroy(itemList0[j].mbItemComponent.obj.gameObject);
                }
            }
            self.upList.Clear();
            string ups = "";
            if (this.order != "")
                ups = this.order;
            else
            {
                if (MBDataComponent.Instance.curPlayLevel > upItems.Length)
                    ups = upItems[upItems.Length - 1];
                else
                    ups = this.upItems[MBDataComponent.Instance.curPlayLevel - 1];
            }
            string[] itemups = ups.Split(',');
            for (int i = 0; i < itemups.Length; i++)
            {
                string[] pointStr = itemups[i].Split('.');
                if (string.IsNullOrEmpty(itemups[i]))
                    break;
                UpItemGroup group = new UpItemGroup();
                for (int j = 0; j < pointStr.Length; j++)
                {
                    if (string.IsNullOrEmpty(pointStr[j]))
                        break;
                    string[] split = pointStr[j].Split(':');
                    int itemId = int.Parse(split[0]);
                    int num = int.Parse(split[1]);
                    group.AddItem(itemId, num);
                }
                upItemList.Add(group);
            }

            self.upGrid1.DOKill();
            self.upGrid2.DOKill();
            self.upGrid1.localPosition = new Vector3(this.orderPosX[upItemList[0].itemList.Count], 435, 0);
            float moveX = -820;
            if(upItemList[0].itemList.Count == 1) moveX = -680;
            self.upGrid2.localPosition = new Vector3(moveX, 435, 0);
            self.upGrid1.gameObject.SetActive(true);
            self.upGrid2.gameObject.SetActive(true);
            this.self.txtAllFruit.text = "/" + this.upItemList.Count;
            this.self.txtNowFruit.text = "0";
            for (int i = 0; i < 2 && i < upItemList.Count; i++)
            {
                int count = upItemList[i].itemList.Count;
                UpItemGroup group = new UpItemGroup();
                //UpItemGroup group = new UpItemGroup(upItemList[i].itemList[0].itemId, upItemList[i].itemList[0].num, mbItemComponent);
                this.self.upList.Add(group);
                for (int j = 0; j < count; j++)
                {
                    UpOneItem oneItem = upItemList[i].itemList[j];
                    if (i == 0)
                    {
                        UIMBItemComponent mbItemComponent = CreateOneUpItem(i * 2 + j, oneItem.itemId, oneItem.num, this.self.upGrid);
                        group.AddItem(oneItem.itemId, oneItem.num, mbItemComponent);
                    }
                    else if (i > 0)
                    {
                        UIMBItemComponent mbItemComponent = CreateOneUpItem(j, oneItem.itemId, oneItem.num, this.self.upGrid2);
                        mbItemComponent.btnItem.material = AppInfoComponent.Instance.grey;
                        group.AddItem(oneItem.itemId, oneItem.num, mbItemComponent);
                        this.self.upGrid2.gameObject.SetActive(true);
                        //mbItemComponent.obj.gameObject.SetActive(false); // Find("grey").gameObject.SetActive(true);
                    }
                }
            }
            upItemIndex++;
            if(modelMgr == null)
                self.modelMgr = modelMgr = new ModelMgr(this.self.gameSelf,this.self.menu);
            modelMgr.InitModel(this.upItemList.Count, this.self.peoples);
            SetOrderBg(self.upGrid.Find("bg").GetComponent<RectTransform>(), self.upList[0].itemList.Count);
            if(self.upList.Count > 1)
                SetOrderBg(self.upGrid2.Find("bg").GetComponent<RectTransform>(), self.upList[1].itemList.Count);
            else
                this.self.upGrid2.gameObject.SetActive(false);
        }

        private void SetOrderBg(RectTransform bg, int num)
        {
            // var bg = this.self.upGrid.Find("bg").GetComponent<RectTransform>();
            // int num = self.upList[0].itemList.Count;
            if(num == 2)
                bg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 228);
            else if(num == 1)
                bg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 122);
            else if(num == 3)
                bg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 320);
            else if(num == 4)
                bg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 410);
        }

        private UIMBItemComponent CreateOneUpItem(int index, int itemId, int num, Transform parent)
        {
            index = index + 1;
            GameObject item = GameObject.Instantiate(this.self.oneItem.gameObject, parent);
            GameObject goUpNum = GameObject.Instantiate(this.self.goUpNum, item.transform);
            //item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            goUpNum.SetActive(true);
            goUpNum.transform.localPosition = new Vector3(1, -42, 0);
            goUpNum.name = "goUpNum";
            item.name = "up" + index;
            item.SetActive(true);
//            item.transform.localScale = Vector3.one * 0.7f;
            GameObjectMgr.Refer(goUpNum, "upNum").GetComponent<Text>().text = num.ToString();
//            upNum.SetActive(index <= 2);

            UI ui = self.AddChild<UI,string,GameObject>(UIType.UIMBItem, item);
            UIMBItemComponent mbItemComponent = ui.AddComponent<UIMBItemComponent>();
            mbItemComponent.SetMBItemInfo(0, 0, 0, 0, itemId, item.transform);
            mbItemComponent.isDown = true;
            mbItemComponent.bgImg.gameObject.SetActive(false);

            item.transform.position = parent.Find(index.ToString()).position;
            Image img = item.transform.Find("btnItem").GetComponent<Image>();
            int imgId = itemId + this.rangNum;
            if (imgId > maxFruitNum)
                imgId -= maxFruitNum;
            img.sprite = this.spriteList[imgId - 1];
            img.SetNativeSize();
            return mbItemComponent;
        }
    }
    
    [ObjectSystem]
    public class UIMBDestroySystem: DestroySystem<UIMBComponent>
    {
        public override void Destroy(UIMBComponent self)
        {
            EventDispatcher.RemoveObserver(EventName.MBAgain);
            EventDispatcher.RemoveObserver(EventName.NetWaitLongSingleResult);
            EventDispatcher.RemoveObserver(EventName.RechargeItem);
            //EventDispatcher.RemoveObserver(EventName.UpdateItemInMB, );
            EventDispatcher.RemoveObserver(ETEventName.ShowItemUnlock);
            EventDispatcher.RemoveObserver(ETEventName.CloseUIMB);

            EventDispatcher.RemoveObserver("ClickFruit");
        }
    }
    
    public class UIMBUpdateSystem: UpdateSystem<UIMBComponent>
    {
//        private TouchPhase phase = TouchPhase.Ended;
        public override void Update(UIMBComponent self)
        {
            // if (Input.touchCount > 0)
            // {
            //     Touch touch0 = Input.GetTouch(0);
            //     touch0 = Input.GetTouch(0);
            //     if (touch0.phase == TouchPhase.Began)
            //     {
            //         phase = TouchPhase.Began;
            //         Log.Console(touch0.position.ToString());
            //     }
            //     else if (touch0.phase == TouchPhase.Moved)
            //         phase = TouchPhase.Moved;
            //     else if (touch0.phase == TouchPhase.Ended)
            //         phase = TouchPhase.Ended;
            // }
        }
    }
}