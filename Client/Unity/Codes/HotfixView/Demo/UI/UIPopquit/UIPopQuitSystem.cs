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
    public class UIPopQuitAwakeSystem : AwakeSystem<UIPopQuitComponent>
    {
        private UIPopQuitComponent self;
        private TipInfoComponent tip;
        public override void Awake(UIPopQuitComponent self)
        {
            this.self = self;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.btnClose = rc.Get<GameObject>("btnClose").GetComponent<Button>();
            self.btnCancel = rc.Get<GameObject>("btnCancel").GetComponent<Button>();
            self.btnConfirm = rc.Get<GameObject>("btnConfirm").GetComponent<Button>();
            self.text = rc.Get<GameObject>("text").GetComponent<Text>();
            self.netWork = rc.Get<GameObject>("netWork");
            self.title = rc.Get<GameObject>("title").GetComponent<Text>();

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
            this.AddListner();
            
        }

        
        public void AddListner()
        {
            self.btnClose.onClick.AddListener(() => { OnClickClose(); });
            //self.btnCancel.onClick.AddListener(() => { OnClickCancele(); });
            self.btnCancel.onClick.AddListener(() => { OnClickClose(); });
            self.btnConfirm.onClick.AddListener(() => { OnClickConfirm(); });
        }

        private  void  OnClickClose()
        {
            if (this.tip.cancelCallback != null)
            {
                this.tip.cancelCallback();
            }
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(self.ZoneScene(), UIType.UIPopQuit).Coroutine();
        }
        
        private  void  OnClickCancele()
        {
            if (this.tip.cancelCallback != null)
            {
                this.tip.cancelCallback();
            }

            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(self.ZoneScene(), UIType.UIPopQuit).Coroutine();
        }
        private  void  OnClickPlay()
        {
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(self.ZoneScene(), UIType.UIPopQuit).Coroutine();
            if (this.tip.sureCallback != null)
            {
                this.tip.sureCallback();
            }
        }
        
        private  void  OnClickConfirm()
        {
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(self.ZoneScene(), UIType.UIPopQuit).Coroutine();
            if (this.tip.sureCallback != null)
            {
                this.tip.sureCallback();
            }
        }

        
        private void SelectGame(int gameId)
        {
            
        }
    }
}