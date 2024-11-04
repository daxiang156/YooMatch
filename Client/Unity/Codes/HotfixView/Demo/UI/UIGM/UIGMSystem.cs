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
    public class UIGMAwakeSystem : AwakeSystem<UIGMComponent>
    {
        private UIGMComponent self;
        public override void Awake(UIGMComponent self)
        {
            this.self = self;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.btnClose = rc.Get<GameObject>("btnClose").GetComponent<Button>();
            self.btnEnter = rc.Get<GameObject>("btnEnter").GetComponent<Button>();
            self.InputField = rc.Get<GameObject>("InputField").GetComponent<InputField>();

         
            this.AddListner();
            
        }

        
        public void AddListner()
        {
            self.btnClose.onClick.AddListener(() => { OnClickClose(); });
            self.btnEnter.onClick.AddListener(() => { OnClickEnter(); });
        }

        private  void  OnClickClose()
        {
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(self.ZoneScene(), UIType.UIGM).Coroutine();
        }
        
        private  void  OnClickEnter()
        {
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            string text = this.self.InputField.text;
            HallHelper.GM(this.self.ZoneScene(),text);
        }
    }
}