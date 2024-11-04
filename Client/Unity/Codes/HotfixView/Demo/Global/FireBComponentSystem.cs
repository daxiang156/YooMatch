using System;
using UnityEngine;
using Random = System.Random;

namespace ET
{
    [ObjectSystem]
    public class FireBComponentSystem: AwakeSystem<FireBComponent>
    {
        public override void Awake(FireBComponent self)
        {
            FireBComponent.Instance = self;

            // self.initFinish = 0;
            // try
            // {
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.first_time_player_experien, FirebaseSetup.Instance.RCJ.first_enterGame);
            //     //游戏内
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.game_ad_move, 3);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.game_ad_shuffle, 3);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.game_ad_undo, 4);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.game_ad_revive_num, 3001);
            //     //结算
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.result_ad_move, 2);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.result_ad_shuffle, 2);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.result_ad_undo, 2);
            //     //关卡界面
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.map_ad_move, 2);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.map_ad_shuffle, 2);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.map_ad_undo, 2);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.map_ad_life, 1);
            //
            //     //关卡界面
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.init_move, 1);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.init_shuffle, 1);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.init_undo, 1);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.init_life, 1);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.init_coin, 2000);
            //     // if(GameDataMgr.Instance.Platflam() == PlatForm.IOS)
            //     //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.init_diamond, 10);
            //     // else
            //         FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.init_diamond, 1);
            //     
            //     //关卡界面
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.ad_max_bigturn_percent, 0);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.ad_max_game_percent, 0);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.ad_max_map_percent, 0);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.ad_max_result_percent, 0);
            //     
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.ui_levelup_level, 2);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.mb_btnreturn_show, 0);
            //
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.mb_pvp_total_time, 120);//FirebaseSetup.Instance.RCJ.pvpTime);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.mb_level_ab, 1);
            //     
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.can_pvp, 1);
            //
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.skin_property, "2000:1:0:0,1000:1:0:0,3005:0:1:0,3008:0:1:1,3001:1:1:1,6001:1:1:2,3009:1:2:1,3002:2:2:2,1003:3:2:1,2003:3:1:2,2004:1:2:3,3003:2:2:3,3004:3:2:2,3006:2:3:3,1004:3:3:3,3007:4:4:4");
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.skin_lv_unlock, "2:2000,3:1000,5:1003,7:2003,10:3003,15:3004,20:3005,25:3001,30:6001,35:3002,40:2004,45:1004,50:3006,55:3007,60:3008,65:3009");
            //
            //     
            //     
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.fail_reduce_num, 2010);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.load_bg_icon_inMB, 1);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.free_item_level, "1:0:0:0,2:0:0:0,3:1:1:1,4:1:1:1,5:3:3:3,6:1:1:1,7:1:1:1,8:1:1:1,9:2:2:2,10:3:3:3,11:1:1:1,12:1:1:1,13:1:1:1,14:1:1:1,15:1:1:1,16:1:1:1,17:1:1:1,18:1:1:1,19:1:1:1,20:1:1:1,21:1:1:1,22:1:1:1,23:1:1:1,24:1:1:1,25:1:1:1");
            //     
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.ad_times_per_game, 99999);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.coin_shop_reveive, 1);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.gold_level, "6:11:16:21");
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.jump_level, "");
            //     
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.pvp_single, 1);//1单机，2联机
            //     
            //    // FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.fruit_select, 0);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.BackgroundRestart, 5);
            //     FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.fail_reduce_score, -10);
            //     //int version = int.Parse(GameDataMgr.Instance.version.Split('.')[2]);
            //     // if(version > 21)
            //          FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.level_difficulty, "1:1000,2:1000");
            //     // else
            //     // {
            //     //    FirebaseSetup.Instance.RCJ.SetDefaultValue(FireBRemoteName.level_difficulty, "1:1000,2:500,3:500,4:600,5:600,6:700,7:700,8:800,9:800,10:900,11:1000");
            //     //}
            //
            //     //if (!GameDataMgr.Instance.isRemoteConfig)
            //     if (GameDataMgr.Instance.Platflam() == PlatForm.Win)
            //     {
            //         self.initFinish = 1;
            //         return;
            //     }
            //
            //     //if (GameDataMgr.Instance.Platflam() != PlatForm.IOS)
            //     {
            //         FirebaseSetup.Instance.RCJ.InitializeRemoteConfig();
            //         FirebaseSetup.Instance.RCJ.SetDefaultsSetUpAsync(() =>
            //         {
            //             Log.Console("FireBase Remote success");
            //             self.initFinish = 1;
            //         });
            //     }
            // }
            // catch (Exception e)
            // {
            //     Log.Error(e.Message);
            // }
        }
    }
    
    public static class FireBSystem
    {
        public static int GetRemoteLong(this FireBComponent self, string key)
        {
            try
            {
                // if (GameDataMgr.Instance.Platflam() == PlatForm.IOS)
                //     return (int) FirebaseSetup.Instance.RCJ.defaults[key];
                // else
                return 1;//(int) FirebaseSetup.Instance.RCJ.GetRemoteLong(key);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                //if (FirebaseSetup.Instance.RCJ == null)
                    return 3001;
                //else
                //    return (int) FirebaseSetup.Instance.RCJ.defaults[key];
            }
        }
        
        public static string GetRemoteString(this FireBComponent self, string key)
        {
            try
            {
                // if (GameDataMgr.Instance.Platflam() == PlatForm.IOS)
                //     return (string) FirebaseSetup.Instance.RCJ.defaults[key];
                // else
                return "";//FirebaseSetup.Instance.RCJ.GetRemoteString(key);
            }
            catch (Exception e)
            {
                 Log.Error(e.Message);
                 //if (FirebaseSetup.Instance.RCJ != null && FirebaseSetup.Instance.RCJ.defaults.ContainsKey(key))
                 //   return (string)FirebaseSetup.Instance.RCJ.defaults[key];
                 //else
                 {
                     return "";
                 }
            }
        }

        // public static AdPlatform AdSelect(this FireBComponent self, string key)
        // {
        //     int randomNum = self.GetRemoteLong(key);
        //     if (randomNum > UnityEngine.Random.Range(1, 100))
        //     {
        //         return AdPlatform.MaxAppLovin;
        //     }
        //     return AdPlatform.Google_AdMob;
        // }
    }
    
    [ObjectSystem]
    public class FireBDestroySystem: DestroySystem<FireBComponent>
    {
        public override void Destroy(FireBComponent self)
        {
        }
    }
}