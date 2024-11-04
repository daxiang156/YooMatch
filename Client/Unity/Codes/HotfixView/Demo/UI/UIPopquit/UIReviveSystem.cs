using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using ILRuntime.Runtime;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class UIReviveAwakeSystem : AwakeSystem<UIReviveComponent>
    {
        private UIReviveComponent self;
        private TipInfoComponent tip;
        
        private int coin_shop_reveive = 1;
        public override void Awake(UIReviveComponent self)
        {
            this.self = self;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.btnClose = rc.Get<GameObject>("btnClose").GetComponent<Button>();
            self.btnBuy = rc.Get<GameObject>("btnBuy").GetComponent<Button>();
            self.btnAddDiamond = rc.Get<GameObject>("btnAddDiamond").GetComponent<Button>();
            self.btnConfirm = rc.Get<GameObject>("btnConfirm").GetComponent<Button>();
            self.btnGiveUp = rc.Get<GameObject>("btnGiveUp").GetComponent<Button>();
            self.text = rc.Get<GameObject>("text").GetComponent<Text>();
            self.title = rc.Get<GameObject>("title").GetComponent<Text>();
            self.textDiamond = rc.Get<GameObject>("textDiamond").GetComponent<Text>();

            self.top = rc.Get<GameObject>("top");
            self.Icon = rc.Get<GameObject>("Icon").GetComponent<SkeletonGraphic>();
            
            tip = Game.Scene.GetComponent<TipInfoComponent>();
            // if(tip.isNet)
            //     self.text.text = "";
            // else
            //     self.text.text = tip.text;
            
            SoundComponent.Instance.PlayActionSound("Music3","Tile_Full");
            
            if (tip.titleStr != "")
            {
                self.title.text = this.tip.titleStr;
            }
            this.AddListner();
            int diamondNum = HallInfoComponent.Instance.GetItemNum(KeyDefine.PlatFormCoin);
            self.textDiamond.text = diamondNum.ToString();
            UpdateDiamond();
            PlayAni();
            
            
            coin_shop_reveive = FireBComponent.Instance.GetRemoteLong(FireBRemoteName.coin_shop_reveive);
            self.btnBuy.transform.Find("diamond").gameObject.SetActive(coin_shop_reveive != 1);
            self.btnBuy.transform.Find("coin").gameObject.SetActive(coin_shop_reveive == 1);
            if (coin_shop_reveive == 1)
            {
                self.btnBuy.transform.Find("Text").GetComponent<Text>().text = "Play on     1000";
            }
            else
            {
                self.btnBuy.transform.Find("Text").GetComponent<Text>().text = "Play on     1";
            }

            //self.btnBuy.gameObject.SetActive(diamondNum < 1);
        }

        private async void PlayAni()
        {
            var pos = this.self.top.transform.localPosition;
            this.self.top.transform.localPosition += new Vector3(0, 300, 0);
            this.self.top.transform.DOLocalMove(pos, 1).SetEase(Ease.OutBounce);
            
            pos = this.self.btnBuy.transform.localPosition;
            this.self.btnBuy.transform.localPosition += new Vector3(300, 0,0);
            this.self.btnBuy.transform.DOLocalMove(pos, 0.5f).SetEase(Ease.Linear);
            
            pos = this.self.btnGiveUp.transform.localPosition;
            this.self.btnGiveUp.transform.localPosition += new Vector3(-300, 0,0);
            this.self.btnGiveUp.transform.DOLocalMove(pos, 0.5f).SetEase(Ease.Linear);
            
            this.self.Icon.color = new Color(1, 1, 1, 0);this.self.Icon.gameObject.SetActive(false);

            await TimerComponent.Instance.WaitAsync(1000);
            this.self.Icon.color = new Color(1, 1, 1, 0);this.self.Icon.gameObject.SetActive(true);
            this.self.Icon.DOFade(1, 1f);
        }

        private async void UpdateDiamond()
        {
            while (true)
            {
                await TimerComponent.Instance.WaitAsync(1000);
                if(self != null && this.self.title != null)
                    self.textDiamond.text = HallInfoComponent.Instance.GetItemNum(KeyDefine.PlatFormCoin).ToString();
            }
        }


        public void AddListner()
        {
            self.btnClose.onClick.AddListener(() => { OnClickClose(); });
            //self.btnCancel.onClick.AddListener(() => { OnClickCancele(); });
            self.btnConfirm.onClick.AddListener(() => { OnClickConfirm(); });
            self.btnBuy.onClick.AddListener(() =>
            {
                OnClickConfirm();
            });
            self.btnAddDiamond.onClick.AddListener(() =>
            {
                UIHelper.Create(GlobalComponent.Instance.scene, UIType.UICharge, UILayer.Mid).Coroutine();
            });
            self.btnGiveUp.onClick.AddListener(() => { OnClickClose(); });
        }

        private  void  OnClickClose()
        {
            if (this.tip.cancelCallback != null)
            {
                this.tip.cancelCallback();
            }
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
//            UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIRevive).Coroutine();
        }
        
        private  void  OnClickCancele()
        {
//            UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIRevive).Coroutine();
            if (this.tip.cancelCallback != null)
            {
                this.tip.cancelCallback();
            }

            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
//            UIHelper.Remove(self.ZoneScene(), UIType.UIRevive).Coroutine();
        }
        private  void  OnClickPlay()
        {
//            UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIRevive).Coroutine();
SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
  //          UIHelper.Remove(self.ZoneScene(), UIType.UIRevive).Coroutine();
            if (this.tip.sureCallback != null)
            {
                this.tip.sureCallback();
            }
        }
        
        private async void  OnClickConfirm()
        {
 //           UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIRevive).Coroutine();
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            if (this.coin_shop_reveive == 1)
            {
                if (HallInfoComponent.Instance.GetItemNum(KeyDefine.GameCoin) >= 1000)
                {
                    if (this.tip.sureCallback != null)
                    {
                        this.tip.sureCallback();
                    }
                }
                else
                {
                    await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UICharge, UILayer.Mid);
                    UIHelper.ShowTip(GlobalComponent.Instance.scene, "Not enough gold...Get some?");
                }
            }
            else
            {
                if (HallInfoComponent.Instance.GetItemNum(KeyDefine.PlatFormCoin) > 0)
                {
                    if (this.tip.sureCallback != null)
                    {
                        this.tip.sureCallback();
                    }
                }
                else
                {
                    await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UICharge, UILayer.Mid);
                    UIHelper.ShowTip(GlobalComponent.Instance.scene, "Not enough diamond...Get some?");
                }
            }
        }
    }
}