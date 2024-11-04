using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Spine.Unity;

namespace ET
{
    public class UIMBMoreComponent: Entity, IAwake, IDestroy, IUpdate
    {
        public GameObject oneItem;
        public Button btnItem;
        public Button btnReturn;
        public Button btnReLast;
        public Button btnAddGrid;
        public Button btnUpdate;
        public Slider slider;
        public Text textRelast;
        public Text textAddgrid;
        public Text textUpdate;
        public Text myName;
        public Image imgMK;
        public Image head;
        
        
        public Image add;
        public Image re;
        public Image itemBg;
        public Image itemBomb;
        public Image bombBg;
        public Image ice;

        public Text txtCountTime;
        public Slider percent;
        public Text percentTxt;
        
        public GameObject bottomItem;
        public GameObject MB_Star;
        public GameObject lastGridRed;
//        public SkeletonGraphic spineResort;

        public GameObject greyUpdate;
        public GameObject greyAddGrid;
        public GameObject greyRelast;
        public GameObject free;
        public GameObject mask;
        
        public GameObject particleMy;
        public GameObject particleOther;
        public GameObject particleBloodMy;
        public GameObject particleBloodOther;
        public GameObject particleMagic;
        public Image imgBg;
        
        public Text scoreRe1;
        public Text scoreRe2;
        public Text ScoreMy;
        public Text ScoreOther;
        public Image magicProgress;
        public Text magicProgressTxt;
        public int totalBlood = 100;
        public int curBlood = 100;
        public int curMagic = 0;
        public int TotalMagic = 3;
        public GameObject Imageicon;
        public GameObject player2;
        public GameObject goTop;
        
        public Slider lianji;
        public Text txtLianji;
        public Button btnLianji;
        public Button btnGrid8;
        
        public List<Transform> downPosList = new List<Transform>();
        public List<Transform> itemGridPosList = new List<Transform>();
        public List<UIMBItemComponent> addList = new List<UIMBItemComponent>();
        public List<UIMBItemComponent> downList = new List<UIMBItemComponent>();
        public List<UIMBItemComponent> addGridList = new List<UIMBItemComponent>();
        
        // public GameObject player2;
        // public GameObject player3;
        // public GameObject player4;
        /// <summary>
        /// 所有水果
        /// </summary>
        public Dictionary<int, UIMBItemComponent> allGridDic = new Dictionary<int, UIMBItemComponent>();
        /// <summary>
        /// 格子Id，格子里面水果列表
        /// </summary>
        public Dictionary<int, Dictionary<int, UIMBItemComponent>> allOneGridDic = new Dictionary<int, Dictionary<int,UIMBItemComponent>>();
        
        public List<UIMBPlayerComponent> otherPlayerList = new List<UIMBPlayerComponent>();
        public List<GameObject> otherPlayerObj = new List<GameObject>();
        public PlayerData myPlayerData;
        public GameObject Go30Point;
    }
}