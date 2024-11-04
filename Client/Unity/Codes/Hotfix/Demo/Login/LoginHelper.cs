using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
//using System.Security.Policy;
using System.Text;
using ILRuntime.Runtime;
using UnityEngine;
using UnityEngine.tvOS;
using static UnityEngine.Networking.UnityWebRequest;

namespace ET
{
    public class ClientServerList
    {
        public string ClientVersion;
        public string AndroidDownLoad;
        public string IosDownLoad;
        public List<ServerData> ServerList;
        public string IsCheckVersion;
        public string CheckVersion;
        public string ClientIOSVersion;
    }
    public class ServerData
    {
        public string ServerIp;
        public string ServerName;
    }
    /// <summary>
    /// login account server logic
    /// </summary>
    public static class LoginHelper
    {
        /// <summary>
        /// 登录账号管理服，如果账号服返回的AccountSkinId == 0 需要选择默认皮肤
        /// </summary>
        public static Session accountSession = null;
        public static async ETTask<int> Login(Scene zoneScene, string address, long account, string password, bool isReLogin = false)
        {
            await ETTask.CompletedTask;
            return 0;
            //Session realSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint("114.55.11.158:9901"));
            
            // TimeHelper.ExecutionTime("startConnectServer::",1);
            // GameDataMgr.isLoginFinish = false;
            // string reslut = "";
            // //account = 1689549470068;//正式服账号
            // try
            // {
            //     if(!GameDataMgr.Instance.isConnecting && !isReLogin){
            //         await Game.EventSystem.PublishAsync(new EventType.CreateTpsControl()
            //         {
            //             ZoneScene = zoneScene,
            //         });
            //         Log.Console("UILoading");
            //         if (AppInfoComponent.Instance.enterType == KeyDefine.enterMainCity)
            //         {
            //             await Game.EventSystem.PublishAsync(new EventType.LoadingBegin()
            //             {
            //                 Scene = zoneScene,
            //             });
            //         }
            //     }
            //     EventDispatcher.PostEvent(EventName.ShowInitBg, null, false);
            //     ClientServerList clientServerList;
            //     string[] ips = ConstValue.LoginAddress.Split(':');
            //     string url = "http://" + ips[0]+ ":10010/GetClientVersion";
            //     HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
            //     wbRequest.Proxy = null;
            //     wbRequest.Method = "GET";
            //     HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
            //     
            //     //EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Event_Version_Get);
            //     using (Stream responseStream = wbResponse.GetResponseStream())
            //     {
            //         using (StreamReader sReader = new StreamReader(responseStream))
            //         {
            //             reslut = sReader.ReadToEnd();
            //             Log.Console(reslut);
            //             Log.Console("cur version:" + GameDataMgr.Instance.version);
            //             clientServerList = LitJson.JsonMapper.ToObject<ClientServerList>(reslut);
            //
            //
            //             if (GameDataMgr.Instance.Platflam() == PlatForm.IOS)
            //             {
            //                 if (GameDataMgr.Instance.version == clientServerList.CheckVersion)
            //                 {
            //                     HallInfoComponent.Instance.isCheckVersion = int.Parse(clientServerList.IsCheckVersion);
            //                     Log.Console("IOS isCheckVersion:" + HallInfoComponent.Instance.isCheckVersion);
            //                 }
            //                 else
            //                 {
            //                     HallInfoComponent.Instance.isCheckVersion = 0;
            //                 }
            //             }
            //             else if (GameDataMgr.Instance.Platflam() == PlatForm.Android)
            //             {
            //                 HallInfoComponent.Instance.isCheckVersion = 0;
            //                 Log.Console("Android isCheckVersion:" + HallInfoComponent.Instance.isCheckVersion);
            //             }
            //             else
            //             {
            //                 HallInfoComponent.Instance.isCheckVersion = 0;
            //                 Log.Console("else isCheckVersion:" + HallInfoComponent.Instance.isCheckVersion);
            //             }
            //
            //             string SerVersion = clientServerList.ClientVersion;
            //             if (GameDataMgr.Instance.Platflam() == PlatForm.IOS)
            //             {
            //                 SerVersion = clientServerList.ClientIOSVersion;
            //             }
            //             if (GameDataMgr.Instance.version != SerVersion)
            //             {
            //                 Debug.Log("服务端下发的版本号：" + SerVersion);
            //                 Debug.Log("本地的版本号：" + GameDataMgr.Instance.version);
            //                 AppInfoComponent.Instance.downloadUrl = clientServerList.AndroidDownLoad;
            //                 if (GameDataMgr.Instance.Platflam() == PlatForm.Android)
            //                 {
            //                     AppInfoComponent.Instance.downloadUrl = clientServerList.AndroidDownLoad;
            //                 }
            //                 else if (GameDataMgr.Instance.Platflam() == PlatForm.IOS)
            //                 {
            //                     AppInfoComponent.Instance.downloadUrl = clientServerList.IosDownLoad;
            //                 }
            //                 else
            //                 {
            //                     AppInfoComponent.Instance.downloadUrl = clientServerList.AndroidDownLoad;
            //                 }
            //                 
            //                 string[] serVersion = SerVersion.Split('.');
            //                 string[] clientVersion = GameDataMgr.Instance.version.Split('.');
            //                 int serBig = int.Parse(serVersion[1]);
            //                 int clientBig = int.Parse(clientVersion[1]);
            //                 int serSmall = int.Parse(serVersion[2]);
            //                 int clientSmall = int.Parse(clientVersion[2]);
            //                 if (serBig > clientBig || serSmall > clientSmall)
            //                 {
            //                     // EventDispatcher.PostEvent(EventName.UpdateVersion, null);
            //                     // return 0;
            //                 }
            //                 else
            //                 {
            //                     //EventDispatcher.PostEvent(EventName.UpdateVersion, null);
            //                     Log.Console("客户端版本号大于服务器版本号，可能是测试包");
            //                 }
            //             }
            //
            //             switch (GameDataMgr.Instance.IpSelect)
            //             {
            //                 case IPMenu.IpHangZhouDevelop:
            //                     ConstValue.LoginAddress = clientServerList.ServerList[0].ServerIp;
            //                     break;
            //                 case IPMenu.IPSingapore:
            //                     ConstValue.LoginAddress = clientServerList.ServerList[1].ServerIp;
            //                     break;
            //                 case IPMenu.SingaporeProd:
            //                     ConstValue.LoginAddress = clientServerList.ServerList[2].ServerIp;
            //                     break;
            //                 case IPMenu.IPWrite:
            //                     //ConstValue.LoginAddress = clientServerList.ServerList[0].ServerIp;
            //                     break;
            //             }
            //             //Log.Console(ConstValue.LoginAddress);
            //         }
            //     }
            // }
            // catch (Exception e)
            // {
            //     Log.Error(e.Message);
            //     //return ErrorCode.ERR_LoginError;
            // }
            //
            // // int waitCount = 0;
            // // while (string.IsNullOrEmpty(GameDataMgr.Instance.appsFlyerId))
            // // {
            // //     await TimerComponent.Instance.WaitAsync(10);
            // //     waitCount++;
            // //     if(waitCount > 20)
            // //         break;
            // // }
            // A2C_LoginAccount a2c_loginAccount = null;
            // accountSession = null;
            // try
            // {
            //     if(!GameDataMgr.Instance.isConnecting)
            //         GameDataMgr.Instance.curConnectType = ConnectType.Logining;
            //     //account = "1653557919";
            //     address = ConstValue.LoginAddress;
            //     Log.Console("连接IP:" + address);
            //     accountSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
            //     //login to account server
            //     string md5PassWord = MD5Helper.StringToMd5(password);
            //     C2A_LoginAccount cmd = new C2A_LoginAccount();
            //     cmd.AccountName = account.ToString();
            //     cmd.Password = md5PassWord;
            //     cmd.DeviceId = GameDataMgr.Instance.DeviceId();
            //     cmd.AppsFlyerId = GameDataMgr.Instance.appsFlyerId;
            //     a2c_loginAccount = (A2C_LoginAccount)await accountSession.Call(cmd);
            //     if(!GameDataMgr.Instance.isConnecting && !isReLogin)
            //         accountSession.AddComponent<PingComponent>();
            //     if (a2c_loginAccount.DeleteTime < 0)
            //     {
            //         Log.Error("账号已经过期：" + a2c_loginAccount.DeleteTime);
            //         System.TimeSpan timeStamp = System.DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            //         long newAccount = timeStamp.TotalSeconds.ToInt64() * 1000 + UnityEngine.Random.Range(0, 999);
            //         PlayerPrefs.SetString(ItemSaveStr.account, newAccount.ToString());
            //         await Login(zoneScene, ConstValue.LoginAddress, newAccount, password, true);
            //         return 0;
            //     }
            //
            // }
            // catch (Exception e)
            // {
            //     if (e.Message.Contains("dispose"))
            //     {
            //         Log.Error("服务器连接不上");
            //         JumpMainCity(zoneScene);
            //     }
            //
            //     Log.Error(e.ToString());
            //     return ErrorCode.ERR_NetworkError;
            // }
            //
            // if (AppInfoComponent.Instance.isNewDevice == 2)
            // {
            //     Log.Console("新设备登录");
            // }
            // else
            // {
            //     if (a2c_loginAccount.LastDeviceId != GameDataMgr.Instance.DeviceId())
            //         AppInfoComponent.Instance.isNewDevice = 1;
            //     if (string.IsNullOrEmpty(GameDataMgr.Instance.DeviceId()) || string.IsNullOrEmpty(a2c_loginAccount.LastDeviceId))
            //     {
            //         AppInfoComponent.Instance.isNewDevice = 1;
            //     }
            //     if(AppInfoComponent.Instance.isNewDevice == 1)
            //         Log.Console("新设备登录");
            // }
            //
            // GameDataMgr.Instance.nickName = a2c_loginAccount.UserName;
            // HallInfoComponent.Instance.NickName = a2c_loginAccount.UserName;
            // if (a2c_loginAccount.Error != ErrorCode.ERR_Success)
            // {
            //     accountSession.Dispose();
            //     await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
            //     {
            //         ZoneScene = zoneScene,
            //         errorId = a2c_loginAccount.Error
            //     });
            //     return a2c_loginAccount.Error;
            // }
            //
            //
            // if (!isReLogin && !GameDataMgr.Instance.isConnecting)
            // {
            //     EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Log_in);
            // }
            //
            // //login suc
            // GameDataMgr.Instance.nickName = a2c_loginAccount.UserName;
            // HallInfoComponent.Instance.NickName = a2c_loginAccount.UserName;
            // AccountInfoComponent accountData = zoneScene.GetComponent<AccountInfoComponent>();
            // accountData.AccountId = a2c_loginAccount.AccountId;
            // accountData.Token = a2c_loginAccount.Token;
            // accountData.Address = a2c_loginAccount.Address;
            // accountData.GateID = a2c_loginAccount.GateId;
            // accountData.AccountSkinId = a2c_loginAccount.AccountSkinId;
            // accountData.account = account.ToString();
            // Log.Console("My Ip:" + a2c_loginAccount.UserIp);
            // EventDispatcher.PostEvent(EventName.IPReady, null);
            //
            // if (GameDataMgr.Instance.curConnectType == ConnectType.SelectRule)
            // {
            //     Log.Console("选角界面重连");
            //     GameDataMgr.Instance.isConnecting = false;
            //     return ErrorCode.ERR_Success;
            // }
            // if (GameDataMgr.Instance.isConnecting)
            // {
            //     if (a2c_loginAccount.AccountSkinId == 0 && AppInfoComponent.Instance.enterType > KeyDefine.enterMainCity)
            //     {
            //         Log.Console("进入选角界面");
            //         if(!GameDataMgr.Instance.isConnecting)
            //             GameDataMgr.Instance.curConnectType = ConnectType.SelectRule;
            //         GameDataMgr.Instance.isConnecting = false;
            //         return ErrorCode.ERR_Success;
            //     }
            //     else
            //     {
            //         if(!GameDataMgr.Instance.isConnecting)
            //             GameDataMgr.Instance.curConnectType = ConnectType.Logined;
            //         HallHelper.ConnectToGate(zoneScene);
            //         return ErrorCode.ERR_Success;
            //     }
            // }
            //
            // var hallInfo = Game.Scene.GetComponent<HallInfoComponent>();
            // if (hallInfo == null)
            // {
            //     hallInfo = Game.Scene.AddComponent<HallInfoComponent>();
            // }
            // hallInfo.playerName = a2c_loginAccount.UserName;
            //
            // //AccountSkinId == 0 表示需要没有默认皮肤需要打开第一次选角界面
            // if (a2c_loginAccount.AccountSkinId == 0) 
            // {
            //     if (AppInfoComponent.Instance.enterType == KeyDefine.enterMainCity)
            //     {
            //         //打开选角界面
            //         await Game.EventSystem.PublishAsync(new EventType.AccountLoginFinish()
            //         {
            //             AccountSkinId = a2c_loginAccount.AccountSkinId, AccountZone = accountData.ZoneScene(),
            //         });
            //         // await Game.EventSystem.PublishAsync(new EventType.LoadingFinish()
            //         // {
            //         //     Scene = zoneScene,
            //         // });
            //     
            //         EventDispatcher.PostEvent(EventName.LoadingCloseListen, null);
            //         EventDispatcher.PostEvent(EventName.ShowInitBg, null, false);
            //         return ErrorCode.ERR_SelectAccountSkin;
            //     }
            //     else
            //     {
            //         SelectAccountSkinId(zoneScene,1,"100" + account, () =>
            //         {
            //             EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Create_account);
            //             EventDispatcher.PostEvent(EventName.CheckEnterMainCityInEnterName, null);
            //         });
            //     }
            // }
            // else
            // {
            //     //string modelName = "Player_6_Modified";
            //     string modelName = SkinConfigCategory.Instance.Get(3003).CityModel;
            //     if (a2c_loginAccount.AccountSkinId == 1)
            //     {
            //         //modelName = "Player_4_modified";
            //         modelName = SkinConfigCategory.Instance.Get(3004).CityModel;
            //     }
            //
            //     EventDispatcher.PostEvent(EventName.ChgModuleId, null, modelName);
            //     await Game.EventSystem.PublishAsync(new EventType.EnterNameFinish()
            //     {
            //         AccountZone = accountData.ZoneScene(),
            //     });
            //     HallHelper.ConnectToGate(zoneScene);
            // }
            //
            // return ErrorCode.ERR_Success;
        }

