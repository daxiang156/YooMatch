using System;

namespace ET
{
    public class EventName
    {
        //Volume
        public const string VolumeMusic = "VolumeMusic";
        public const string VolumeUI = "VolumeUI";
        
        
        
        //Dev2Dev
        public static string Dev2Dev = "Dev2Dev";
        public static string Dev2LvUp = "Dev2LvUp";
        
        //UI player
        public static string ShowUIAvatar = "ShowUIAvatar";
        
        //Master转换
        public static string RequestMaster = "RequestMaster";
        public static string SwitchMaster = "SwitchMaster";
        
        //App后台
        public static string AppBackgrounded = "AppBackgrounded";
        public static string AppFocused = "AppFocused";
        public static string MobileChatInput = "MobileChatInput"; //聊天的键盘被拉起
        public static string ForceDisconnectAfter1Min = "ForceDisconnectAfter1Min"; //强行断掉网络
        
        //Star system
        public static string UpdateStarCount = "UpdateStarCount";
        
        //Weapon system 给武器
        public static string GiveWeapon = "GiveWeapon"; //参数一: 武器类型: 0-hammer, 1-RPG. 参数二: 武器编号: 
        public static string RemoveWeapon = "RemoveWeapon"; //参数一: 武器类型: 0-hammer, 1-RPG.
        public static string UpdateRoomWeaponList = "UpdateRoomWeaponList";
        
        //nameTag
        
        public const string ShowNameTag = "ShowNameTag";
        //Switch Camera
        public const string SwitchCamera = "SwitchCamera";
        
        //Map
        public const string ChangeStar = "ChangeStar";

        //HP
        public const string ResetHP = "ResetHP";
        
        //Joe_Connect_To_Server
        public const string ConnectUsingSetting = "ConnectUsingSetting";
        public const string RoomNameReady = "RoomNameReady";
        public const string SwitchToSinglePlayer = "SwitchToSinglePlayer";
        
        //Joe control
        public const string MapReady = "MapReady";
        public const string removeInitScreen = "removeInitScreen";
        public const string SinglePlayerStart = "SinglePlayerStart";
        public const string MultiplPlayerStart = "MultiplayerStart";
        public const string GameRuleSet = "GameRuleSet";
        
        //player control
        public const string PlayerControlInstantiated = "PlayerControlInstantiated";
        public const string PlayerControlReady = "PlayerControlReady";

        //spawn player v2
        public const string ResetSpawnPlayerV2 = "ResetSpawnPlayerV2";
        public const string SetupPlayer = "SetupPlayer";
        public const string SetupAllBots = "SetupAllBots";
        public const string LocalPlayerReady = "LocalPlayerReady";
        public const string PlayerInstantiated = "PlayerInstantiated";
        public const string AllPlayersReady = "AllPlayersReady";
        public const string AllBotsReady = "AllBotsReady";
        public const string AllPlayerHaveCreatedBots = "AllPlayerHaveCreatedBots";

        //game control
        public const string StopGame = "StopGame";
        public const string PauseGame = "PauseGame"; //for team elimination
        public const string StartGame = "StartGame"; //triggered after 3,2,1, this will reset timer
        public const string PhotonRoomReady = "PhotonRoomReady"; //triggered after 3,2,1, this will reset timer
        public const string PlayerConnected = "PlayerConnected"; //this will update "PhotonServerStat"
        public const string ConnectPhotonRoom = "ConnectPhotonRoom";
        public const string GameRoomInfoReceived = "GameRoomInfoReceived";
        public const string MasterUpdatePlayerCount = "MasterUpdatePlayerCount";
        public const string OtherPlayerLeft = "OtherPlayerLeft";
        public const string ChangeScene = "ChangeScene"; //从副本退出需要离开光子房间

        //Photon
        public const string PhotonLeaveRoom = "PhotonLeaveRoom"; //游戏后台时间长一点之后重新加入光子
        
