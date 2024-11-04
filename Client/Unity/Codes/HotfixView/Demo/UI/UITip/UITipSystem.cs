using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class UITipAwakeSystem : AwakeSystem<UITipComponent>
    {
        private UITipComponent self;
        public override void Awake(UITipComponent self)
        {
            this.self = self;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.text = rc.Get<GameObject>("text").GetComponent<Text>();
            
            var tip = Game.Scene.GetComponent<TipInfoComponent>();
            if(tip != null)
                self.text.text = tip.text;

            this.waitClose();
        }
        
        public async void waitClose()
        {
            var timeComp = Game.Scene.GetComponent<TimerComponent>();
            await timeComp.WaitAsync(2500, null);
            self.text.transform.parent.DOLocalMoveY(900, 0.2f).onComplete = () =>
            {
                UIHelper.Remove(this.self.ZoneScene(), UIType.UITip).Coroutine();
            };
        }
    }
}