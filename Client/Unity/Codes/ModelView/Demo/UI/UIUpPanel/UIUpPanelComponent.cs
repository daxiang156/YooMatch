using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIUpPanelComponent: Entity, IAwake, IDestroy,IUpdate
    {
        public Button btnAd;
        public Button btnShop;
        public Button btnCharge;
        public Button btnSet;
        public GameObject goGold;
        public object headInfo;
        public object resourceInfo;
        public object uiAniExpand;
    }
}