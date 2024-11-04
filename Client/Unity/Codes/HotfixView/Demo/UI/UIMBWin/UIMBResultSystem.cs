using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class UIMBResultSystem : AwakeSystem<UIMBResultComponent>
    {
        private UIMBResultComponent self;
        public override void Awake(UIMBResultComponent self)
        {
            
            this.self = self;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.btnContinue = rc.Get<GameObject>("btnContinue").GetComponent<Button>();
            self.win = rc.Get<GameObject>("win");
            self.lose = rc.Get<GameObject>("lose");
            this.AddListner();
            this.Init();
        }

        private async void Init()
        {
            SoundComponent.Instance.StopBgMusic();
            if (MBDataComponent.Instance.isWin)
            {
                SoundComponent.Instance.PlayActionSound("Music1","win_1");
            }
            else
            {
                SoundComponent.Instance.PlayActionSound("Music1","fail");
            }
            
            self.win.SetActive(MBDataComponent.Instance.isWin);
            self.lose.SetActive(!MBDataComponent.Instance.isWin);
            await TimerComponent.Instance.WaitAsync(2000);
            if (self != null && this.self.win != null)
                OnClickContinue(false);
        }

        public async void AddListner()
        {
            await TimerComponent.Instance.WaitAsync(600);
            this.self.btnContinue.onClick.AddListener(()=>{this.OnClickContinue();});
        }

        private async void OnClickContinue(bool isMusic = true)
        {
            if(isMusic)
                SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            
            // Log.Console("销毁6：EventName.SendGoogleAdsReward");
            // EventDispatcher.RemoveObserver(EventName.SendGoogleAdsReward);
            // if(AppInfoComponent.Instance.guiding)
            //     UIHelper.Remove(this.self.ZoneScene(), UIType.UIGuide).Coroutine();

            EventDispatcher.PostEvent(EventName.ChangeItemNum, null, KeyDefine.entityId, 0, -1);
            EventDispatcher.PostEvent(EventName.MBAgain, this);
            
            UIHelper.Remove(this.self.ZoneScene(), UIType.UIMBResult).Coroutine();
            await TimerComponent.Instance.WaitAsync(50);
            SoundComponent.Instance.ReplayBgMusic();
            await ETTask.CompletedTask;
        }
    }
}