        public static void LanguageByIp()
        {
            if (GameDataMgr.Instance.languageSelect == LanguageSelect.AutoSelectByIP)
            {
                var lanCountryConfigs = LanCountryConfigCategory.Instance.GetAll();
                bool isFind = false;
                foreach (var VARIABLE in lanCountryConfigs)
                {
                    LanCountryConfig lanCountryConfig = VARIABLE.Value;
                    string[] countrys = lanCountryConfig.countrys.Split(',');
                    for (int i = 0; i < countrys.Length; i++)
                    {
                        if (countrys[i] == GameDataMgr.Instance.country)
                        {
                            AppInfoComponent.Instance.languageSelect = (LanguageSelect2)Enum.Parse(typeof(LanguageSelect2), lanCountryConfig.language);
                            GameDataMgr.Instance.languageSelect = (LanguageSelect)Enum.Parse(typeof(LanguageSelect2), lanCountryConfig.language);
                            Log.Console("语言：" + GameDataMgr.Instance.languageSelect);
                            isFind = true;
                            break;
                        }
                    }
                    if(isFind)
                        break;
                }
                if(!isFind)
                {
                    AppInfoComponent.Instance.languageSelect = LanguageSelect2.English;
                    Log.Console("没有找到语言：" + GameDataMgr.Instance.languageSelect);
                }
            }
        }

