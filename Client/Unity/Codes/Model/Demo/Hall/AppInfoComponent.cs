using System;
using UnityEngine;

namespace ET
{
    public enum SingleState{
        none,
        reSet,
        singling,
        singled,
        intering,
        interned,
    }

    public class AppInfoComponent : Entity, IAwake,IDestroy
    {
        public static AppInfoComponent Instance;
        private SingleState _singleState = SingleState.none;
        public string downloadUrl;

        public int closeMatching = 0;
        public string registContry = "";
        public string registRetion = "";
        public bool photoConnect = false;
        public int enterType = 0;

        public int loginStep = 0;
        public int pvpWin = 0;

        public bool _isWaitingSer = false;

        public int AppStartSign = 0;
        public int playMBTimes = 0;
        public bool canPvp = false;
        public int percentMBEdit = -1;
        public int mapRandom = 0;
        public Material grey;

        public bool isWaitingSer
        {
            set
            {
                this._isWaitingSer = value;
                EventDispatcher.PostEvent(EventName.IsWaitSer, null);
            }
            get
            {
                return this._isWaitingSer;
            }
        }

        public int isNewDevice//2：更换账号登录，1：挤账号登录（本次设备和上次设备不一致，0：正常登录,3:新账号第一次登录
        {
            get
            {
                int device = PlayerPrefs.GetInt("IsNewDevice", 0);
                return device;
            }
            set
            {
                Log.Console("isNewDevice:" + this.isNewDevice);
                PlayerPrefs.SetInt("IsNewDevice", value);
            }
        }

        public SingleState singleState
        {
            get
            {
                return this._singleState;
            }
            set
            {
                this._singleState = value;
            }
        }

        public bool guiding = false;
        public int guideStep
        {
            get
            {
                return (int)PlayerPrefs.GetInt(ItemSaveStr.GuideStep, 1);
            }
            set
            {
                PlayerPrefs.SetInt(ItemSaveStr.GuideStep, value);
                Log.Console((GuideStep)value + " :引导Step：" + value);
            }
        }
        //public bool isForceEnterMainCity = false;


        //1位,level up UI
        public int Sign
        {
            set
            {
                PlayerPrefs.SetInt(ItemSaveStr.Sign, value);
            }
            get
            {
                int sign = PlayerPrefs.GetInt(ItemSaveStr.Sign, 0);
                return sign;
            }
        }

        private LanguageSelect2 _languageSelect2;

        public LanguageSelect2 languageSelect{
            set
            {
                this._languageSelect2 = value;
//                GameDataMgr.Instance.languageSelect = (LanguageSelect)Enum.Parse(typeof(LanguageSelect), _languageSelect2.ToString());
            }
            get
            {
                return this._languageSelect2;
            }
        }
    }
}