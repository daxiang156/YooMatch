using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class MBMoreDataComponent: Entity, IAwake, IDestroy
    {
        public long StartTime = 0;
        private static MBMoreDataComponent _instance;
        
//        public List<HouseObjInfo> housePlayerList = new List<HouseObjInfo>();
        public int mySeat = 0;
        public int pvp_single = 1;
        public int totalBlood = 100;
        public int failScoreRe = -10;
        
        public int SelectCardItemId = CardId.reSortId;
        
        public List<PlayerData> playerList = new List<PlayerData>();

        public int curMBMoreMode = MBMoreMode.Attack;
        /// <summary>
        /// 结算后增加多少星星
        /// </summary>
        public int resultAddStar = 0;
        public static MBMoreDataComponent Instance
        {
            get
            {
                if (_instance == null)
                {
                    Game.Scene.AddComponent<MBMoreDataComponent>();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }

    public class PlayerData
    {
        public int rank = 0;
        private float _percent = 0;
        public int score = 0;

        public float percent
        {
            set
            {
                Log.Console("_percent:" + value);
                this._percent = value;
            }
            get
            {
                return this._percent;
            }
        }
        public HouseObjInfo houseObjInfo = null;
    }
}