        //Photon disconnect 断线重连
        public const string PhotonDisconnected = "PhotonDisconnected"; //经过尝试，光子无法链接，断开光子链接
        public const string PhotonCannotConnect = "PhotonCannotConnect"; //经过尝试，光子无法链接，断开光子链接
        public const string PhotonCanConnect = "PhotonCanConnect"; //光子可以连接
        public const string PhotonReconnected = "PhotonReconnected"; //重连光子, 回到房间
        public const string PhotonReconnected_ToLobby = "PhotonReconnected_ToLobby"; //重连光子, 没有回到房间
        public const string ReconnectPhoton = "ReconnectPhoton";
        public const string PhotonReconnecting = "PhotonReconnecting"; //脚本间通信，正在光子重连
        public const string BotAIAdded = "BotAIAdded"; //光子重连，重新启动机器人
        public const string PhotonCannotReconnect = "PhotonCannotReconnect"; //光子无法重连
        public const string PhotonGoBackToCity = "PhotonGoBackToCity"; //光子无法链接, 直接回到主城
        public const string PhotonHardDisconnect = "PhotonHardDisconnect";
        public const string PhotonStopReconnecting = "PhotonStopReconnecting";
        public const string SpawnPlayerV2Reconnect = "SpawnPlayerV2Reconnect";
        public const string EnterOrExitMainCity = "EnterOrExitMainCity";//1,进入主城。2，退出主城

        //loading screen
        public const string LoadingScreenClosed = "LoadingScreenClosed";

        //IP地址
        public const string IPReady = "IPReady";

        //机器人bot
        public const string GenerateBot = "GenerateBot";

        //副本游戏开始，分数
        public const string HouseStart = "HouseStart";
        public const string AddGameScore = "AddGameScore";
        public const string SyncGameScoreInfo = "SyncGameScoreInfo";

        public const string Dead = "Dead";
        public const string CanControl = "CanControl";
        public const string CameraAni = "CameraAni";

        //Score keeping
        public const string PlayerDied = "PlayerDied";
        public const string CargoCarried = "CargoCarried";
        public const string SetFinalRank = "SetFinalRank";

        //Lava
        public const string LavaRise = "LavaRise";
        public const string StartFire = "StartFire";

        //AI
        public const string JumpBegin = "JumpBegin";
        
        public const string PoleGet = "PoleGet";
        public const string PolePost = "PolePost";
        
        public const string Finish = "Finish";

        //PVP01
        public const string FloorDropped = "FloorDropped";
        public const string ShowCollectBtn = "ShowCollectBtn";
        public const string UpdateCollectBtn = "UpdateCollectBtn";
        public const string CollectBtnClick = "CollectBtnClick";
        public const string CollectDiamondNumUpdate = "CollectDiamondNumUpdate";
        public const string BeginCollect = "BeginCollect";
        public const string ChgModuleId = "ChgModuleId";
        public const string ChgModuleIdSkinUI = "ChgModuleIdSkinUI";
        public const string ChgModuleClothes = "ChgModuleClothes";
        public const string ChgModuleCap = "ChgModuleCap";
        public const string ChgOneModuleCap = "ChgOneModuleCap";
        
        public const string PhotonLeaveMainCity = "PhotonLeaveMainCity";
        public const string ChangeMap = "ChangeMap";
        public const string EnterFunGame = "EnterFunGame";
        public const string EnterFunGameClear = "EnterFunGameClear";
        
        public const string ADAlreadyLoad = "ADAlreadyLoad";

        //restart game for all clients
        public const string ReStartGame = "ReStartGame";
        public const string ClearLava = "ClearLava";
        public const string RestartPlayer = "RestartPlayer";
        public const string ResetTimer = "ReserTimer";
        public const string ClearDiamondCounter = "ClearDiamondCounter";
        public const string RestoreDiamond = "RestoreDiamond";
        
