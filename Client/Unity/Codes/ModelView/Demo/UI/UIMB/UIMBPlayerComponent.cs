using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Spine.Unity;

namespace ET
{
    public class UIMBPlayerComponent: Entity, IAwake, IDestroy, IUpdate
    {
        public long unitId;
        public int skinId;
        public GameObject oneItem;
        public GameObject selfObj;
        public GameObject bottomItem;
        public Text playerName;
        public int totalBlood = 100;
        public int curBlood = 100;
        public Slider percent;
        public Text percentTxt;
        public List<Transform> downPosList = new List<Transform>();
        public List<UIMBItemComponent> downItemist = new List<UIMBItemComponent>();
        public GameObject lose;
        public GameObject particleOther;
        public Image imgMK;
        public Image head;
        public bool isRobot = false;
        
        public Image ice;

        public int seat = 0;
        public int downCount = 0;
        public bool isLose = false;
        public bool stop = false;
        
        public int rangNum = 0;

        public int resortNum = 1;
        public int addGridNum = 1;

        public PlayerData playerData = null;
        /// <summary>
        /// 所有水果
        /// </summary>
        public Dictionary<int, UIMBItemComponent> allGridDic = new Dictionary<int, UIMBItemComponent>();
        public Dictionary<int, UIMBItemComponent> upGridDic = new Dictionary<int, UIMBItemComponent>();
        public Dictionary<int, UIMBItemComponent> allInitGridDic = new Dictionary<int, UIMBItemComponent>();

        
        /// <summary>
        /// 上层遮挡数量,<itemId， 列表>
        /// </summary>
        public Dictionary<int, Dictionary<int, List<UIMBItemComponent>>> overTileDic = new Dictionary<int,Dictionary<int, List<UIMBItemComponent>>>();
       
        /// <summary>
        /// itemId,<上层遮挡数量， 列表>
        /// </summary>
        public Dictionary<int, Dictionary<int, List<UIMBItemComponent>>> itemNumTileDic = new Dictionary<int,Dictionary<int, List<UIMBItemComponent>>>();

        // public Dictionary<int, List<UIMBItemComponent>> over1TileDic = new Dictionary<int, List<UIMBItemComponent>>();
        // public Dictionary<int, List<UIMBItemComponent>> over2TileDic = new Dictionary<int, List<UIMBItemComponent>>();
        // public Dictionary<int, List<UIMBItemComponent>> over3TileDic = new Dictionary<int, List<UIMBItemComponent>>();
        
        /// <summary>
        /// 格子Id，格子里面水果列表
        /// </summary>
        public Dictionary<int, Dictionary<int, UIMBItemComponent>> allOneGridDic = new Dictionary<int, Dictionary<int,UIMBItemComponent>>();

        
        public List<UIMBItemComponent> lastRemoveList = new List<UIMBItemComponent>();
    }
}