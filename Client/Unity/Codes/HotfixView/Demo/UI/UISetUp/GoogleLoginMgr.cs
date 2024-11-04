using System.Reflection;
using UnityEngine;
using  UnityEngine.SceneManagement;
namespace ET
{
    public class GoogleLoginMgr
    {
        private static GoogleLoginMgr _instance;
        public static GoogleLoginMgr GetInstance()
        {
            if (_instance == null)
                _instance = new GoogleLoginMgr();
            return _instance;
        }

        public void Init()
        {
            AddListner();
        }

        public void AddListner()
        {
            EventDispatcher.AddObserver(this, EventName.GoogleLoginSuc, (object[] userInfo) =>
            {
                Log.Info("send google login result to server");
                //GameHelper.Instance.GoogleOrIosUid = userInfo[0].ToString();
                GoogleLoginSuc(userInfo[0].ToString());
                return false;
            }, null);
            EventDispatcher.AddObserver(this,ETEventName.LoginServerFinish, (object[] userInfo) =>
            {
                //暂时注释
                // Log.Info("login server finish start check google login state");
                // this.CheckLoginState();
                return false;
            },null);
        }

        public void CheckLoginState()
        {
            CheckAndroidLoginState(); 
        }
        /// <summary>
        /// 检测是否需要登录google
        /// </summary>
        private void CheckAndroidLoginState()
        {
            Log.Info("check android login state");
            string googleUid = PlayerPrefs.GetString("googleUid");
            if (string.IsNullOrEmpty(googleUid))
            {
                Log.Info("send google login");
                EventDispatcher.PostEvent(EventName.ClickGoogleLogin,null);
            }
        }

        private void GoogleLoginSuc(string uid)
        {
            //向服务器查询是否该GoogleID是否已绑定了玩家
            if (PlatForm.IOS == GameDataMgr.Instance.Platflam())
            {
                SendLogin(uid,2);     
            }
            else
            {
                SendLogin(uid,1);
            }
        }

        /// <summary>
        /// type:1 = google登录  2 = ios 登录
        /// </summary>
        public async void SendLogin(string uid,int type)
        {
            C2M_StartBindByOther cmd = new C2M_StartBindByOther();
            cmd.Uid = uid;
            cmd.BindType = type;
            M2C_StartBindByOther message = (M2C_StartBindByOther) await HallHelper.gateSession.Call(cmd);
            Log.Info("找回账号：" + message.AccountName);
            Log.Info("当前账号：" + UnityEngine.PlayerPrefs.GetString(ItemSaveStr.account));
            string text  = "You have an existing account with us. Load progress?";;
            //找到已绑定账号,发起重新登录
            if (!string.IsNullOrEmpty(message.AccountName) && message.AccountName != UnityEngine.PlayerPrefs.GetString(ItemSaveStr.account))
            {
                UIHelper.ShowPop(GlobalComponent.Instance.scene,text, () =>
                {
                    OnReloginGame(message.AccountName);
                    //Relogin(message.AccountName);
                },false,null,"",UILayer.High);
            }
        }
        /// <summary>
        /// 获得绑定信息
        /// </summary>
        public async void GetBindInfo()
        {
            C2M_BindOtherInfo cmd = new C2M_BindOtherInfo();
            M2C_BindOtherInfo message = (M2C_BindOtherInfo) await HallHelper.gateSession.Call(cmd);
            GoogleLoginDataMgr.GetInstance().GoogleOrIosUid = message.Uid;
            EventDispatcher.PostEvent(EventName.GetAndriodOrIosLoginInfo,this,message.Uid);
            await ETTask.CompletedTask;
        }


        /// <summary>
        /// 退出游戏，重新加载游戏
        /// </summary>
        /// <param name="account"></param>
        public async void OnReloginGame(string account = "")
        {
            Log.Info("退出游戏,重新登录");
            HallHelper.gateSession?.Dispose();
            EventDispatcher.RemoveObserver(EventName.DownLoadFinish);
            EventDispatcher.RemoveObserver(EventName.ShowGoolgeAds);
            EventDispatcher.RemoveObserver(EventName.WaitAdTimeOut);
            if (!string.IsNullOrEmpty(account))
            {
                int guideStep = PlayerPrefs.GetInt(ItemSaveStr.GuideStep, 1);
                PlayerPrefs.DeleteAll();
                AppInfoComponent.Instance.isNewDevice = 2;
                AppInfoComponent.Instance.loginStep = 0;
                PlayerPrefs.SetInt(ItemSaveStr.GuideStep, guideStep);
                UnityEngine.PlayerPrefs.SetString(ItemSaveStr.account, account);
            }
            
            ResourcesComponent.Instance.SetReloginMaskShow();
            await ResourcesComponent.Instance.UnLoadAllBundle();
            await TimerComponent.Instance.WaitAsync(1000);
            Game.Scene.GetComponent<SoundComponent>().RemoveListner();
            Game.Scene.RemoveComponent<SoundComponent>();
            Game.Close();
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
            //
            // RankHelper.GetInstance().RemoveListner();
            // EventDispatcher.RemoveObserver(EventName.DownLoadFinish);
            // EventDispatcher.RemoveObserver(EventName.ShowGoolgeAds);
            // EventDispatcher.RemoveObserver(EventName.WaitAdTimeOut);
            // Log.Info("退出游戏,重新登录");
            // HallHelper.gateSession?.Dispose();
            // int guideStep = PlayerPrefs.GetInt(ItemSaveStr.GuideStep, 1);
            // PlayerPrefs.DeleteAll();
            // AppInfoComponent.Instance.isNewDevice = 2;
            // AppInfoComponent.Instance.loginStep = 0;
            // PlayerPrefs.SetInt(ItemSaveStr.GuideStep, guideStep);
            // UnityEngine.PlayerPrefs.SetString(ItemSaveStr.account, account);
            // ResourcesComponent.Instance.SetReloginMaskShow();
            // await ResourcesComponent.Instance.UnLoadAllBundle();
            // await TimerComponent.Instance.WaitAsync(1000);
            // Game.Close();
            // SceneManager.LoadScene("Start", LoadSceneMode.Single);
            // EventDispatcher.PostEvent(EventName.PhotonCannotConnect,this,1,true);
        }   
    }
}