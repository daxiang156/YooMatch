using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class UIPopAwakeSystem : AwakeSystem<UIPopComponent>
    {
        private UIPopComponent self;
        private TipInfoComponent tip;
        int costCoin = 0;
        private int coin_shop_reveive = 1;
        public override void Awake(UIPopComponent self)
        {
            this.self = self;
            // GameObject gameSelf = self.GetParent<UI>().GameObject;
            // gameSelf.SetActive(false);
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.btnMask = rc.Get<GameObject>("btnMask").GetComponent<Button>();
            self.btnMask.onClick.AddListener(OnClickClose);
            self.panel = rc.Get<GameObject>("panel");
            self.btnClose = rc.Get<GameObject>("btnClose").GetComponent<Button>();
            self.btnConfirm = rc.Get<GameObject>("btnConfirm").GetComponent<Button>();
            self.text = rc.Get<GameObject>("text").GetComponent<Text>();
            self.netWork = rc.Get<GameObject>("netWork");
            self.title = rc.Get<GameObject>("title").GetComponent<Text>();
            self.btnCancel = rc.Get<GameObject>("btnCancel").GetComponent<Button>();
            self.btnShop = rc.Get<GameObject>("btnShop").GetComponent<Button>();
            self.btnAd = rc.Get<GameObject>("btnAd").GetComponent<Button>();
            
            self.txtConfirm = rc.Get<GameObject>("txtConfirm").GetComponent<Text>();
            self.txtCancel = rc.Get<GameObject>("txtCancel").GetComponent<Text>();
            
            self.itemNum = rc.Get<GameObject>("itemNum").GetComponent<Text>();
            self.ItemIcon = rc.Get<GameObject>("ItemIcon").GetComponent<Image>();

            PlayAni();
            
            tip = Game.Scene.GetComponent<TipInfoComponent>();
            this.self.netWork.SetActive(tip.isNet);
            if(tip.isNet)
                self.text.text = "";
            else
                self.text.text = tip.text;
            
            if (tip.titleStr != "")
            {
                self.title.text = this.tip.titleStr;
            }

            Log.Console("price:" + tip.itemId);
            if (this.tip.itemId > 0 && this.tip.itemNum > 0)
            {
                self.itemNum.transform.parent.gameObject.SetActive(true);
                if(this.tip.itemNum < 999)
                    self.itemNum.text = "X" + this.tip.itemNum;
                else
                {
                    self.itemNum.text = "";
                }
                
                        
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(tip.itemId);
                self.ItemIcon.sprite = Resources.Load("Image/" + itemConfig.Icon, typeof(Sprite)) as Sprite;
                
                self.btnAd.transform.parent.gameObject.SetActive(this.tip.usdeNum != 0);
                self.btnShop.transform.parent.gameObject.SetActive(true);
                self.btnConfirm.transform.parent.gameObject.SetActive(false);
                self.btnCancel.transform.parent.gameObject.SetActive(false);

                coin_shop_reveive = FireBComponent.Instance.GetRemoteLong(FireBRemoteName.coin_shop_reveive);
                if(coin_shop_reveive == 1)
                {
                    self.btnShop.transform.Find("txtShop").gameObject.SetActive(false);
                    self.btnShop.transform.Find("coin").gameObject.SetActive(true);
                    Text txt = self.btnShop.transform.Find("coin/txtPrice").GetComponent<Text>();

                    Dictionary<int, ActivityGoodsConfig> activityConfigDic = ActivityGoodsConfigCategory.Instance.GetAll();
                    foreach (var item in activityConfigDic)
                    {
                        if (item.Value.goods.StartsWith(tip.itemId + ":") && item.Value.cost.StartsWith(KeyDefine.GameCoin.ToString()))
                        {
                            tip.goodId = item.Key;
                            string priceStr = item.Value.cost;
                            costCoin = int.Parse(priceStr.Split(':')[1]) * this.tip.itemNum;
                            txt.text = costCoin.ToString();
                            break;
                        }
                    }

                    if (HallInfoComponent.Instance.GetItemNum(KeyDefine.GameCoin) < costCoin)
                    {
                        self.btnShop.transform.Find("txtShop").gameObject.SetActive(true);
                        self.btnShop.transform.Find("coin").gameObject.SetActive(false);
                        // this.tip.itemNum = 0;
                        // this.tip.itemId = 0;
                        // UIHelper.Create(GlobalComponent.Instance.scene, UIType.UICharge, UILayer.Mid).Coroutine();
                        // ETCommonFunc.Instance.DelayAction(200, () =>
                        // {
                        //     UIHelper.Remove(self.ZoneScene(), UIType.UIPop).Coroutine();
                        // });
                        //return;
                    }
                    //gameSelf.SetActive(true);
                }
                else
                {
                    self.btnShop.transform.Find("txtShop").gameObject.SetActive(true);
                    self.btnShop.transform.Find("coin").gameObject.SetActive(false);
                }
            }
            else
            {
                //gameSelf.SetActive(true);
                self.btnAd.transform.parent.gameObject.SetActive(false);
                self.btnShop.transform.parent.gameObject.SetActive(false);
                self.btnConfirm.transform.parent.gameObject.SetActive(true);
                self.btnCancel.transform.parent.gameObject.SetActive(false);
                self.itemNum.transform.parent.gameObject.SetActive(false);
            }

            this.AddListner();
            
        }

        
        public void AddListner()
        {
            self.btnClose.onClick.AddListener(() => { this.OnClickClose(); });
            self.btnCancel.onClick.AddListener(() => { OnClickCancele(); });
            self.btnConfirm.onClick.AddListener(() => { OnClickConfirm(); });
            self.btnAd.onClick.AddListener(() => { OnClickConfirm(); });
            self.btnShop.onClick.AddListener(() => { ClickShop(); });
        }

        public async void ClickShop()
        {
            if (coin_shop_reveive == 2 && this.tip.cancelCallback != null)
            {
                this.OnClickCancele();
                return;
            }

            if (HallInfoComponent.Instance.GetItemNum(KeyDefine.GameCoin) < costCoin)
            {
                 this.tip.itemNum = 0;
                 this.tip.itemId = 0;
                 UIHelper.Create(GlobalComponent.Instance.scene, UIType.UICharge, UILayer.Mid).Coroutine();
                 //ETCommonFunc.Instance.DelayAction(200, () =>
                 //{
                     UIHelper.Remove(self.ZoneScene(), UIType.UIPop).Coroutine();
                 //});
                return;
            }

            int goodId = tip.goodId;

            var activityGoodsConfig = ActivityGoodsConfigCategory.Instance.Get(goodId);
            //string[] goods0 = activityGoodsConfig.goods.Split(':');
            //Game.EventSystem.PublishAsync(new EventType.GetHeroRoadReward() { ZoneScene = this.self.ZoneScene(),}).Coroutine();
            

            string[] goods = activityGoodsConfig.goods.Split(':');
            int itemId0 = int.Parse(goods[0]);
            int itemNum0 = int.Parse(goods[1]) * this.tip.itemNum;
            HallInfoComponent.Instance.ChgItemNum(itemId0, 0, itemNum0);
            
            string[] cost = activityGoodsConfig.cost.Split(':');
            int itemId1 = int.Parse(cost[0]);
            int itemNum1 = int.Parse(cost[1]) * this.tip.itemNum;
            HallInfoComponent.Instance.ChgItemNum(itemId1, 0, itemNum1 * -1);
            

            EventDispatcher.PostEvent(EventName.UpdateGameCoin, this);
            EventDispatcher.PostEvent(EventName.UpdateItemInMB, this);

            
            HallInfoComponent hallInfoComponent = Game.Scene.GetComponent<HallInfoComponent>();
            hallInfoComponent.goodId = goodId;

            string[] goods1 = ActivityGoodsConfigCategory.Instance.Get(goodId).goods.Split(':');
            Log.Console("Goods:" + goods1);
            int itemId = int.Parse(goods1[0]);
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemId);
            if (itemConfig.type == ItemBigType.ItemType_Clothes)
            {
                await Game.EventSystem.PublishAsync(new EventType.GetSkin() { ZoneScene = GlobalComponent.Instance.scene, });
            }
            else
            {
                await Game.EventSystem.PublishAsync(new EventType.GetHeroRoadReward() { ZoneScene = GlobalComponent.Instance.scene, });
            }
                        
            HallHelper.BuyGoods(this.self.ZoneScene(), this.tip.goodId, () =>
            {
                
            }, false);
            //HallHelper.BuyGoods(GlobalComponent.Instance.scene, this.tip.goodId, () => { }, true);
            
            this.tip.itemNum = 0;
            this.tip.itemId = 0;
            this.tip.goodId = 0;
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(self.ZoneScene(), UIType.UIPop).Coroutine();
        }

        private async void  OnClickClose()
        {
            this.tip.itemNum = 0;
            this.tip.itemId = 0;
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            PlayAni(false);
            UIAniHelper.GetInstance().PlayUIDOFade(self.btnMask.gameObject.GetComponent<Image>(),0,false);
            await TimerComponent.Instance.WaitAsync(300);
            UIHelper.Remove(self.ZoneScene(), UIType.UIPop).Coroutine();
        }
        
        private  void  OnClickCancele()
        {
            this.tip.itemNum = 0;
            this.tip.itemId = 0;
            if (this.tip.cancelCallback != null)
            {
                this.tip.cancelCallback();
            }
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(self.ZoneScene(), UIType.UIPop).Coroutine();
        }
        
        private  void  OnClickConfirm()
        {
            this.tip.itemNum = 0;
            this.tip.itemId = 0;
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(self.ZoneScene(), UIType.UIPop).Coroutine();
            if (this.tip.sureCallback != null)
            {
                this.tip.sureCallback();
            }

            if (tip.isNet)
            {
                this.OnClickClose();
            }
        }
        
        private async void PlayAni(bool isOpen = true)
        {
            if (isOpen)
            {
                UIAniHelper.GetInstance().PlayUIAni(this.self.panel,0.2f);
                UIAniHelper.GetInstance().PlayUIDOFade(self.btnMask.gameObject.GetComponent<Image>(),0.6f);
            }
            else
            {
                UIAniHelper.GetInstance().PlayUIAni2(this.self.panel);
                UIAniHelper.GetInstance().PlayUIDOFade(self.btnMask.gameObject.GetComponent<Image>(),0.6f,false);
            }
            await ETTask.CompletedTask;
        }


        private void SelectGame(int gameId)
        {
            
        }
    }
}