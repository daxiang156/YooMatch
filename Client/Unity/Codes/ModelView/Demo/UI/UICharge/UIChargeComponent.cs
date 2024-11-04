using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIChargeComponent : Entity, IAwake,IDestroy
    {
        public Button btnReturn;
        public Text textGem;
        public Image imgGem;
        public Image imgGold;
        public Text textGold;
        public GameObject scrollContent;
        public GameObject goodItem;
        public GameObject FirstCharge;
        public GameObject ScrollDeposit;
        
        public object uiMbGold;
        public object uiGem;
        public GameObject btn_mask;
        public GameObject go_panel;
    }
}