using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIReviveComponent: Entity, IAwake
    {
        public Button btnClose;
        public Button btnBuy;
        public Button btnConfirm;
        public Button btnGiveUp;
        public Text text;
        public Text title;
        
        public Button btnAddDiamond;
        public Text textDiamond;
        //public GameObject netWork;

        public GameObject top;
        public SkeletonGraphic Icon;
    }
}