        public static async void JumpMainCity(Scene zoneScene)
        {
            if(AppInfoComponent.Instance.enterType != KeyDefine.enterMainCity)
                return;
            if(CommonFuc.CurScene() != "Start")
                return;
            Log.Console("跳转进入主城");
            if (GameDataMgr.Instance.isConnecting == false)
            {
                var cameraObj = Camera.main.gameObject;
                var gameInfo = Game.Scene.GetComponent<HallInfoComponent>();
                if (gameInfo == null)
                {
                    gameInfo = Game.Scene.AddComponent<HallInfoComponent>();
                }

                gameInfo.cameraObj = cameraObj;
                // await Game.EventSystem.PublishAsync(new EventType.CreateTpsControl()
                // {
                //     ZoneScene = zoneScene,
                // });;
                EventDispatcher.PostEvent(EventName.ShowInitBg, null, false);
                int sceneId = 3;
                MapConfig config = MapConfigCategory.Instance.Get(sceneId);
                Log.Console("回主城8");
                await SceneChangeHelper.SceneChangeTo(zoneScene, config.source, config.Id);
                if(!GameDataMgr.Instance.isConnecting)
                    GameDataMgr.Instance.curConnectType = ConnectType.EnterGamed;
                EventDispatcher.PostEvent(EventName.CameraRendMode, null, true);
            }

            if (AppInfoComponent.Instance.singleState == SingleState.none)
            {
                await Game.EventSystem.PublishAsync(new EventType.CreateUIMainCity() { ZoneScene = zoneScene });
            }

            AppInfoComponent.Instance.singleState = SingleState.singled;
            GameDataMgr.Instance.isConnecting = false;
            if(accountSession != null && !accountSession.IsDisposed)
                accountSession.Dispose();
            
            //EventDispatcher.PostEvent(EventName.CloseWaitingUI, null, false);
            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 选择皮肤 选择成功后会重新发登录帐号服成功的消息
        /// </summary>
        /// <returns></returns>
        public static async void SelectAccountSkinId(Scene zoneScene,int SkinId,string name, Action callback = null)
        {
            if (accountSession == null)
                return;
            AccountInfoComponent accountData = zoneScene.GetComponent<AccountInfoComponent>();
            C2A_SelectAccountSkin cmd = new C2A_SelectAccountSkin();
            cmd.AccountSkinId = SkinId;
            cmd.AccountId = accountData.AccountId;
            cmd.name = name;
            //select accountSkin finish
            A2C_SelectAccountSkin  message = await accountSession.Call(cmd) as A2C_SelectAccountSkin;
            
            if (message.Error != ErrorCode.ERR_Success)
            {
                await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
                {
                    ZoneScene = zoneScene,
                    errorId = message.Error
                });
                return;
            }

            if (AppInfoComponent.Instance.enterType == KeyDefine.enterMainCity)
            {
                await Game.EventSystem.PublishAsync(new EventType.LoadingBegin()
                {
                    Scene = zoneScene,
                });
            }
            if (callback != null)
                callback();
            GameDataMgr.Instance.nickName = name;
            HallInfoComponent.Instance.NickName = name;
            var hallInfo = Game.Scene.GetComponent<HallInfoComponent>();
            if (hallInfo == null)
            {
                hallInfo = Game.Scene.AddComponent<HallInfoComponent>();
            }

            hallInfo.playerName = message.name;
            
            if (message.AccountSkinId == 0)
                return;
            accountData.AccountSkinId = message.AccountSkinId;
            accountData.AccountName = message.name;
            //string modelName = "Player_6_Modified";
            // string modelName = SkinConfigCategory.Instance.Get(3003).CityModel;
            // if (message.AccountSkinId == 1)
            // {
            //     //modelName = "Player_4_modified";
            //     modelName = SkinConfigCategory.Instance.Get(3004).CityModel;
            // }

            //EventDispatcher.PostEvent(EventName.ChgModuleId, null, modelName);
            //皮肤选完
            await Game.EventSystem.PublishAsync(new EventType.EnterNameFinish()
            {
                AccountZone = accountData.ZoneScene(),
            });
            
            HallHelper.ConnectToGate(zoneScene);
            //SelectGameId(zoneScene, 1);
        }
        /// <summary>
        /// 选择游戏 发往account 会返回连接real服务器的token和gateID
        /// </summary>
        // public static async void SelectGameId(Scene zoneScene,int gameId)
        // {
        //     if (accountSession == null)
        //         return;
        //     if (AppInfoComponent.Instance.enterType == KeyDefine.enterMainCity)            
        //         EventDispatcher.PostEvent(EventName.CameraRendMode, null, true);
        //
        //     AccountInfoComponent accountData = zoneScene.GetComponent<AccountInfoComponent>();
        //     C2A_GetRealmKey cmd = new C2A_GetRealmKey();
        //     cmd.ServerId = gameId;
        //     cmd.Token = accountData.Token;
        //     cmd.AccountId = accountData.AccountId;
        //     A2C_GetRealmKey message = await accountSession.Call(cmd) as A2C_GetRealmKey;
        //     if (message.Error != ErrorCode.ERR_Success)
        //     {
        //         accountSession.Dispose();
        //         await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
        //         {
        //             ZoneScene = zoneScene,
        //             errorId = message.Error
        //         });
        //         return;
        //     }
        //     accountSession.Dispose();
        //     if (string.IsNullOrEmpty(message.RealmKey))
        //         return;
        //
        //     
        //     accountData.realToken = message.RealmKey;
        //     accountData.realAddress = message.RealmAddress;
        //     
        //     //connect to realserver
        //     ConnectToRealm(zoneScene,message.RealmAddress);
        // }

