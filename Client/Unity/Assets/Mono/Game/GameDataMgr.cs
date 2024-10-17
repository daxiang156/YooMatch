using System.Collections.Generic;
using ET;
using UnityEngine;
using UnityEngine.UIElements;
using EventDispatcher = ET.EventDispatcher;

public class GameDataMgr : Singleton<GameDataMgr>
{
    /// <summary>
    /// 当前模型ID
    /// </summary>
    public string moduleId = "";
    /// <summary>
    /// 玩家名字
    /// </summary>
    public string playerName = "";
    /// <summary>
    /// 金币
    /// </summary>
    public int gold = 0;
    /// <summary>
    /// 平台币
    /// </summary>
    public int coin = 0;
    /// <summary>
    /// 消消乐等级
    /// </summary>
    public int mbLv = 1;
    public int skipLv = 0;
    /// <summary>
    /// 主城匹配Id
    /// </summary>
    public string cityId = "";
    /// <summary>
    /// 当前背包
    /// </summary>
    public Dictionary<int, string> bagModelName = new Dictionary<int, string>();
    /// <summary>
    /// 玩家名字
    /// </summary>
    public string nickName = "";
    /// <summary>
    /// 跳转等级
    /// </summary>
    public string jump_level = "";
    
    /// <summary>
    /// 当前武器列表
    /// </summary>
    public List<string> weaponList = new List<string>();
    public int currentWeapon_Hammer = -1; //current weapon for Serhill
    public int currentWeapon_RPG = -1;

    /// <summary>
    /// 当前衣服
    /// </summary>
    private string clothesStr = "";
    
    /// <summary>
    /// 挂载机器人信息, 第一个数字是阵营，第二个是位置
    /// </summary>
    //public List<Dictionary<int, int>> robDic = new List<Dictionary<int, int>>();
    public List<int> teamList = new List<int>();
    public List<int> posList = new List<int>();

    /// <summary>
    /// google pay 初始化列表
    /// </summary>
    public List<GooglePayData> GooglePlayDatas = new List<GooglePayData>();

    public string FireBaseToken = "";
    public bool isRemoteConfig;


