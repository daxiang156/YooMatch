using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIGuideComponent: Entity, IAwake<object>, IDestroy
    {
        public Button btnPlay;
        public GameObject gameSelf;
        public GameObject spineLoading;
        public Button maskAll;
        public RectTransform mask;
        public RectTransform left;
        public RectTransform top;
        public RectTransform right;
        public RectTransform bottom;
        public RectTransform finger;
        public RectTransform btnMask;
        public RectTransform btnMask2;
        public RectTransform fruit;
        
        
        public GameObject spAddGrid;
        public GameObject spRelast;
        public GameObject spUpdate;
        public GameObject up1;
        public GameObject up2;
        public GameObject up3;
        public GameObject up4;
        
        public GameObject guidePeople;
        public Text txtGuide;

        //private int _curGuide = 1;

        public int curGuideIndex
        {
            set
            {
                AppInfoComponent.Instance.guideStep = value;
                Log.Console("当前步：" + AppInfoComponent.Instance.guideStep);
            }
            get
            {
                return AppInfoComponent.Instance.guideStep;
            }
        }
    }
}