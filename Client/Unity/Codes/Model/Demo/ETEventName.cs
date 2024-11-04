namespace ET
{
    public class ETEventName
    {
        public const string ShowItemUnlock = "ShowItemUnlock";

        public const string FireBProperty = "FireBProperty";
        public const string VoodooEvent = "VoodooEvent";

        public const string SendTenjinEvent = "SendTenjinEvent";      //发送tenjin事件
        /// <summary>
        /// 登录服务器完成
        /// </summary>
        public const string LoginServerFinish = "LoginServerFinish";
        /// <summary>
        /// 停止背景音效
        /// </summary>
        public const string StopBgMusic = "StopBgMusic";
        /// <summary>
        /// 播放action音效
        /// </summary>
        public const string PlayActionSound = "PlayActionSound";
        /// <summary>
        /// 切换皮肤
        /// </summary>
        public const string ChangeSkin = "ChangeSkin";

        public const string SetJump_level = "SetJump_level";
        public const string BackgroundRestart = "BackgroundRestart";

        //Pvp
        public const string PvpOtherAttackRemove = "PvpOtherAttackRemove";
        public const string PvpMyAttackRemove = "PvpMyAttackRemove";
        public const string PvpOtherToMeBlood = "PvpOtherToMeBlood";
        public const string PvpMyToOtherBlood = "PvpMyToOtherBlood";
        public const string PvpOtherLose = "PvpOtherLose";
        public const string PvpResultHandle = "PvpResultHandle";
        public const string PvpMatchRemove = "PvpMatchRemove";
        public const string AddHouseObjInfo = "AddHouseObjInfo";
        public const string PvpOtherScore = "PvpOtherScore";
        
        //facebook event
        public const string FaceBookEvent = "FaceBookEvent";
        public const string Dev2Dev = "Dev2Dev";
        public const string Dev2LvUp = "Dev2LvUp";
        
        public const string GetFunyGameFailData = "GetFunyGameFailData";
        
        //皮肤解锁
        public const string CloseUIMB = "CloseUIMB";
        
        //隐藏UIMBmap 滚动条
        public const string HideUIMBMapList = "HideUIMBMapList";
        public const string UIRewawrdClose = "UIRewardClose";
        public const string GetRankData = "GetRankData";
        /// <summary>
        /// 改名
        /// </summary>
        public const string ChangeName = "ChangeName";
        public const string AutoShowLoadingUI = "AutoShowLoadingUI";
        /// <summary>
        /// 上下左右UI收起扩展，0 收起，1 展开
        /// </summary>
        public const string UIAniType = "UIAniType";
        public const string SecondTipYes = "SecondTipYes";
    }

    public class PropertyName
    {
        public const string mb_level_ab = "mb_level_ab";
    }
    
    public enum LanguageSelect2 {
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