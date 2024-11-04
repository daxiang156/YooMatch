using UnityEngine;

namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }

        public struct SceneChangeStart
        {
            public Scene ZoneScene;
        }
        
        
        public struct SceneChangeFinish
        {
            public Scene ZoneScene;
            public Scene CurrentScene;
        }

        public struct ChangePosition
        {
            public Unit Unit;
            public Vector3 OldPos;
        }

        public struct ChangeRotation
        {
            public Unit Unit;
        }
        
        public struct EntityChangePosition
        {
            public SEntity sEntity;
            public Vector3 OldPos;
        }

        public struct EntityChangeRotation
        {
            public SEntity sEntity;
        }

        public struct PingChange
        {
            public Scene ZoneScene;
            public long Ping;
        }
        
        public struct AfterCreateZoneScene
        {
            public Scene ZoneScene;
        }
        
        public struct AfterCreateCurrentScene
        {
            public Scene CurrentScene;
        }
        
        public struct AfterCreateLoginScene
        {
            public Scene LoginScene;
        }

        public struct AppStartInitFinish
        {
            public Scene ZoneScene;
        }

        public struct LoginFinish
        {
            public Scene ZoneScene;
        }

        public struct LoadingBegin
        {
            public Scene Scene;
        }
        
        public struct EnterGame
        {
            public Scene Scene;
        }

        public struct LoadingFinish
        {
            public Scene Scene;
        }

        public struct EnterMapFinish
        {
            public Scene ZoneScene;
        }

        public struct AfterUnitCreate
        {
            public Unit Unit;
        }
        
        public struct MoveStart
        {
            public Unit Unit;
        }

        public struct MoveStop
        {
            public Unit Unit;
        }
        
        public struct SEntityMoveStart
        {
            public SEntity SEntity;
        }

        public struct SEntityMoveStop
        {
            public SEntity SEntity;
        }
        
        public struct UnitEnterSightRange
        {
        }

        public struct AccountLoginFinish
        {
            public int AccountSkinId;
            public Scene AccountZone;
        }
        
        public struct EnterNameFinish
        {
            public int AccountSkinId;
            public Scene AccountZone;
        }
        
        public struct PlayConnect
        {
            public Scene ZoneScene;
        }
        
        public struct EnterUIGame
        {
            public Scene ZoneScene;
        }
        public struct EnterHeroRoad
        {
            public Scene ZoneScene;
        }
        
        public struct GetHeroRoadReward
        {
            public Scene ZoneScene;
        }
        
        public struct GameFinish
        {
            public Scene ZoneScene;
        }

        public struct BattleSceneFinish
        {
            public Scene battleScene;
        }
        
        
        public struct EnterPopUI
        {
            public Scene ZoneScene;
        }
        
        
        public struct ChangeSkinId
        {
            public Scene ZoneScene;
            public int skinId;
        }
        
        public struct TipErrorCode
        {
            public Scene ZoneScene;
            public int errorId;
        }
        
        public struct GameResult
        {
            public Scene ZoneScene;
        }
        
        public struct MonoResult
        {
            public Scene ZoneScene;
        }
        
        
        public struct CreateUIMainCity
        {
            public Scene ZoneScene;
        }
        
        public struct UpdateGold
        {
            public Scene ZoneScene;
            public int gold;
        }

        public struct UpDatePlatFormCoin
        {
            public Scene ZoneScene;
            public int platFormCoin;
            public int addFromCoin;
        }


        public struct JoinMatch
        {
            public Scene ZoneScene;
        }

        public struct JoinMatchCancel
        {
            public Scene ZoneScene;
        }

        public struct SyncMatchInfo
        {
            public Scene ZoneScene;
            public int matchNum;
        }

        public struct SyncMatchMBInfo
        {
            public Scene ZoneScene;
            public int matchNum;
            public string nickName;
            public int skinId;
            public int pvpLevelStar;
        }

        public struct CreateTpsControl
        {
            public Scene ZoneScene;
        }
        
        
        public struct ChgTpsAreaReive
        {
            public Scene ZoneScene;
        }

        public struct GetGoodList
        {
            public Scene ZoneScene;
        }
        
        public struct BuyGoods
        {
            public Scene ZoneScene;
            public int goodId;
        }
        
        public struct GetSkin
        {
            public Scene ZoneScene;
        }
        
        public struct RemoveShop
        {
            public Scene ZoneScene;
        }

        public struct ShowTpsControl
        {
            public Scene ZoneScene;

            public bool isShow;
            public bool isAll;
        }

        public struct IsShowMainCity
        {
            public Scene ZoneScene;
            public bool isShow;
        }
        
        public struct EnterMBRankUI
        {
            public Scene ZoneScene;
            public object openParam;
        }

        public struct BindEmailSuc
        {
            public Scene ZoneScene;
            public bool state;
        }
        
        public struct CreateMBMore
        {
            public Scene ZoneScene;
        }
        
        public struct CreateChatUnder
        {
            public Scene ZoneScene;
        }
        
        public struct MBEventAttack
        {
            public int eventType;
            public int paras;
        }
    }
    
    public class MBItemType
    {
        public const int none = 0;
        public const int add1 = 1;
        public const int re1 = -1;
        public const int bomb = 101;
        public const int ice = 102;
        public const int beIce = 103;
        public const int beIce3 = 104;
        
        public const int ItemLock = 1001;
        public const int ItemKey = 1002;
        //public const int ItemIce = 1003;
        
        //单击的
        public const int BlindBox = 11;
        public const int MBIce = 12;
        public const int Lock = 13;
        public const int ItemIce = 201;
        public const int max = 202;

    }
}