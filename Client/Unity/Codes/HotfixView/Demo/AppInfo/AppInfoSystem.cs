using ILRuntime.Runtime;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class AppInfoAwakeSystem: AwakeSystem<AppInfoComponent>
    {
        public override void Awake(AppInfoComponent self)
        {
            AppInfoComponent.Instance = self;
            self.AppStartSign = 0;
            self.loginStep = 0;
            self.canPvp = false;
            self.pvpWin = 0;
            this.SetGuide();
            self.grey = Resources.Load("Shader/Grey") as Material;
            //self.isForceEnterMainCity = false;
            ET.EventDispatcher.AddObserver(this, EventName.UpdateVersion, (object[] userInfo) =>
            {
                UIHelper.ShowPop(GlobalComponent.Instance.scene, LanguageComponent.Instance.GetLanguage(2180), () =>
                {
                    Application.OpenURL(AppInfoComponent.Instance.downloadUrl);
                }, false, () =>
                {
                    Application.OpenURL(AppInfoComponent.Instance.downloadUrl);
                });
                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, EventName.CloseWaitingUI, (object[] userInfo) =>
            {
                Log.Console("关闭Waiting界面");
                GameDataMgr.Instance.connectTime = 0;
//                EventDispatcher.PostEvent(EventName.IsShowJoyStick, this, true);
                EventDispatcher.PostEvent(EventName.ShowMainCity, this, true);
                EventDispatcher.PostEvent(EventName.LoadingCloseListen, this);
                EventDispatcher.PostEvent(EventName.GuideEnable, this, true);
                //Game.EventSystem.PublishAsync(new EventType.LoadingFinish() { Scene = GlobalComponent.Instance.scene}).Coroutine();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.removeInitScreen, (object[] userInfo) =>
            {
                if (HallInfoComponent.Instance.curMap == MapDefine.MBMore)
                {
                    self.closeMatching++;
                    Log.Console("匹配结束：" + AppInfoComponent.Instance.closeMatching);
                    if (self.closeMatching >= 2)
                    {
                        UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIMatching).Coroutine();
                        EventDispatcher.PostEvent(ETEventName.PvpMatchRemove, self);
                        self.closeMatching = 0;
                    }
                }
                else
                {
                    UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIConnect).Coroutine();
                }
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.CheckEnterMainCity, (object[] userInfo) =>
            {
                //int BackgroundRestart = FireBComponent.Instance.GetRemoteLong(FireBRemoteName.BackgroundRestart);
                //EventDispatcher.PostEvent(ETEventName.BackgroundRestart, self, BackgroundRestart);
                CheckIsEnterMainCity(self.ZoneScene());
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.ShowLoadingUI, (object[] userInfo) =>
            {
                if (GameDataMgr.Instance.PhotonDisconnected && GameDataMgr.Instance.singleplayer == false)
                {
                    Log.Console("显示加载页");
                    UIHelper.Create(GlobalComponent.Instance.scene, UIType.UILoading, UILayer.Mid).Coroutine();
                }
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.CheckEnterMainCityInEnterName, (object[] userInfo) =>
            {
                CheckIsEnterMainCity(self.ZoneScene(), true);
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.IsWaitSer, (object[] userInfo) =>
            {
                IsWaitSer();
                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, EventName.SyncItemToServer, (object[] userInfo) =>
            {
                int itemId = (int) userInfo[0];
                int itemNum = (int) userInfo[1];
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemId);
                if (itemConfig != null)
                {
                    if(itemConfig.type == ItemBigType.ItemType_1)
                        HallHelper.SyncItem(self.ZoneScene(), 0, 0, itemId, itemNum);
                    else
                        HallHelper.SyncItem(self.ZoneScene(), itemId, itemNum);
                }
                return false;
            }, null);
        }
        
        private void SetGuide()
        {
            //int step = AppInfoComponent.Instance.guideStep;
            // if (step > (int)GuideStep.MBItem1 && step < (int)GuideStep.End)
            //     AppInfoComponent.Instance.guideStep = (int)GuideStep.End;
            // if (step > (int)GuideStep.End && step < (int)GuideStep.EndRelast)
            //     AppInfoComponent.Instance.guideStep = (int)GuideStep.EndRelast;
            // if (step > (int)GuideStep.EndRelast && step < (int)GuideStep.EndUpdate)
            //     AppInfoComponent.Instance.guideStep = (int)GuideStep.EndUpdate;
            // if (step > (int)GuideStep.EndUpdate && step < (int)GuideStep.EndAddGrid)
            //     AppInfoComponent.Instance.guideStep = (int)GuideStep.EndAddGrid;
        }

        private async void IsWaitSer()
        {
            // await TimerComponent.Instance.WaitAsync(1500);
            // AppInfoComponent.Instance._isWaitingSer = false;
            await ETTask.CompletedTask;
        }

        public async void CheckIsEnterMainCity(Scene zoneScene, bool isStart = false)
        {
            if(AppInfoComponent.Instance.enterType != KeyDefine.enterMainCity)
                return;
            string sceneName = CommonFuc.CurScene();
//            Log.Console(sceneName);
            if (sceneName == "Start")
            {
                Log.Console("check enterCity:" + isStart.ToString());
                if (isStart)
                {
                    Log.Console("输入名字进入时间" + Time.time.ToString());
                    await TimerComponent.Instance.WaitAsync(10000);
                    if (CommonFuc.CurScene() != "Start")
                    {
                        return;
                    }
                }
                else
                {
                    if (UIHelper.Get(GlobalComponent.Instance.scene, UIType.UISelectHero) != null)
                    {
                        //AppInfoComponent.Instance.isForceEnterMainCity = true;
                        return;
                    }
                    if (UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIEnterName) != null)
                    {
                        //AppInfoComponent.Instance.isForceEnterMainCity = true;
                        return;
                    }
                }
                Log.Console("进入时间" + Time.time.ToString());
//                LoginHelper.JumpMainCity(GlobalComponent.Instance.scene);
                
                UI ui = UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIPop);
                if (ui != null)
                {
                    UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UISelectHero).Coroutine();
                }
            }
            await ETTask.CompletedTask;
        }
    }

    public static class AppInfoSystem
    {
        public static int GetSign(this AppInfoComponent self, int index, bool isAddTime)
        {
            int signSav = self.Sign;
            int sign = signSav / index;
            if(sign < 9 && isAddTime)
                self.Sign = signSav + index;
            return sign;
        }
    }

    [ObjectSystem]
    public class AppInfoDestroySystem: DestroySystem<AppInfoComponent>
    {
        public override void Destroy(AppInfoComponent self)
        {
            EventDispatcher.RemoveObserver(EventName.UpdateVersion);
            EventDispatcher.RemoveObserver(EventName.CloseWaitingUI);
            EventDispatcher.RemoveObserver(EventName.removeInitScreen);
            EventDispatcher.RemoveObserver(EventName.CheckEnterMainCity);
            EventDispatcher.RemoveObserver(EventName.CheckEnterMainCityInEnterName);
            EventDispatcher.RemoveObserver(EventName.ShowLoadingUI);
        }
    }
}