    public void ReStartGame()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            Log.Info("退出游戏重新登录1!!");
            const int kIntent_FLAG_ACTIVITY_CLEAR_TASK = 0x00008000;
            const int kIntent_FLAG_ACTIVITY_NEW_TASK = 0x10000000;
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var pm = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            var intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", Application.identifier);
            intent.Call<AndroidJavaObject>("setFlags", kIntent_FLAG_ACTIVITY_NEW_TASK | kIntent_FLAG_ACTIVITY_CLEAR_TASK);
            currentActivity.Call("startActivity", intent);
            currentActivity.Call("finish");
            var process = new AndroidJavaClass("android.os.Process");
            int pid = process.CallStatic<int>("myPid");
            process.CallStatic("killProcess", pid);
        }
    }


    /// <summary>
    /// 当前场景ID
    /// </summary>
    public long curSceneId
    {
        get;
        set;
    }

    public string clothesId
    {
        set
        {
            this.clothesStr = value;
        }
        get
        {
            return this.clothesStr;
        }
    }
    /// <summary>
    /// 副本匹配Id
    /// </summary>
    public string gameId;
    /// <summary>
    /// 当前衣服
    /// </summary>
    public int TeamId = 0;
    public int SpawnPos = 0;

    public Transform _playerControlObj;
    public int connectTime = 0;
    /// <summary>
    /// 登录是否完成，用来限制登录流程未走完就向服务器发送消息
    /// </summary>
    public static bool isLoginFinish = false;

    /// <summary>
    /// playerControl's Transform
    /// </summary>
    public Transform playerControlObj
    {
        set
        {
            this._playerControlObj = value;
        }
        get
        {
            return this._playerControlObj;
        }
    }

    public bool isClickBtn = false;
    private bool _connecting = false;

    public bool isConnecting // = false;
    {
        set
        {
            Debug.Log("是否重连中：" + value.ToString());
            this._connecting = value;
            //Debug.Log("set isConnect:" + this._connecting);
        }
        get
        {
            //Debug.Log("get ...isConnect:" + this._connecting);
            return this._connecting;
        }
    }

    public int eventId;
    public object paras;

    public bool mbEdit = false;
    public long unitId;
    public Dictionary<long, int> unitId2ViewIdDic = new Dictionary<long, int>();

    
    private ConnectType _curConnectType = ConnectType.None;
    public ConnectType curConnectType
    {
        set
        {
            this._curConnectType = value;
            Debug.Log("_curConnectType: " + _curConnectType);
        }
        get
        {
            return this._curConnectType;
        }
    }

    public bool isconnectPhotoning = false;
    public bool singleplayer = false;

    //光子断线
    public bool PhotonDisconnected = false;
    
    public bool setLanguageing = false;
    private LanguageSelect _languageSelect;
    public LanguageSelect languageSelect
    {
        set
        {
            this._languageSelect = value;
        }
        get
        {
            return _languageSelect;
        }
    }
    
    public string language = "";
    public void Init()
    {
        _deviceId = "";
        Application.RequestAdvertisingIdentifierAsync(((string advertisingId,bool trackingEnabled,string errorMsg) =>
        {
            Debug.LogError("GAID:" + errorMsg);
            Debug.LogError("GAID:" + advertisingId);
            _deviceId = advertisingId;
        }));
        unitId2ViewIdDic.Clear();
        isConnecting = false;
        curConnectType = ConnectType.None;
        cityId = "";
        gameId = "";
        _playerControlObj = null;
        bot_number = -1;
        TeamId = 0;
        SpawnPos = 0;
        mbEdit = false;
        isconnectPhotoning = false;
        isClickBtn = false;
        connectTime = 0;
        EventDispatcher.AddObserver(this, EventName.SetJump_level, (object[] userInfo) =>
        {
            string jumpLv = userInfo[0] as string;
            this.jump_level = jumpLv;
            return false;
        }, null);
    }

    #region 机器人
    public int bot_number = -1; //总共有多少机器人
    #endregion

    #region IP
    public string photonIp = "";//光子Ip
    public string country = "";//国家
    public string region = "";//省份
    public string myRegion = "";//排行榜省份，非真实省份
    public string city = "";//城市
    public bool isAutoSelectIp = true;
    public string IPAdress = "";
    public IPMenu IpSelect = IPMenu.IpHangZhouDevelop;
    public int CanPassMB = 0;
    #endregion

    public string version
    {
        get
        {
            return Application.version;
        }
    }


    public PlatForm Platflam()
    {
#if UNITY_IOS || UNITY_IPHONE || UNITY_EDITOR_IOS
        return PlatForm.IOS;
#elif UNITY_ANDROID && !UNITY_EDITOR
        return PlatForm.Android;
#else
        return PlatForm.Win;
#endif
        return PlatForm.Android;
    }

    public string appsFlyerId = "";

    private string _deviceId = "";

    public string DeviceId()
    {
        if (this._deviceId != "")
        {
            return this._deviceId;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        // AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        // AndroidJavaClass jc2 = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdclient");
        // AndroidJavaObject jo2 = jc2.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", jo);
        // if (jo2 != null)
        // {
        //     //获取广告id:
        //     _deviceId = jo2.Call<string > ("getId");
        //     if (string.IsNullOrEmpty(_deviceId))
        //     {
        //         
        //         Debug.LogError("获取不到GAID，采用原始签名设备Id:" + _deviceId);
        //         _deviceId = SystemInfo.deviceUniqueIdentifier;
        //     }
        //
        //     //获取广告跟踪状态:当为false时，则无法根据用户行为定向推送广告，但看到的广告数量并不会减少
        //     var adTrackLimited = jo2.Call<bool > ("isLimitAdTrackingEnabled");
        // }
        // Debug.LogError("GAID:" + _deviceId);
        // return _deviceId;
        this._deviceId = SystemInfo.deviceUniqueIdentifier;
        return _deviceId;
#else
        this._deviceId = SystemInfo.deviceUniqueIdentifier;
        return _deviceId;
#endif
    }

    void OnDestroy()
    {
        EventDispatcher.RemoveObserver(EventName.SetJump_level);
    }
}

public enum PlatForm
{
    Win = 0,
    Android = 1,
    IOS = 2,
}


/// <summary>
/// google pay init data
/// </summary>
public class GooglePayData
{
    /// <summary>
    /// 商品ID
    /// </summary>
    public string productId;
    /// <summary>
    /// 本地化价格字符串
    /// </summary>
    public string localizedPriceString;
    /// <summary>
    /// 本地化标题
    /// </summary>
    public string localizedTitle;
    /// <summary>
    /// 本地化描述
    /// </summary>
    public string localzedDescription;
    /// <summary>
    /// 等值货币代码
    /// </summary>
    public string isoCurrencyCode;
    /// <summary>
    /// 本地化价格
    /// </summary>
    public decimal localizedPrice;
    /// <summary>
    /// 收据
    /// </summary>
    public string receipt;
    /// <summary>
    /// 开关
    /// </summary>
    public string enabled;
}

public enum IPMenu
{
    IpHangZhouDevelop = 0,
    SingaporeProd,
    IPSingapore,
    IPWrite,
}
