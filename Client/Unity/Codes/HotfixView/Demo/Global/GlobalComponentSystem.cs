using UnityEngine;
using Random = UnityEngine.Random;
using System;
using ILRuntime.Runtime;

namespace ET
{
    [ObjectSystem]
    public class GlobalComponentAwakeSystem: AwakeSystem<GlobalComponent>
    {
        public override void Awake(GlobalComponent self)
        {
            GlobalComponent.Instance = self;
            
            self.Global = GameObject.Find("/Global").transform;
            self.Unit = GameObject.Find("/Global/Unit").transform;
            self.UI = GameObject.Find("/Global/UI").transform;
            self.eventSystem = self.Global.Find("EventSystem");
            self.mainCamera = self.Global.Find("MainCamera");
            self.UICamera = self.Global.Find("UICamera");
            //string account = UnityEngine.PlayerPrefs.GetString(ItemSaveStr.account);

            EventDispatcher.AddObserver(this, EventName.ServerNotice, (object[] paras)=>
            {
                if (self.noticeTime > 0)
                    return false;
                string str = (string)paras[0];
                self.noticeTime = 1;
                this.NoticeHandle(str);
                return false;
            }, null);
            
            
            EventDispatcher.AddObserver(this, EventName.ForceDisconnectAfter1Min, (object[] paras) =>
            {
                HallHelper.gateSession.Dispose();
                ReConnectMySer(null);
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.ConnectMySerInGame, (object[] paras) =>
            {
                if (GameDataMgr.Instance.curConnectType == ConnectType.None || GameDataMgr.Instance.isConnecting)
                    return false;
                if (HallHelper.gateSession != null && !HallHelper.gateSession.IsDisposed)
                {
                    return false;
                }
                switch (GameDataMgr.Instance.curConnectType)
                {
                    case ConnectType.WinBe:
                        EventDispatcher.PostEvent(EventName.CloseWinBe, this);
                        break;
                    case ConnectType.Win:
                        EventDispatcher.PostEvent(EventName.CloseWin, this);
                        break;
                    case ConnectType.Matching:
                        this.ConnectInMatching();
                        break;
                }

                if (LoginHelper.accountSession != null && !LoginHelper.accountSession.IsDisposed &&
                    (UIHelper.Get(GlobalComponent.Instance.scene, UIType.UISelectHero) != null || UIHelper.Get(GlobalComponent.Instance.scene, UIType.UIEnterName) != null))
                {
                    Log.Console("处于选择角色取名，无需重连");
                    return false;
                }

                if (HallHelper.gateSession == null || HallHelper.gateSession.IsDisposed)
                {
                    ReConnectMySer(null);
                }
                return false;
            }, null);

            EventDispatcher.AddObserver(this, EventName.DisPhotonTis, (object[] paras) =>
            {
                this.ShowConnectintTips();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.ShowTips, (object[] paras) =>
            {
                string txt = (string) paras[0];
                UIHelper.ShowTip(GlobalComponent.Instance.scene, txt);
                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, EventName.ApplicationFocus, (object[] paras) =>
            {
                bool focus = (bool) paras[0];
                bool mustConnect = (bool) paras[1];
                if (GameDataMgr.Instance.curConnectType == ConnectType.None)
                {
                    Log.Console("GameDataMgr.Instance.curConnectType == ConnectType.None");
                    return false;
                }

                if (AppInfoComponent.Instance.singleState == SingleState.singling)
                {
                    Log.Console("准备单机模式");
                    return false;
                }
                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, EventName.GetGameCoin, (object[] userInfo) =>
            {
                var info = Game.Scene.GetComponent<HallInfoComponent>();
                if (info == null)
                    return false;
                ET.EventDispatcher.PostEvent(EventName.UpdateGameCoin, this, info.gameCoin, info.platFormCoin);
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.GoogleAdLoadFail, (object[] userInfo) =>
            {
                Log.Console("广告失败");
                //UIHelper.ShowTip(GlobalComponent.Instance.scene, "Load Ad Failed!");
                EventDispatcher.PostEvent(EventName.NetWaitUI, null, false);
                return false;
            }, null);
            
            EventDispatcher.AddObserver(this, EventName.ChargeToSer, (object[] userInfo) =>
            {
                string payId = (string)userInfo[0];
                string key = (string) userInfo[1];
                string price = (string) userInfo[2];
                string isoCurrencyCode = (string) userInfo[3];
                
                var chargeList = RechargeConfigCategory.Instance.GetAll();
                int quantity = 1;   //购买次数
                // if (GameDataMgr.Instance.Platflam() == PlatForm.Android)
                // {
                //     Log.Console("安卓");
                //     //key = "{\"Payload\":\"{\\\"json\\\":\\\"{\\\\\\\"orderId\\\\\\\":\\\\\\\"GPA.3355-7591-9594-63022\\\\\\\",\\\\\\\"packageName\\\\\\\":\\\\\\\"com.adslego.arvoland\\\\\\\",\\\\\\\"productId\\\\\\\":\\\\\\\"001\\\\\\\",\\\\\\\"purchaseTime\\\\\\\":1679943166919,\\\\\\\"purchaseState\\\\\\\":0,\\\\\\\"purchaseToken\\\\\\\":\\\\\\\"ekflbklnjbjnmdcbnfmgnbeh.AO-J1Oy14g9jpOcF8t8aM3r2RrxyNHR1CV6X_87J8pKXbIv7STM2E8eQAAkbVkGAX5HvGxfjttLBf4dM3KD52MN8oQGdQRJnkw\\\\\\\",\\\\\\\"quantity\\\\\\\":1,\\\\\\\"acknowledged\\\\\\\":false}\\\",\\\"signature\\\":\\\"A9BHlfMnooY3IHZoOW+CrqEtfBq2E8o1ILJMhvnkseHn4fqskYplJA3Tt9qs3/iR7C4UfpfF/faqP8WLbzBs/CKcfJUWzf+IQU3ooefGThTEqMp/vfRjG8dO80FKmAmCt35Vu6oy1qcOKwjHESekHZzGv2qfSzvM3A6zpyJUaN83poZiuZymtC+UIP/H5xmuP913FD9Keyb4WZ8z5bNPlxAfccbHHZanU7Vr/uc/2tgZR/CL5C8EiotmRhIRbbnrYPaNcupWXxhlBhSxz5+WXUHUe1PXwusP26kL9Mvm7JMHzUfAJyi2TWt2gc5mbvOpI0P7gd9QQ+IGOHi5Xz2xcg==\\\",\\\"skuDetails\\\":[\\\"{\\\\\\\"productId\\\\\\\":\\\\\\\"001\\\\\\\",\\\\\\\"type\\\\\\\":\\\\\\\"inapp\\\\\\\",\\\\\\\"title\\\\\\\":\\\\\\\"Green Gem (Tasty Tile)\\\\\\\",\\\\\\\"name\\\\\\\":\\\\\\\"Green Gem\\\\\\\",\\\\\\\"description\\\\\\\":\\\\\\\"Main currency to purchase skins, power ups\\\\\\\\n\\\\u7eff\\\\u5b9d\\\\u77f3\\\\uff0c\\\\u7528\\\\u6765\\\\u8d2d\\\\u4e70\\\\u76ae\\\\u80a4\\\\u548c\\\\u9053\\\\u5177\\\\\\\",\\\\\\\"price\\\\\\\":\\\\\\\"32,99\\\\u00a0\\\\u0433\\\\u0440\\\\u043d.\\\\\\\",\\\\\\\"price_amount_micros\\\\\\\":32990000,\\\\\\\"price_currency_code\\\\\\\":\\\\\\\"UAH\\\\\\\",\\\\\\\"skuDetailsToken\\\\\\\":\\\\\\\"AEuhp4J2irvGDLW_TG7AFefamaN0xFiIMCY88WeAm-pQWl4O00qJQf4mwHjJc4kiddY=\\\\\\\"}\\\"]}\",\"Store\":\"GooglePlay\",\"TransactionID\":\"ekflbklnjbjnmdcbnfmgnbeh.AO-J1Oy14g9jpOcF8t8aM3r2RrxyNHR1CV6X_87J8pKXbIv7STM2E8eQAAkbVkGAX5HvGxfjttLBf4dM3KD52MN8oQGdQRJnkw\"}";
                //     Log.Console("Key:   " + key);
                //     RechargeData rechargeData = LitJson.JsonMapper.ToObject<RechargeData>(key);
                //     string payload = rechargeData.Payload;
                //     //payload = "{\"json\":\"{\\\"orderId\\\":\\\"GPA.3358-2715-9023-75766\\\",\\\"packageName\\\":\\\"com.adslego.arvoland\\\",\\\"productId\\\":\\\"001\\\",\\\"purchaseTime\\\":1674069558397,\\\"purchaseState\\\":0,\\\"purchaseToken\\\":\\\"jddilmgcigoknadogaedgcnj.AO-J1OyqVzQoPcD5vxMKao8B6ZIfIB7E869PHg3d-9eUcNP5ARUiV161AdIIBJOwCqfkem04_ZdVE4wXtmu_utINonfMgWqgpg\\\",\\\"acknowledged\\\":false}\",\"signature\":\"G+OjFtCkiHbtzVVXylIcOF2mwOh2XNpHJaGQfDLT+MAOmkOmR/c+ZbNwRjiVreHLrMuZOmjhj+lUQvcxufeNI+og4uEPsO92CxlnXymTneF6sMrRG9UxNnH+878MSaxwdYl0lvHTH3JJzq2DUAzJkqFoLgaryred0ChyXX8ofBjFnQ+iqBhs2sWNXu5zryiG+0CLSgajbN0qMNccl35T7zFao4zAltJd2Y70K/uzBi2Viv9RiWvNXnZjM5Pn6nDFdbWMb9nHQIhD7kI4gWOX53PTg1zUsJAsBeM+eJUsViZ2Jjvyi96Jg5YlEFzOn0V5E1LO17zvt8S5WDKHKJwI7A==\",\"skuDetails\":\"{\\\"productId\\\":\\\"001\\\",\\\"type\\\":\\\"inapp\\\",\\\"title\\\":\\\"Green Gem (Adoo)\\\",\\\"name\\\":\\\"Green Gem\\\",\\\"iconUrl\\\":\\\"https:\\\\/\\\\/lh3.googleusercontent.com\\\\/VzVXiHlSOGpGiIAMKNtwHCpzYEwUvK3gaaQ75czYvBEMqPmiH8dzNJ5yvkpZtLaa7AY\\\",\\\"description\\\":\\\"Main currency to purchase skins, power ups\\\\n\\u7eff\\u5b9d\\u77f3\\uff0c\\u7528\\u6765\\u8d2d\\u4e70\\u76ae\\u80a4\\u548c\\u9053\\u5177\\\",\\\"price\\\":\\\"$0.99\\\",\\\"price_amount_micros\\\":990000,\\\"price_currency_code\\\":\\\"USD\\\",\\\"skuDetailsToken\\\":\\\"AEuhp4KzxPtUZM9Ul-xPVFN0roNdwt2M4aZr59Y1p5LD1pyp5XqwAk6v-txstr9yAio=\\\"}\"}";
                //     //rechargeData.Payload = "{\"json\":\"{\\\"orderId\\\":\\\"GPA.3356-2528-4124-41206\\\",\\\"packageName\\\":\\\"com.adslego.arvoland\\\",\\\"productId\\\":\\\"003\\\",\\\"purchaseTime\\\":1677209890225,\\\"purchaseState\\\":0,\\\"purchaseToken\\\":\\\"cangeaegpjkndoeobaokbngc.AO-J1OxZAFCN3FDL6Xs4-O-l5HsXzgjzQslb00k5iTzqGjpkXv_EiGhP92nPiujtC7JJfB3N_z053BXtAFqTR-RPmYgn8isMVg\\\",\\\"quantity\\\":1,\\\"acknowledged\\\":false}\",\"signature\":\"EXL45WmAmXvb3BVDw1OzYQjtfG+L7dTn4GiXv0pSHhKNyP6XJvSinm40auKBgEP0tiNeNqnNoSV4NpWfJFnBqaz2BfazmckIlI0zPHDsir1kmF/HsKEG5+TDbBgBHorBjA1PHz5A3VmOw7P2HKf0oWv+/18NigAECsHg9iBQjvYYnezVHQfVPJH/+BBd8ZGQN3h4hQ+dSpNtvzFgTbugOPKYZNdQVWnhsDwO2pm08VENZiooj10iMYvaQkKYWFXHDZu0bNrQoXSyW6aA8Kk+ZcIZHAOefzGd2lF/JmSs+kz/WuMYUu1PurZm9Y9eyDvIzW294rD4aBZUr55OnEoH+w==\",\"skuDetails\":[\"{\\\"productId\\\":\\\"003\\\",\\\"type\\\":\\\"inapp\\\",\\\"title\\\":\\\"Green Gem (Adoo)\\\",\\\"name\\\":\\\"Green Gem\\\",\\\"description\\\":\\\"Main currency to purchase skins, power ups\\\\n\\u7eff\\u5b9d\\u77f3\\uff0c\\u7528\\u6765\\u8d2d\\u4e70\\u76ae\\u80a4\\u548c\\u9053\\u5177\\\",\\\"price\\\":\\\"$2.99\\\",\\\"price_amount_micros\\\":2990000,\\\"price_currency_code\\\":\\\"USD\\\",\\\"skuDetailsToken\\\":\\\"AEuhp4L89D2D1ttRtXxErtXgyrRD566RHveKGEKY_h8VWy2BHzBETcwAqJcnxYGlC9uH\\\"}\"]}";
                //     AndroidRechargeData androidRechargeData = LitJson.JsonMapper.ToObject<AndroidRechargeData>(rechargeData.Payload);
                //     OrderData orderData = LitJson.JsonMapper.ToObject<OrderData>(androidRechargeData.json);
                //     quantity = orderData.quantity;
                //     Log.Console("订订：" + orderData.orderId);
                //     string lastOrder = PlayerPrefs.GetString(ItemSaveStr.orderId, "");
                //     if (lastOrder == orderData.orderId)
                //     {
                //         Log.Error("android 与上次订单一致，过滤发奖");                        
                //         EventDispatcher.PostEvent(EventName.PayServerCallback, null, payId);
                //         return false;
                //     }
                //     Log.Console("last订：" + lastOrder);
                //
                //     PlayerPrefs.SetString(ItemSaveStr.orderId, orderData.orderId);
                // }else if (GameDataMgr.Instance.Platflam() == PlatForm.IOS)
                // {
                //     RechargeData rechargeData = LitJson.JsonMapper.ToObject<RechargeData>(key);
                //     string lastOrder = PlayerPrefs.GetString(ItemSaveStr.orderId, "");
                //     if (lastOrder == rechargeData.TransactionID)
                //     {
                //         Log.Error("IOS 与上次订单一致，过滤发奖");
                //         return false;
                //     }
                //     Log.Console("last订：" + lastOrder);
                //     PlayerPrefs.SetString(ItemSaveStr.orderId, rechargeData.TransactionID);
                // }
                // else
                //     Log.Error("Edit go");

                
                foreach (var item in chargeList)
                {
                    RechargeConfig rechargeConfig = item.Value;
                    if (rechargeConfig.productId == payId)
                    {
                        string[] itemStr = rechargeConfig.RewardStr.Split(',');
                        for (int i = 0; i < itemStr.Length; i++)
                        {
                            string[] rewards = itemStr[i].Split(':');
                            int itemType = int.Parse(rewards[0]);
                            int itemId = int.Parse(rewards[1]);
                            int itemNum = int.Parse(rewards[2]) * quantity;
                            EventDispatcher.PostEvent(EventName.ChangeItemNum, null, itemId, 0, itemNum);
                        }
                        EventDispatcher.PostEvent(EventName.PayServerCallback, null, payId);
                        EventDispatcher.PostEvent(EventName.FireBEvent, null, FireBEventName.Purchase_Complete_1);

                        //ChargeShake();
                        break;
                    }
                }

                PlayerPrefs.SetString(ItemSaveStr.chargeIds, payId);
                if (!HallHelper.gateSession.IsDisposed)
                {
                    HallHelper.ChargeToSer(self.ZoneScene(), payId, key,price,isoCurrencyCode, () =>
                    {
                        PlayerPrefs.SetString(ItemSaveStr.chargeIds, "");
                        Log.Console("充值Ser返回。。。");
                    });
                }
                return false;
            }, null);
        }

        private async void NoticeHandle(string txt)
        {
            TipInfoComponent data = Game.Scene.GetComponent<TipInfoComponent>();
            if (data == null)
            {
                data = Game.Scene.AddComponent<TipInfoComponent>();
            }
            data.text = txt;
            await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UINotice, UILayer.Mid);
        }

        private async void GamePlayResultHandle(int rank, int gameCoin)
        {
            EventDispatcher.PostEvent(EventName.ChangeItemNum, null, KeyDefine.GameCoin, 0, gameCoin);
            MapConfig mapConfig = MapConfigCategory.Instance.Get(101);
            await SceneChangeHelper.SceneChangeTo(GlobalComponent.Instance.scene, mapConfig.source, mapConfig.Id);
            GlobalComponent.Instance.UI.gameObject.SetActive(true);
            GlobalComponent.Instance.Global.transform.Find("UICamera").gameObject.SetActive(true);
            if (MBDataComponent.Instance.level == MBDataComponent.Instance.curPlayLevel) // && rank == 1)
            {
                MBDataComponent.Instance.level++;
                EventDispatcher.PostEvent(EventName.MBLevelUpdate, null);
            }

           // UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMBWin, UILayer.Mid, 20).Coroutine();
            await ETTask.CompletedTask;
        }

        private async void UnloadBundle()
        {
            await TimerComponent.Instance.WaitAsync(30000);
            Log.Console("UnloadBundle(config.unity3d)");
            ResourcesComponent.Instance.UnloadBundle("config.unity3d");
        }
        
        private async void ReConnectMySer(object[] paras)
        {
            await ETTask.CompletedTask;
            return;
            // if (HallHelper.gateSession != null && !HallHelper.gateSession.IsDisposed)
            // {
            //     HallHelper.gateSession.Dispose();
            //     HallHelper.gateSession = null;
            // }
            //
            // Log.Console("重连");
            // GameDataMgr.Instance.isConnecting = true;
            // //AppInfoComponent.Instance.singleState = SingleState.reSet;
            // string account = UnityEngine.PlayerPrefs.GetString(ItemSaveStr.account);
            // if (account != null && account != "")
            // {
            //     LoginHelper.Login(GlobalComponent.Instance.scene, ConstValue.LoginAddress, long.Parse(account), "").Coroutine();
            // }
            // else{
            //     Log.Error("重连时发现账号是空的");
            //     // 注意, 如果直接使用DateTime.Now, 会有系统时区问题, 导致误差
            //     // System.TimeSpan timeStamp = System.DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            //     // int account0 = timeStamp.TotalSeconds.ToInt32();
            //     // account0 += Random.Range(1, 100000);
            //     // LoginHelper.Login(
            //     //     GlobalComponent.Instance.scene,ConstValue.LoginAddress,account0.ToString(), "").Coroutine();
            // }
            // Log.Console("重新连接登录服");
            // await TimerComponent.Instance.WaitAsync(30000);
            // GameDataMgr.Instance.isConnecting = false;
            // await ETTask.CompletedTask;
        }

        private async void ConnectInMatching()
        {
            
            UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UIConnect).Coroutine();
            UIHelper.Create(GlobalComponent.Instance.scene, UIType.UIMainCity, UILayer.Low).Coroutine();
            int sceneId = 3;
            MapConfig config = MapConfigCategory.Instance.Get(sceneId);
            Log.Console("回主城11");
            await SceneChangeHelper.SceneChangeTo(GlobalComponent.Instance.scene, config.source, config.Id);
        }

        private async void  ShowConnectintTips()
        {
            Log.Error("重连光子，显示加载页");
            if (UIHelper.Get(GlobalComponent.Instance.scene, UIType.UILoading) != null)
            {
                return;
            }

            if (GlobalComponent.Instance.isShowConnectingLoading)
                return;
            GlobalComponent.Instance.isShowConnectingLoading = true;
            await UIHelper.Create(GlobalComponent.Instance.scene, UIType.UILoading, UILayer.Mid);
            await TimerComponent.Instance.WaitAsync(2000);
            await UIHelper.Remove(GlobalComponent.Instance.scene, UIType.UILoading);
            GlobalComponent.Instance.isShowConnectingLoading = false;
            await ETTask.CompletedTask;
        }
    }
    
    [ObjectSystem]
    public class UIGlobalDestroySystem: DestroySystem<GlobalComponent>
    {
        public override void Destroy(GlobalComponent self)
        {
            EventDispatcher.RemoveObserver(EventName.ServerNotice);
            EventDispatcher.RemoveObserver(EventName.DisPhotonTis);
            EventDispatcher.RemoveObserver(EventName.ShowTips);
            EventDispatcher.RemoveObserver(EventName.CheckOurSerConnect);
            EventDispatcher.RemoveObserver(EventName.ApplicationFocus);
            EventDispatcher.RemoveObserver(EventName.GetGameCoin);
            EventDispatcher.RemoveObserver(EventName.ConnectMySerInGame);
            EventDispatcher.RemoveObserver(EventName.ForceDisconnectAfter1Min);
            EventDispatcher.RemoveObserver(EventName.GoogleAdLoadFail);
        }
    }
}