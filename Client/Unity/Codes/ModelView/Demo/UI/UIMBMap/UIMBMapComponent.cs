using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIMBMapComponent: Entity, IAwake, IDestroy
    {
        public Button btnReturn;
        public GameObject gameSelf;
        public List<Button> BtnLvList = new List<Button>();
        public Transform mapContent;

        public Text textUpdate;
        public Text textAddgrid;
        public Text textRelast;
        public Text textUnlock;
        public Button btnUnlock;
        public Button btnMBMore;
        public GameObject spineLoading;

        public GameObject curLevel;
        public GameObject curLevelAni;
        
        public ScrollRect levelScrollRect;
        public object uiAniExpand;

    }
}