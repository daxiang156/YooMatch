using ET.Data;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIUnLockTipsSystem : AwakeSystem<UIUnLockTipsComponent>
    {
        private UIUnLockTipsComponent self;
        private ReferenceCollector rc;
        private int unLockCost;
        public override void Awake(UIUnLockTipsComponent self)
        {
            this.self = self;
            rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.btnConfirm = this.rc.Get<GameObject>("btnConfirm").GetComponent<Button>();
            self.btnConfirm.onClick.AddListener(OnConfirm);
            self.btnCancle = this.rc.Get<GameObject>("btnCancle").GetComponent<Button>();
            self.btnCancle.onClick.AddListener(OnCancle);

            self.txtPriceTips = this.rc.Get<GameObject>("txtPriceTips").GetComponent<Text>();            
            
            string str = "Extra Slot For ${0}?";
            unLockCost = InitConfigCategory.Instance.Get(1).gridCost;
            self.txtPriceTips.text = string.Format(str, this.unLockCost);
        }

        private void OnConfirm()
        {
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            if (UIMBDataHelper.Instance.MoneyValue < this.unLockCost)
            {
                OnCancle();
                UIHelper.ShowTip(GlobalComponent.Instance.scene, "Not enough money");
                return;
            }
            UIMBDataHelper.Instance.SaveGrid(UIMBDataHelper.AddGridId);
            UIMBDataHelper.Instance.MonyValueCtr(this.unLockCost,2);
            EventDispatcher.PostEvent(ETEventName.SecondTipYes, self);
            OnCancle();
        }

        private void OnCancle()
        {
            SoundComponent.Instance.PlayActionSound("Music3","commonBtn");
            UIHelper.Remove(GlobalComponent.Instance.scene,UIType.UIUnLockTips).Coroutine();
        }
    }
}