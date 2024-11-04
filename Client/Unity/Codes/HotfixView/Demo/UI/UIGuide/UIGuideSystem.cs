using System;
using DG.Tweening;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class UIGuideAwakeSystem : AwakeSystem<UIGuideComponent,object>
    {
        private UIGuideComponent self;
        private UIGuidData guidData;
        public override void Awake(UIGuideComponent self,object data)
        {
            this.self = self;
            guidData = (UIGuidData) data;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.gameSelf = self.GetParent<UI>().GameObject;
            self.btnPlay = rc.Get<GameObject>("btnPlay").GetComponent<Button>();
            self.spineLoading = rc.Get<GameObject>("spineLoading");
            self.mask = rc.Get<GameObject>("mask").GetComponent<RectTransform>();
            self.maskAll = rc.Get<GameObject>("maskAll").GetComponent<Button>();
            self.maskAll.enabled = false;
            self.top = rc.Get<GameObject>("top").GetComponent<RectTransform>();
            self.left = rc.Get<GameObject>("left").GetComponent<RectTransform>();
            self.right = rc.Get<GameObject>("right").GetComponent<RectTransform>();
            self.bottom = rc.Get<GameObject>("bottom").GetComponent<RectTransform>();
            self.finger = rc.Get<GameObject>("finger").GetComponent<RectTransform>();
            self.btnMask = rc.Get<GameObject>("btnMask").GetComponent<RectTransform>();
            self.btnMask2 = rc.Get<GameObject>("btnMask2").GetComponent<RectTransform>();
            self.txtGuide = rc.Get<GameObject>("txtGuide").GetComponent<Text>();
            self.guidePeople = rc.Get<GameObject>("guidePeople");
            self.fruit = rc.Get<GameObject>("fruit").GetComponent<RectTransform>();
            
            self.spAddGrid = rc.Get<GameObject>("spAddGrid");
            self.spRelast = rc.Get<GameObject>("spRelast");
            self.spUpdate = rc.Get<GameObject>("spUpdate");
            self.spAddGrid.SetActive(false);
            self.spRelast.SetActive(false);
            self.spUpdate.SetActive(false);
            self.up1 = rc.Get<GameObject>("up1");
            self.up2 = rc.Get<GameObject>("up2");
            self.up3 = rc.Get<GameObject>("up3");
            self.up4 = rc.Get<GameObject>("up4");
        
            //self.mask.gameObject.SetActive(false);
            self.guidePeople.SetActive(false);
            self.spineLoading.SetActive(false);
            self.curGuideIndex = AppInfoComponent.Instance.guideStep;
            this.AddListner();
            AppInfoComponent.Instance.guiding = true;
            if (AppInfoComponent.Instance.guideStep >= (int)GuideStep.MBItem1)
            {
                this.self.btnPlay.gameObject.SetActive(false);
                //GuideConfig config = GuideConfigCategory.Instance.Get(this.self.curGuideIndex);
                //Log.Error("curGuideIndex++?");
                //if (config.fouceGuide > 0)
                {
                    //Log.Error("curGuideIndex++");
                    //self.curGuideIndex = self.curGuideIndex + 1;
                }
                self.up1.gameObject.SetActive(false);
                self.up4.gameObject.SetActive(false);
                this.GuideClick();
            }
            else
            {
                self.maskAll.gameObject.SetActive(true);
                self.maskAll.enabled = true;
                self.maskAll.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.01f);
                this.self.btnPlay.gameObject.SetActive(true);
                var people = self.btnPlay.transform.Find("guidePeople2");
                // if (people != null && GameDataMgr.Instance.Platflam() == PlatForm.IOS)
                // {
                //     people.gameObject.SetActive(false);
                // }else if(people != null)
                     people.gameObject.SetActive(true);
                SkipStepTimeOut();
            }
            // if(ETCommonFunc.IsTablet())
            //     self.gameSelf.SetActive(false);
        }

        private async void SkipStepTimeOut()
        {
            await TimerComponent.Instance.WaitAsync(3500);
            if (AppInfoComponent.Instance.guideStep == (int)GuideStep.MainCityFirst)
                this.MaskClick();
        }

        public void AddListner()
        {
            self.btnPlay.onClick.AddListener(() => { OnClickEnter(); });
            this.self.maskAll.onClick.AddListener(() =>
            {
                MaskClick();
            });
            EventDispatcher.AddObserver(this, EventName.ShowMainCity, (object[] userInfo) =>
            {
                if(self != null && self.gameSelf)
                    self.gameSelf.SetActive(true);
                else
                {
                    Log.Console("关闭Waiting界面报错，self == null");
                }
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.MBAgainAlready, (object[] info) =>
            {
                Log.Console("已经摆好道具2");
                this.self.spineLoading.SetActive(false);
                this.self.btnPlay.gameObject.SetActive(false);
//                UIHelper.Remove(self.ZoneScene(), UIType.UIGuide).Coroutine();
                EventDispatcher.PostEvent(EventName.GuideClick, this);
                
                
                EventDispatcher.PostEvent(EventName.PhotonLeaveMainCity, this);
                EventDispatcher.PostEvent(EventName.IsShowTpsObj, this, false, false);
                
                SceneChangeHelper.SceneChangeTo(this.self.ZoneScene(), "Init", 2).Coroutine();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.GuideClick, (object[] info) =>
            {
                GuideConfig config = GuideConfigCategory.Instance.Get(this.self.curGuideIndex);
                //if(config.fouceGuide > 0)
                    self.curGuideIndex = self.curGuideIndex + 1;
                Log.Console("进入引导：" + this.self.curGuideIndex);
                this.GuideClick();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.GuideEnable, (object[] info) =>
            {
                bool isEnable = (bool) info[0];
                if(self != null && this.self.gameSelf != null)
                    self.gameSelf.SetActive(isEnable);
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.FirstOrderGuide, (object[] info) =>
            {
                if (this.self == null || this.self.gameSelf == null) return false;
                bool isEnable = (bool) info[0];
                this.self.up1.SetActive(false);
                this.self.up4.SetActive(false);
                this.self.up2.SetActive(isEnable);
                this.self.up3.SetActive(isEnable);
                return false;
            }, null);
        }

        private async void MaskClick()
        {
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            // if (this.self.curGuideIndex == (int) GuideStep.AddGrid ||
            //     this.self.curGuideIndex == (int) GuideStep.UpdateItem ||
            //     this.self.curGuideIndex == (int) GuideStep.Relast)
            // {
            //     await TimerComponent.Instance.WaitAsync(1000);
            // }
            this.self.btnPlay.gameObject.SetActive(false);
            EventDispatcher.PostEvent(EventName.GuideClick, this);
            await ETTask.CompletedTask;
        }

        private  void  OnClickEnter()
        {
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            EventDispatcher.PostEvent(EventName.GuideClick, this);
            this.self.btnPlay.gameObject.SetActive(false);
//            EventDispatcher.PostEvent(EventName.ChangeItemNum, null, KeyDefine.entityId, 0, -1);
//            UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMB, UILayer.Mid).Coroutine();
            //UIHelper.Remove(this.self.ZoneScene(), UIType.UIGuide).Coroutine();
            
            // self.spineLoading.SetActive(true);
            // HallHelper.EnterFunGame(this.self.ZoneScene(), 1, FireBComponent.Instance.GetRemoteString(FireBRemoteName.free_item_level),
            //     FireBComponent.Instance.GetRemoteString(FireBRemoteName.level_difficulty), null);
            // HallHelper.EnterFunGame(this.self.ZoneScene(), 1, () =>
            // {
            //     //this.self.spineLoading.SetActive(false);
            //     //this.self.btnPlay.gameObject.SetActive(false);
            //     //EventDispatcher.PostEvent(EventName.GuideClick, this);
            // });
            
        }

        SkeletonGraphic sp = null;
        private void GuideClick()
        {
            //PlayerPrefs.SetInt("GuideStep", this.self.curGuideIndex);
            AppInfoComponent.Instance.guideStep = self.curGuideIndex;
            Log.Console("新手引导：" + this.self.curGuideIndex);
            
            if (this.self.curGuideIndex == (int) GuideStep.End 
                    || this.self.curGuideIndex == (int) GuideStep.EndRelast
                    || this.self.curGuideIndex == (int) GuideStep.EndUpdate
                    || this.self.curGuideIndex == (int) GuideStep.EndAddGrid
                    || this.self.curGuideIndex == (int) GuideStep.EndBlind
                    || this.self.curGuideIndex == (int) GuideStep.EndIce
                    || this.self.curGuideIndex == (int) GuideStep.End3)
            {
                UIHelper.Remove(this.self.ZoneScene(), UIType.UIGuide).Coroutine();
                AppInfoComponent.Instance.guiding = false;
                //HallHelper.SyncGuide(GlobalComponent.Instance.scene, self.curGuideIndex);
                guidData?.callBack?.Invoke(this.self.curGuideIndex);
                return;
            }
            GuideConfig config = GuideConfigCategory.Instance.Get(this.self.curGuideIndex);
            
            this.self.fruit.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, config.btnWide);
            this.self.fruit.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, config.btnHeight);
            SpecialStep();
            
            this.self.fruit.transform.localPosition = new Vector3(config.posX + 0.5f, config.posY, 0);
            if (this.self.curGuideIndex == (int) GuideStep.AddGrid ||
                this.self.curGuideIndex == (int) GuideStep.UpdateItem ||
                this.self.curGuideIndex == (int) GuideStep.Relast)
            {
                this.self.guidePeople.transform.Find("Image").GetComponent<Image>().enabled = false;
                this.self.guidePeople.transform.Find("ItemImg").gameObject.SetActive(true);
                SoundComponent.Instance.PlayActionSound("Music2","ItemUnlock");
            }
            if (config.btnWide != 0 && config.btnHeight != 0)
            {
                if (config.anchors == 3)
                {
                    this.self.mask.anchorMin = new Vector2(1, 1);
                    this.self.mask.anchorMax = new Vector2(1, 1);
                    this.self.finger.anchorMax = new Vector2(1, 1);
                    this.self.finger.anchorMin = new Vector2(1, 1);
                }
                if (config.anchors == 8)
                {
                    this.self.mask.anchorMin = new Vector2(0.5f, 0);
                    this.self.mask.anchorMax = new Vector2(0.5f, 0);
                    this.self.finger.anchorMax = new Vector2(0.5f, 0);
                    this.self.finger.anchorMin = new Vector2(0.5f, 0);
                    this.self.fruit.anchorMax = new Vector2(0.5f, 0);
                    this.self.fruit.anchorMin = new Vector2(0.5f, 0);
                }
                else if (config.anchors == 1)
                {
                    this.self.mask.anchorMin = new Vector2(0, 1);
                    this.self.mask.anchorMax = new Vector2(0, 1);
                    this.self.finger.anchorMax = new Vector2(0, 1);
                    this.self.finger.anchorMin = new Vector2(0, 1);
                }
                else if (config.anchors == 2)
                {
                    this.self.mask.anchorMin = new Vector2(0.5f, 1);
                    this.self.mask.anchorMax = new Vector2(0.5f, 1);
                    this.self.finger.anchorMax = new Vector2(0.5f, 1);
                    this.self.finger.anchorMin = new Vector2(0.5f, 1);
                }
                else if (config.anchors == 5)
                {
                    this.self.finger.anchorMin = new Vector2(0.5f, 0.5f);
                    this.self.finger.anchorMax = new Vector2(0.5f, 0.5f);
                    this.self.mask.anchorMin = new Vector2(0.5f, 0.5f);
                    this.self.mask.anchorMax = new Vector2(0.5f, 0.5f);
                }
                else
                {
                    this.self.finger.anchorMin = new Vector2(1, 1);
                    this.self.finger.anchorMax = new Vector2(0, 0);
                    this.self.mask.anchorMin = new Vector2(1, 1);
                    this.self.mask.anchorMax = new Vector2(0, 0);
                }
                this.self.maskAll.gameObject.SetActive(false);
                if (config.fouceGuide > 0)
                {
                    this.self.mask.gameObject.SetActive(true);
                    this.self.top.localPosition = new Vector3(config.posX, config.posY + config.btnHeight / 2f, 0);
                    this.self.top.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, config.btnWide);
                    //this.self.top.rect.Set(0, config.posY + config.btnHeight / 2f, config.btnWide, 1080);
                    this.self.bottom.localPosition = new Vector3(config.posX, config.posY - config.btnHeight / 2f, 0);
                    this.self.bottom.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, config.btnWide);
                    //this.self.bottom.rect.Set(0, config.posY - config.btnHeight / 2f, config.btnWide, 1080);
                    this.self.right.localPosition = new Vector3(config.posX + config.btnWide / 2f, 0, 0);
                    this.self.left.localPosition = new Vector3(config.posX - config.btnWide / 2f, 0, 0);
                }
                else
                {
                    this.self.mask.gameObject.SetActive(false);
                }

                self.btnMask2.gameObject.SetActive(false);
                

                // if (this.self.curGuideIndex == (int) GuideStep.AddGridUse ||
                //     this.self.curGuideIndex == (int) GuideStep.UpdateUse ||
                //     this.self.curGuideIndex == (int) GuideStep.RelastUse)
                // {
                //     this.self.btnMask.gameObject.SetActive(true);
                //     this.self.btnMask.localPosition = new Vector3(config.posX, config.posY, 0);
                //     this.self.btnMask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, config.btnWide);
                //     this.self.btnMask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, config.btnHeight);
                //     sp.AnimationState.SetAnimation(0, "animation3", false);
                //     sp.transform.DOMove(this.self.btnMask.position, 1).OnComplete(() =>
                //     {
                //         sp.gameObject.SetActive(false);
                //         int index = 1;
                //         if (self.curGuideIndex == (int) GuideStep.AddGridUse) index = 2;
                //         if (self.curGuideIndex == (int) GuideStep.UpdateUse) index = 3;
                //         EventDispatcher.PostEvent(ETEventName.ShowItemUnlock, this.self, index);
                //     });
                // }
                if (this.self.curGuideIndex == (int) GuideStep.Task)
                {
                    //RectTransform btnMask2 = this.self.gameSelf.transform.Find("mask/btnMask2").GetComponent<RectTransform>();
                    this.self.btnMask2.gameObject.SetActive(true);
                    this.self.btnMask2.localPosition = new Vector3(config.posX, config.posY, 0);
                    //this.self.btnMask2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, config.btnWide);
                    //this.self.btnMask2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, config.btnHeight);
                }

                this.self.finger.GetComponent<RectTransform>().anchoredPosition = new Vector3(config.posX + 50, config.posY - 100, 0);
                this.self.finger.gameObject.SetActive(true);
                
            }
            else
            {
                this.self.maskAll.gameObject.SetActive(true);
                this.self.mask.gameObject.SetActive(false);
                this.self.maskAll.enabled = true;
                this.self.finger.gameObject.SetActive(false);
            }

            this.self.guidePeople.SetActive(config.guidePeople > 0);
            if (config.guidePeople != 0)
            {
                Log.Console("languageSelect: " + GameDataMgr.Instance.languageSelect);
                if((int)GameDataMgr.Instance.languageSelect == (int)LanguageSelect2.Indonesia)
                    this.self.txtGuide.text = config.guideTxtIn;
                else if((int)GameDataMgr.Instance.languageSelect == (int)LanguageSelect2.India)
                    this.self.txtGuide.text = config.guideTxtHindi;
                else if((int)GameDataMgr.Instance.languageSelect == (int)LanguageSelect2.Spanish)
                    this.self.txtGuide.text = config.guideTxtSpanish;
                else if((int)GameDataMgr.Instance.languageSelect == (int)LanguageSelect2.Thailand)
                    this.self.txtGuide.text = config.guideTxtThailand;
                else if((int)GameDataMgr.Instance.languageSelect == (int)LanguageSelect2.Japan)
                    this.self.txtGuide.text = config.guideTxtJP;
                else if((int)GameDataMgr.Instance.languageSelect == (int)LanguageSelect2.Vietnam)
                    this.self.txtGuide.text = config.guideTxtVietnamese;
                else if((int)GameDataMgr.Instance.languageSelect == (int)LanguageSelect2.Brazil)
                    this.self.txtGuide.text = config.guideTxtBrazil;
                else
                {
                    this.self.txtGuide.text = config.guideTxtEn;
                }

                this.self.guidePeople.GetComponent<Image>().enabled = config.guidePeople != 2;
                if (config.guidePeople == 2)
                {
                    RectTransform guidePe = this.self.guidePeople.GetComponent<RectTransform>();
                    guidePe.localPosition = new Vector3(config.posX + 50, config.posY - 100, 0);
                    guidePe.anchorMax = new Vector2(0.5f, 1);
                    guidePe.anchorMin = new Vector2(0.5f, 1);
                    // guidePe.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, config.btnWide);
                    // guidePe.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, config.btnHeight);
                }else if (Math.Abs(config.guidePeople) > 10)
                {
                    self.guidePeople.SetActive(true);
                    self.guidePeople.transform.localPosition = new Vector3(self.guidePeople.transform.localPosition.x, config.guidePeople, 0);
                }
            }
        }

        private void SpecialStep()
        {
            Log.Console("curGuideIndex:" + self.curGuideIndex);
            
            switch (self.curGuideIndex)
            {
                case (int) GuideStep.Relast:
                    this.self.spRelast.SetActive(true);
                    sp = this.self.spRelast.GetComponent<SkeletonGraphic>();
                    sp.AnimationState.SetAnimation(0, "animation1", false);
                    ETCommonFunc.Instance.DelayAction(2000, () => { sp.AnimationState.SetAnimation(0, "animation2", true); }).Coroutine();
                    break;
                case (int) GuideStep.UpdateItem:
                    this.self.spUpdate.SetActive(true);
                    sp = this.self.spUpdate.GetComponent<SkeletonGraphic>();
                    ETCommonFunc.Instance.DelayAction(2000, () => { sp.AnimationState.SetAnimation(0, "animation2", true); }).Coroutine();
                    break;
                case (int) GuideStep.AddGrid:
                    //this.self.spAddGrid.SetActive(true);
                    // sp = this.self.spAddGrid.GetComponent<SkeletonGraphic>();
                    // ETCommonFunc.Instance.DelayAction(2000, () => { sp.AnimationState.SetAnimation(0, "animation2", true); }).Coroutine();
                    break;
                case (int) GuideStep.AddGridUse:
                    self.fruit.gameObject.SetActive(true);
                    break;
                case (int) GuideStep.UpdateUse:
                    self.fruit.gameObject.SetActive(true);
                    break;
                case (int) GuideStep.MainCityFirst:
                    self.fruit.gameObject.SetActive(true);
                    this.self.up1.SetActive(true);
                    this.self.up2.SetActive(false);
                    this.self.up3.SetActive(false);
                    this.self.up4.SetActive(true);
                    break;
                case (int) GuideStep.MBItem1:
                    self.fruit.gameObject.SetActive(true);
                    this.self.up1.SetActive(true);
                    this.self.up2.SetActive(false);
                    this.self.up3.SetActive(false);
                    this.self.up4.SetActive(true);
                    break;
                case (int) GuideStep.MBItem2:
                    self.up1.transform.Find("upNum").GetComponent<Text>().text = "2";
                    break;
                case (int) GuideStep.MBItem3:
                    self.up4.transform.Find("upNum").GetComponent<Text>().text = "2";
                    break;
                case (int) GuideStep.MBItem4:
                    self.up4.transform.Find("upNum").GetComponent<Text>().text = "1";
                    //self.up4.transform.Find("gou").gameObject.SetActive(true);
                    break;
                case (int) GuideStep.MBItem21:
                    self.up1.transform.Find("upNum").GetComponent<Text>().text = "1";
                    // ETCommonFunc.Instance.DelayAction(500, () =>
                    // {
                    //     self.up1.transform.Find("gou").gameObject.SetActive(false);
                    //     self.up2.transform.Find("gou").gameObject.SetActive(false);
                    //     this.self.up1.transform.Find("btnItem").GetComponent<Image>().sprite =
                    //             Resources.Load("MBitem/item4", typeof (Sprite)) as Sprite;
                    //     this.self.up2.transform.Find("btnItem").GetComponent<Image>().sprite =
                    //             Resources.Load("MBitem/item2", typeof (Sprite)) as Sprite;
                    //     self.up1.transform.Find("upNum").GetComponent<Text>().text = "2";
                    //     self.up2.transform.Find("upNum").GetComponent<Text>().text = "3";
                    // });
                    break;
                case (int) GuideStep.MBItem22:
                    self.up1.transform.Find("upNum").GetComponent<Text>().text = "0";
                    self.up1.transform.Find("gou").gameObject.SetActive(true);
                    break;
                case (int) GuideStep.MBItem23:
                    // this.self.up1.SetActive(false);
                    // this.self.up2.SetActive(true);
                    // this.self.up3.SetActive(true);
                    // this.self.up4.SetActive(false);
                    self.up4.transform.Find("upNum").GetComponent<Text>().text = "0";
                    self.up4.transform.Find("gou").gameObject.SetActive(true);
                    break;
                case (int) GuideStep.MBItem24:
                    self.up3.transform.Find("upNum").GetComponent<Text>().text = "1";
                    break;
                case (int) GuideStep.MBItem25:
                    self.up3.transform.Find("upNum").GetComponent<Text>().text = "0";
                    self.up3.transform.Find("gou").gameObject.SetActive(true);
                    break;
                case (int) GuideStep.UseKey:
                    this.self.fruit.gameObject.SetActive(true);
                    this.self.gameSelf.transform.Find("order").gameObject.SetActive(false);
                    break; 
                case (int) GuideStep.UseBlind:
                    this.self.fruit.gameObject.SetActive(true);
                    this.self.gameSelf.transform.Find("order").gameObject.SetActive(false);
                    break;
                case (int) GuideStep.UseIce:
                    this.self.fruit.gameObject.SetActive(true);
                    this.self.gameSelf.transform.Find("order").gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
    
    public class UIGuideDestroySystem: DestroySystem<UIGuideComponent>
    {
        public override void Destroy(UIGuideComponent self)
        {
            Log.Console("去掉引导界面");
            //EventDispatcher.RemoveObserver(EventName.ShowMainCity);
            EventDispatcher.RemoveObserver(EventName.MBAgainAlready);
            EventDispatcher.RemoveObserver(EventName.GuideClick);
            EventDispatcher.RemoveObserver(EventName.GuideEnable);
        }
    }
}