using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIChargeInGameComponent : Entity, IAwake,IDestroy
    {
        public Button btnClose;
        public Text txtDiamondNum;
        public Button btnBuy;
        public Text txtCost;
    }
}