        public const string ResultMsgReq = "ResultMsgReq";
        public const string ResultMsgReturn = "ResultMsgReturn";


        public const string CollectGoldCity = "CollectGoldCity";
        
        
        public const string BtnPlayClick = "BtnPlayClick";
        public const string BtnAvatarClick = "BtnAvatarClick";
        public const string ETBtnAvatarClick = "ETBtnAvatarClick";
        public const string ShowMainCityUI = "ShowMainCityUI";
        public const string CloseSkinUI = "CloseSkinUI";
        public const string OpenSkinCameraPos = "OpenSkinCameraPos";
        public const string IsShowTpsObj = "IsShowTpsObj";
        public const string LockCamera = "LockCamera";
        public const string RevertSkin = "RevertSkin";
        
        
        public const string UITipsShow = "UITipsShow";
        public const string ExitGameScene = "ExitGameScene";

        public const string SkillClick = "SkillClick";

        //diamond
        public const string UpdateDiamondCount = "UpdateDiamondCount";
        public const string RankDiamondInfo = "RankDiamondInfo";

        //set nickname
        public const string SetNickname = "SetNickname";
        
        public const string PunLeaveRoom2 = "PunLeaveRoom2";
        public const string PunLeaveRoom = "PunLeaveRoom";
        public const string ReturnCityIng = "ReturnCityIng";
        public const string PunJoinOrCreateRoom = "PunJoinOrCreateRoom";

        public const string IsHideChat = "IsHideChat";
        public const string StartAndHideBtnStart = "StartAndHideBtnStart";
        public const string ExitMiniGame = "ExitMiniGame";

        public const string UpdateGameCoin = "UpdateGameCoin";
        public const string UpdateLevel = "UpdateLevel";
        public const string CloseWaitingUI = "CloseWaitingUI";
        public const string UpdateNumeric = "UpdateNumeric";
        public const string UpDatePlatFormCoin = "UpDatePlatFormCoin";

        public const string IsShowJoyStick = "IsShowJoyStick";
        public const string CameraRendMode = "CameraRendMode";
        public const string ShowHeroRoad = "ShowHeroRoad";
        public const string ShowGameTpsControl = "ShowGameTpsControl";
        public const string LoginErrCatch = "LoginErrCatch";
        public const string RealErrCatch = "RealErrCatch";
        public const string GateErrCatch = "GateErrCatch";
        public const string ApplicationFocus = "ApplicationFocus";
        public const string DisAndConnect = "DisAndConnect";

        public const string NoticeSkinWear = "NoticeSkinWear";
        public const string NoticeSkinApperance = "NoticeSkinApperance";
        public const string NoticeSkinWearCash = "NoticeSkinWearCash";

        //team
        public const string TeamAssignmentReady = "TeamAssignmentReady";

        //camera
        public const string ResetCameraPivot = "ResetCameraPivot";

        //for joystick
        public const string DisableJoystick = "DisableJoystick";

        //for testing
        public const string SpawnPlayer = "SpawnPlayer";
        public const string enableControl = "enableControl";

        public const string JoinPhotoRoom = "JoinPhotoRoom";
        public const string ShowInitBg = "ShowInitBg";
        public const string ReconnectChat = "ReconnectChat";
        public const string DashboardReady = "DashboardReady";
        public const string ShowTips = "ShowTips";
        
        public const string ShowTpsInCity = "ShowTpsInCity";
        public const string GetGameCoin = "GetGameCoin";

        public const string HideMainCityTpsControl = "HideMainCityTpsControl";

        //map specific
        public const string MasterSwitched = "MasterSwitched";

        //player rank
        public const string UpdatePlayerRank = "UpdatePlayerRank";

        //player live 玩家掉血
        public const string LoseLive = "LoseLive";
        public const string DisReconnectPhoton = "DisReconnectPhoton";
        public const string DisConnect = "DisConnect";
        public const string AutoReconnectPhoton = "AutoReconnectPhoton";

