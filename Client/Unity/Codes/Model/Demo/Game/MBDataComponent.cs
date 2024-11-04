using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class MBDataComponent: Entity, IAwake, IDestroy
    {
        public static MBDataComponent Instance;

        public float process = 0.1f;
        public int mySeat = 0;
        public int curPlayLevel = -1;
        private int mblevel = 1;
        public int reveiveNum = 3;
        public int maxlevel = 600;
        public int itemNum = 16;
        public int maxItemType = 8;
        public int curFruitItem = 20;//水果效果Id
        public int curFruitRangNum = 50;//概率

        public int failTime = 0;
        public List<FunyLevelEvent> funyLevelList = null;
        public int level
        {
            get
            {
                this.mblevel = PlayerPrefs.GetInt("mblevel", 1);
                return this.mblevel;
            }
            set
            {
                this.mblevel = value;
                PlayerPrefs.SetInt("mblevel", this.mblevel);
                GameDataMgr.Instance.mbLv = value;
                Log.Console("level:" + value);
            }
        }
        public bool isWin = true;

        public int itemBuyId = 0;
        public int alreadyReveiveNum = 0;
        public List<int> freeNumList = new List<int>();
        public List<int> mbMoreFreeNumList = new List<int>();
    }

    public class MBConfigItem
    {
        // public int configId;
        // public int index;
        public List<MBConfigOneItem> itemPosDic = new List<MBConfigOneItem>();//key:configId, value:index
        public int num = 0;

        public void Init(int configId, int pos)
        {
            num = 1;
            MBConfigOneItem mbConfigOneItem = new MBConfigOneItem();
            mbConfigOneItem.configId = configId;
            mbConfigOneItem.index = pos;
            this.itemPosDic.Add(mbConfigOneItem);
        }
        
        public void Add(int configId, int pos)
        {
            num += 1;
            MBConfigOneItem mbConfigOneItem = new MBConfigOneItem();
            mbConfigOneItem.configId = configId;
            mbConfigOneItem.index = pos;
            this.itemPosDic.Add(mbConfigOneItem);
        }
    }
    
    public class CardId
    {
        public const int addGridId = 2;
        public const int reSortId = 3;
        public const int bomb = 4;
        public const int extendGrid = 5;
        public const int lighting = 6;
    }
    
    public class MBConfigOneItem
    {
        public int configId;
        public int index;
    }
    
    public class MBLvAll
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Num { get; set; }
        public int RowOffset { get; set; }
        public int NumOffset { get; set; }
        public string ItemList { get; set; }
    }
    
    public class MBLvOneItem
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Num { get; set; }
        public int RowOffset { get; set; }
        public int NumOffset { get; set; }
        public int itemId { get; set; }
        public int level { get; set; }

        public MBLvOneItem(int id, int row, int num, int rowOffset, int numOffset, int itemId, int level)
        {
            this.Id = id;
            this.Row = row;
            this.Num = num;
            this.RowOffset = rowOffset;
            this.NumOffset = numOffset;
            this.itemId = itemId;
            this.level = level;
            
        }
    }
}