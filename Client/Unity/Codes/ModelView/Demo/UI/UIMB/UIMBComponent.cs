using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Spine.Unity;

namespace ET
{
    public class UIMBComponent: Entity, IAwake, IDestroy, IUpdate
    {
        public GameObject gameSelf;
        public GameObject menu;
        public GameObject oneItem;
       // public Button btnItem;
        public Button btnReturn;
        public Button btnReLast;
        public Button btnAddGrid;
        public Button btnUpdate;
        public Text textRelast;
        public Text textAddgrid;
        public Text textUpdate;
        
        // public Button btnReLastAd;
        // public Button btnAddGridAd;
        // public Button btnUpdateAd;
        
        public Button btnShop;
        public Button btnQuest;
        public Button btnLucky;
        
        public Slider percent;
        public Text percentTxt;
        public Text textEntity;
        public Text txtLevel;
        //public Text textCoin;
        
//        public SkeletonGraphic countDownsp;
//        public Image taskPercent;
//        public Image BG;

        public GameObject bottomItem;
 //       public GameObject MB_Star;
//        public GameObject lastGridRed;
        public SkeletonGraphic spineResort;
//        public Text txtGuide;
        
        public GameObject addGridTips;
        public Image extraGridBg;
        
        public GameObject greyUpdate;
        public GameObject greyAddGrid;
        public GameObject greyRelast;
        public GameObject free;
        public GameObject mask;
        public GameObject particleGold;
        public GameObject particleFruit;
        public GameObject fruitParent;
        public Transform overParent;

        public Image imgBlindBox;
        public Image ItemLock;
        public Image ItemKey;
        public Image ItemIce;
        public Transform upGrid;
        public Transform upGrid1;
        public Transform upGrid2;
        public GameObject goUpNum;

        public Text txtAllFruit;
        public Text txtNowFruit;

        public SkeletonGraphic qipao;
        public GameObject paopaobaozha;
        public Image flyShader;

        public SkeletonGraphic lockSpine;
        public SkeletonGraphic iceSpine;
//        public GameObject fruitParent2;
        
        public List<Transform> downPosList = new List<Transform>();
        public List<Transform> itemGridPosList = new List<Transform>();
        public List<UIMBItemComponent> addList = new List<UIMBItemComponent>();
        public List<UIMBItemComponent> downList = new List<UIMBItemComponent>();
        public Dictionary<int, UIMBItemComponent> reviveList = new Dictionary<int,UIMBItemComponent>();
        
        
        public List<UpItemGroup> upList = new List<UpItemGroup>();
        /// <summary>
        /// 所有水果
        /// </summary>
        public Dictionary<int, UIMBItemComponent> allGridDic = new Dictionary<int, UIMBItemComponent>();
        /// <summary>
        /// 格子Id，格子里面水果列表
        /// </summary>
        public Dictionary<int, Dictionary<int, UIMBItemComponent>> allOneGridDic = new Dictionary<int, Dictionary<int,UIMBItemComponent>>();

        public GameObject goCoin;
        public Button btnSetUp;
        public GameObject goSetPanel;
        public Transform peoples;

        public object uiSetUpPanel;
        public object uiAniExpand;

        public object modelMgr;
        public GameObject GoTips;
//        public Button btnTipsClick;
    }

    public class UpItemGroup
    {
        // public UpItemGroup(int itemId, int num, UIMBItemComponent mbItemComponent = null)
        // {
        //     UpOneItem upOneItem = new UpOneItem(itemId, num, mbItemComponent);
        //     this.itemList.Add(upOneItem);
        // }
        
        public void AddItem(int itemId, int num, UIMBItemComponent mbItemComponent = null)
        {
            UpOneItem upOneItem = new UpOneItem(itemId, num, mbItemComponent);
            this.itemList.Add(upOneItem);
        }

        public List<UpOneItem> itemList = new List<UpOneItem>();

        public int ItemIdFirst()
        {
            return this.itemList[0].itemId;
        }
    }

    public class UpOneItem
    {
        public UpOneItem(int itemId, int num, UIMBItemComponent mbItemComponent = null)
        {
            this.itemId = itemId;
            this.num = num;
            this.mbItemComponent = mbItemComponent;
        }
        public UIMBItemComponent mbItemComponent;
        public int itemId;
        public int num;
    }
}