        public const string V2AddPhotonView = "V2AddPhotonView";
        public const string ChangeSceneTpsReset = "ChangeSceneTpsReset";
        public const string LeaveMainCity = "LeaveMainCity";
        
        public const string DisPhotonTis = "DisPhotonTis";
        public const string LightChgEvent = "LightChgEvent";
        public const string ConnectInGame = "ConnectInGame";
        public const string CheckPhotonConnect = "CheckPhotonConnect";
        public const string IsPhotonConnect = "IsPhotonConnect";
        public const string CheckOurSerConnect = "CheckOurSerConnect";
        public const string ConnectMySerInGame = "ConnectMySerInGame";
        public const string ConnectDis = "ConnectDis";
        
        public const string MBAgain = "MBAgain";
        public const string MBAgainAlready = "MBAgainAlready";
        public const string UpdateVersion = "UpdateVersion";

        public const string CreateBotSuccess = "CreateBotSuccess";
        public const string NetWaitUI = "NetWaitUI";
        public const string NetWaitLongSingleResult = "NetWaitLongSingleResult";
        public const string ShowMainCity = "ShowMainCity";
        public const string LoadingCloseListen = "LoadingCloseListen";
        public const string LoadingPress = "LoadingPress";
        public const string CloseWinBe = "CloseWinBe";
        public const string CloseWin = "CloseWin";
        public const string CheckEnterMainCity = "CheckEnterMainCity";
        public const string CheckEnterMainCityInEnterName = "CheckEnterMainCityInEnterName";
        
        public const string TaskRed = "TaskRed";
        public const string ChargeToSer = "ChargeToSer";
        public const string UseItemNum = "UseItemNum";

        public const string BindEmailReturn = "BindEmailReturn";
        public const string SyncMBState = "SyncMBState";
        public const string GuideClick = "GuideClick";
        public const string GuideEnable = "GuideEnable";
        public const string FirstOrderGuide = "FirstOrderGuide";
        

        public const string PayStartBuy = "PayStartBuy";
        public const string PayServerCallback = "PayServerCallback";
        public const string Language = "Language";
        public const string MatchFinish = "MatchFinish";

        public const string AppsFlyEvent = "AppsFlyEvent";
        public const string FireBEvent = "FireBEvent";
        public const string FireBProperty = "FireBProperty";
        public const string SyncItemToServer = "SyncItemToServer";
        public const string InitHallInfo = "InitHallInfo";
        public const string RechargeItem = "RechargeItem";
        public const string SocketDelayTime = "SocketDelayTime";
        public const string PhotonBackstage = "PhotonBackstage";
        
        public const string ChangeItemNum = "ChangeItemNum";
        public const string MBLevelUpdate = "MBLevelUpdate";
        public const string ShowLoadingUI = "ShowLoadingUI";
        public const string IsWaitSer = "IsWaitSer";
        public const string JumpMBMap = "JumpMBMap";
        public const string UpdateItemInMB = "UpdateItemInMB";
        public const string PurchaseInit = "PurchaseInit";
        
        //facebook event
        public const string FaceBookEvent = "FaceBookEvent";
        public const string VoodooEvent = "VoodooEvent";
        /// <summary>
        /// 收到聊天消息
        /// </summary>
        public const string ReciveChat = "ReciveChat";
        public const string ReciveChatRecord = "ReciveChatRecord";
        /*----------------------好友 start-----------------------*/
        public const string GetFriendList = "GetFriendList";
        public const string GetApplyList = "GetApplyList";
        public const string GetIslandList = "GetIslandList";
        public const string ApplyFriend = "ApplyFriend";    //申请好友成功
        public const string ReciveApplyFriend = "ReciveApplyFriend"; //收到好友申请请求
        public const string AgreenApply = "AgreenApply";    //同意好友申请
        public const string RejustApply = "RejustApply";    //拒绝好友申请
        public const string SearchFriend = "SearchFriend";  //搜索好友
        public const string DeleteFriend = "DeleteFriend";  //删除好友
        /*----------------------好友 end-----------------------*/

