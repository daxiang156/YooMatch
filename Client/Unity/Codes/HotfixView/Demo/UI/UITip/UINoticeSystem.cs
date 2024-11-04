using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    [ObjectSystem]
    public class UINoticeAwakeSystem : AwakeSystem<UINoticeComponent>
    {
        private UINoticeComponent self;
        public override void Awake(UINoticeComponent self)
        {
            this.self = self;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.text = rc.Get<GameObject>("text").GetComponent<Text>();
            self.ImageTip = rc.Get<GameObject>("ImageTip").GetComponent<RectTransform>();
            
            TipInfoComponent data = Game.Scene.GetComponent<TipInfoComponent>();
            self.text.text = data.text;//"<color=#ff0000>Idle</color><color=#ffdf2cf> Passed the level 3!</color>";

            //Vector3 pos = self.ImageTip.transform.localPosition;
            if (UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIMB) != null)
            {
                this.self.ImageTip.anchorMin = new Vector2(0, 1);
                this.self.ImageTip.anchorMax = new Vector2(0, 1);
                self.ImageTip.transform.localPosition = new Vector3(700, 500, 0);
            }
            else
            {
                self.ImageTip.transform.localPosition = new Vector3(0, 400, 0);
            }

            this.waitClose();
        }
        
        public async void waitClose()
        {
            var timeComp = Game.Scene.GetComponent<TimerComponent>();
            await timeComp.WaitAsync(4000, null);
            await UIHelper.Remove(this.self.ZoneScene(), UIType.UINotice);
            GlobalComponent.Instance.noticeTime = 0;
        }
    }
}