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
    public class UINetWaitAwakeSystem : AwakeSystem<UINetWaitingComponent>
    {
        private UINetWaitingComponent self;
        private int waitTimeOut = 3;
        ETCancellationToken cancellationToken = null;
        public override void Awake(UINetWaitingComponent self)
        {
            this.self = self;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.wait = rc.Get<GameObject>("wait");
            self.AdLoading = rc.Get<GameObject>("AdLoading");
            self.normal = rc.Get<GameObject>("normal");
            self.wait.gameObject.SetActive(false);
            EventDispatcher.AddObserver(this, EventName.NetWaitUI, (object[] userInfo) =>
            {
                bool isShow = (bool)userInfo[0];
                waitTimeOut = 3;
                if (userInfo.Length > 1)
                    waitTimeOut = (int) userInfo[1];
                Log.Console("显示Loading：" + isShow);
                self.wait.gameObject.SetActive(isShow);
                if (!isShow)
                {
                    cancellationToken?.Cancel();
                    Log.Console("关闭之前弹窗");
                    cancellationToken = null;
                }

                if (this.waitTimeOut == 8 && isShow)
                {
                    self.normal.SetActive(false);
                    self.AdLoading.SetActive(true);
                }
                else
                {
                    self.normal.SetActive(true);
                    self.AdLoading.SetActive(false);
                }
                int resultRank = 0;
                if(userInfo.Length >= 2)
                    resultRank = (int)userInfo[1];
                if (isShow)
                    NetNoReturn(resultRank);
                return false;
            }, null);
            
            // EventDispatcher.AddObserver(this, EventName.SendGoogleAdsReward, (object[] userInfo) =>
            // {
            //     self.normal.SetActive(false);
            //     self.AdLoading.SetActive(false);
            //     return false;
            // }, null);
        }

        

        private async void NetNoReturn(int rank)
        {
            cancellationToken = new ETCancellationToken();
            await TimerComponent.Instance.WaitAsync(waitTimeOut * 1000, cancellationToken);
            if (self.wait.gameObject.activeSelf)
            {
                Log.Console(waitTimeOut + "：显示Loading超时：" + self.wait.gameObject.activeSelf);
                if (HallHelper.gateSession != null && !HallHelper.gateSession.IsDisposed && self.AdLoading.gameObject.activeSelf)
                {
                    // if (rank > 0)
                    //     EventDispatcher.PostEvent(EventName.NetWaitLongSingleResult, this);
                    // HallHelper.gateSession.Dispose();
                    // EventDispatcher.PostEvent(EventName.ConnectMySerInGame, this);
                    //await TimerComponent.Instance.WaitAsync(5000);
                    //NetNoReturn(rank) ;
                    if (this.self.AdLoading.activeSelf)
                    {
                        EventDispatcher.PostEvent(EventName.WaitAdTimeOut, null);
                        UIHelper.ShowTip(GlobalComponent.Instance.scene, "No Ad available. Try again later");
                    }
                }
                else
                {
                    if (rank > 0)
                    {
                        //EventDispatcher.PostEvent(EventName.NetWaitLongSingleResult, this, rank);
                    }
                }
                self.wait.gameObject.SetActive(false);
                waitTimeOut = 3;
            }
            await ETTask.CompletedTask;
        }
    }
    
    [ObjectSystem]
    public class UINetWaitIDestroySystem: DestroySystem<UINetWaitingComponent>
    {
        public override void Destroy(UINetWaitingComponent self)
        {
            EventDispatcher.RemoveObserver(EventName.NetWaitUI);
        }
    }
}