        public const string ChangeSceneFinish = "ChangeSceneFinish";        //切换场景完成

        public const string GetAndriodOrIosLoginInfo = "GetAndriodOrIosLoginInfo";  //获得google或IOS登录状态
        public const string ClickGoogleLogin = "ClickGoogleLogin";      //点击google登录
        public const string GoogleLoginSuc = "GoogleLoginSuc";          //google登录成功
        public const string ShowGoolgeAds = "ShowGoolgeAds";            //显示google广告
        public const string SendGoogleAdsReward = "SendGoogleAdsReward";    //发送google广告奖励
        public const string GooleAdsRewardClosed = "GooleAdsRewardClosed";  //广告关闭

        public const string ServerNotice = "ServerNotice";//公告
        public const string GoogleAdLoadFail = "GoogleAdLoadFail";      //google广告加载失败

        public const string BigTurnInfor = "BigTurnInfor";              //获得大转盘数据
        public const string BigTurnStartReturn = "BigTurnStartReturn";  //大转盘抽奖返回
        public const string BigTurnEndReturn = "BigTurnEndReturn";      //大装盘抽奖结束

        public const string UpdateReRead = "UpdateReRead";
        public const string WaitAdTimeOut = "WaitAdTimeOut";

        public const string DownLoadFinish = "DownLoadFinish";      //整个下载过程完毕

        public const string ShowMaxMediationDebugger = "ShowMaxMediationDebugger";

        
        public const string ClickFruit = "ClickFruit";
        
        //VirtualSUN: Quantum.
        public const string HeroSkinChangeLocal = "HeroSkinChangeLocal";
        public const string HeroSkinChangeNetwork = "HeroSkinChangeNetwork";
        public const string HeroWeaponSkinChangeLocal = "HeroWeaponSkinChangeLocal";
        public const string HeroWeaponSkinChangeNetwork = "HeroWeaponSkinChangeNetwork";

        public const string GetHeroModel= "StartLoadModel";
        public const string GetHeroModelFinish = "GetHeroModelFinish";

        public const string Vibration = "vibration";
        /// <summary>
        /// 副玩法结算，参数1：排名，1代表胜利，2以上代表失败，参数2：副本玩法Id
        /// param 1: win or lose, param 3: coins
        /// </summary>
        public const string GamePlayResult = "GamePlayResult";
        /// <summary>
        /// 副本请求等级，无需参数
        /// </summary>
        public const string GamePlayLevelReq = "GamePlayLevelReq";
        /// <summary>
        /// 副本等级返回，参数1是副本关卡等级
        /// </summary>
        public const string GamePlayLevelRes = "GamePlayLevelRes";
        
        //configAB加载完成
        public const string ConfigABLoadFinish = "ConfigABLoadFinish";
        //SetJump_level value,keep rot hotupdate
        public const string SetJump_level = "SetJump_level";
        //后台重启时间
        public const string BackgroundRestart = "BackgroundRestart";
        
        public const string DefaultBg = "DefaultBg";
    }
    
    public enum ConnectType
    {
        None = 0,
        Begin,
        Logining,
        Logined,
        SelectRule,
        RealConnecting,
        RealConnected,
        GateConnecting,
        GateConnected,
        EnterGameing,
        EnterGamed,
        Matching,
        InGame,
        Result,
        WinBe,
        Win,
        MBMap,
        MBGame,
        
        ConnectLoginError,
        ConnectRealError,
        connectGateError,
    }
    
    public enum LanguageSelect {
        AutoSelectByIP = 0,
        English = 1,
        Indonesia,
        Ukraine,
        Spanish,
        India,
        Thailand,
        Japan,
        Vietnam,
        Brazil,
    }
}