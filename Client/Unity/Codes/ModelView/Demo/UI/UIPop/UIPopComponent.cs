using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIPopComponent: Entity, IAwake
    {
        public Button btnClose;
        public Button btnCancel;
        public Button btnConfirm;
        
        public Button btnAd;
        public Button btnShop;
        public Text txtConfirm;
        public Text txtCancel;
        
        public Text text;
        public Text title;
        
        public Text itemNum;
        public Image ItemIcon;
        
        public GameObject netWork;
        public Button btnMask;
        public GameObject panel;

    }
}