        /// <summary>
        /// 连接负载均衡服务器
        /// </summary>
        // public static async void ConnectToRealm(Scene zoneScene, string address)
        // {
        //     //await TimerComponent.Instance.WaitAsync(70000000);
        //     if(!GameDataMgr.Instance.isConnecting)
        //         GameDataMgr.Instance.curConnectType = ConnectType.RealConnecting;
        //     AccountInfoComponent accountInfo = zoneScene.GetComponent<AccountInfoComponent>();
        //     Session realSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
        //     R2C_LoginRealm message = null;
        //     try
        //     {
        //         message = (R2C_LoginRealm)await realSession.Call(new C2R_LoginRealm()
        //         {
        //             AccountId = accountInfo.AccountId,
        //             RealmTokenKey = accountInfo.realToken,
        //         });
        //         if (message.Error != ErrorCode.ERR_Success)
        //         {
        //             accountSession.Dispose();
        //             await Game.EventSystem.PublishAsync(new EventType.TipErrorCode()
        //             {
        //                 ZoneScene = zoneScene,
        //                 errorId = message.Error
        //             });
        //             return;
        //         }
        //
        //     }
        //     catch (Exception e)
        //     {
        //         if(e.Message.Contains("dispose"))
        //             EventDispatcher.PostEvent(EventName.RealErrCatch, null, ConnectType.ConnectRealError);
        //         Log.Error(e.ToString());
        //         return;
        //     }
        //
        //     if (message.Error != ErrorCode.ERR_Success)
        //     {
        //         return;
        //     }
        //     if(!GameDataMgr.Instance.isConnecting)
        //         GameDataMgr.Instance.curConnectType = ConnectType.RealConnected;
        //     realSession.Dispose();
        //
        //     accountInfo.GateAdress = message.GateAddress;
        //     accountInfo.GateToken = message.GateSessionKey;
        //     Log.Info("连接负载均衡服务器成功,开始连接网关");
        //     //connect to gate
        //     HallHelper.ConnectToGate(zoneScene);
        // }
    }
}