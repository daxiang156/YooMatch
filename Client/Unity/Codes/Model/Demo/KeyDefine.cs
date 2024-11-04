namespace ET
{

    public static class KeyDefine
    {
        public const int Max = 10000;
        /// <summary>
        /// 平台币
        /// </summary>
        public const int PlatFormCoin = 1;
        /// <summary>
        /// 金币
        /// </summary>
        public const int GameCoin = 3;
        /// <summary>
        /// 等级
        /// </summary>
        public const int Level = 1000001;
        /// <summary>
        /// 经验
        /// </summary>
        public const int Exp = 1000002;
        /// <summary>
        /// 奖杯进度
        /// </summary>
        public const int HeroCup = 4;
        //public const int PHYSICAL = 5;
        /// <summary>
        /// 消消乐等级
        /// </summary>
        public const int MBLv = 1000003;

        public const string RankWorld = "World";

        public const int SkinId = 2;
        public const int ItemId = 1;
        
        
        public const int entityId = 5;
        public const int unlockId = 530502;
        public const int reLastId = 530503;
        public const int reSortId = 530504;
        public const int addGridId = 530505;
        public const int star = 600001;
        
        
        public const int enterMainCity = 0;
        public const int enterMb = 1;

        public const int guideLevel = -100;
    }
    
    public static class ItemType
    {
        public const int skin = 2;
        public const int item = 1;
        public const int UseItem = 5;
    }
    
    public class ItemBigType
    {
        public const int ItemType_1 = 1;//游戏资源类
        public const int ItemType_Wear = 2;//装备饰品类

        public const int ITemType_Box = 3;//随机宝箱
        //ItemType_Bag = 4,
        public const int ItemType_Clothes = 4;
    }
    
    public class ItemSmallType
    {
        public const int Bag = 1;
        public const int Weapon = 2;
        public const int Balloon = 3;
    }
    
    public enum SkinTabPage
    {
        skin = 1,
        clothes = 2,
        bag = 3,
        weapon = 4,
        balloon = 5,
    }
    
    public class MapDefine
    {
        public const int MonkeyBusiness = 1;
        public const int JAVA_VOLCANO = 4;
        public const int SUPER_ARENA = 9;
        public const int GOLDEN_EGG = 7;
        public const int MBMore = 15;
    }

    public enum GuideStep
    {
        MainCityFirst = 1,
        MBItem1,
        MBItem2,
        MBItem3,
        MBItem4,
        MBItem21,
        MBItem22,
        End,// = 8,
        MBItem23,
        MBItem24,
        MBItem25,
        Relast,
        RelastUse,
        EndRelast,
        UpdateItem,
        UpdateUse,
        EndUpdate,
        AddGrid,
        AddGridUse,
        EndAddGrid ,
        ReturnMbMap,
        ReturnMainCity,
        Task,
//        End3,//暂时隐藏给钥匙用
        



        UseBlind,//24
        EndBlind,
        UseIce,//
        EndIce,
        UseKey,
        End3,
    }

    public class GuideLv
    {
        public const int first = 1;
        public const int addGrid = 5;
        public const int resort = 12;
        public const int useBlind = 8;
        public const int useIce = 16;
        public const int UseKey = 20;
    }

    public class MBEventType
    {
        public const int ClickFruit = 100;
        public const int ResultLose = 2;
        public const int ResultWin = 3;
        public const int AllFruit = 4;
        public const int UseItemReLast = 5;
        public const int UseItemResort = 6;
        public const int UseItemAddGrid = 7;
        
        public const int BombItem = 101;
        public const int BombCard = 201;
    }
    
    public class SignIndex
    {
        public const int levelUp = 1;
        public const int nickName = 10;
    }

    public class LogEvent
    {
        public const int reveive = 17;
        public const int adAdd = 18;
    }
    
    public class PVPResultType
    {
        public const int myWin = 1;
        public const int otherWin = 2;
        public const int timeOut = 3;
    }
    
    public class MBSignType
    {
        public const int FailTime = 1;
        public const int RelastTime = 2;
        public const int AddGridTIme = 3;
        public const int ResortTIme = 4;
    }
    
    public class MBMoreMode
    {
        public const int Normal = 0;
        public const int ItemUse = 1;
        public const int Attack = 2;
    }
    
    public class MBMode
    {
        public const int Normal = 1;
        public const int Pvp = 2;

        public const int NormalStart = 1;
        public const int PvpStart = 2;
        public const int NormalFinish = 11;
        public const int PvpFinish = 12;
    }
    
    public class MBAttackType
    {
        public const int ad = 1;
        public const int re = 2;
        public const int bomb = 3;
        public const int ice = 4;
        
    }
}