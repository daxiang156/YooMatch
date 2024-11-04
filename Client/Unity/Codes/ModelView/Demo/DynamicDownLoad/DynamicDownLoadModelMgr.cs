using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class DynamicDownLoadModelMgr
    {
        private static DynamicDownLoadModelMgr _instance;

        public static DynamicDownLoadModelMgr GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DynamicDownLoadModelMgr();
                _instance.AddListner();
            }
            return _instance;
        }

        private void AddListner()
        {
            EventDispatcher.AddObserver(this, EventName.GetHeroModel, (object[] userInfo) =>
            {
                Debug.Log("funygame GetHeroModel");
                string abName = userInfo[0].ToString();
                string modeName = userInfo[1].ToString();
                this.GetModel(abName,modeName);
                return false;
            }, null);
        }
        /// <summary>
        /// 提供给光子那边使用
        /// </summary>
        private void GetModel(string abName,string modeName)
        {
            DynamicDownLoadMgr.GetInstance().LoadModel<GameObject>(abName + ".unity3d",modeName,(GameObject model) => {
                Log.Info("funygame get hero finish");
                EventDispatcher.PostEvent(EventName.GetHeroModelFinish,this,model);
            }).Coroutine();
        }

        /// <summary>
        /// 进入游戏就下载
        /// </summary>
        public async ETTask EnterGameLoad()
        {
            await ETTask.CompletedTask;
            // await TimerComponent.Instance.WaitAsync(1);
            // Dictionary<int, SkinConfig> skinDic = SkinConfigCategory.Instance.GetAll();
            // foreach (var value in skinDic.Values)
            // {
            //     if (value.Condition == "3")
            //     {
            //         string abName = value.Name.Substring(0,3) + ".unity3d";
            //         DynamicDownLoadMgr.GetInstance().LoadModel<GameObject>(abName,value.CityModel).Coroutine();
            //     }
            // }
        }

        public void PreLoadModel()
        {
            // int curPassLv = MBDataComponent.Instance.level;
            // Dictionary<int,TaskConfig> taskDic = TaskConfigCategory.Instance.GetAll();
            // foreach (var value in taskDic.Values)
            // {
            //     if (value.type == TaskType.PassLv && value.param1 > curPassLv)
            //     {
            //         string[] rewardStr = value.reward.Split(':');
            //         SkinConfig cfg = SkinConfigCategory.Instance.Get(int.Parse(rewardStr[1]));
            //         if (cfg != null)
            //         {
            //             string abName = cfg.Name.Substring(0,3) + ".unity3d";
            //             DynamicDownLoadMgr.GetInstance().LoadModel<GameObject>(abName,cfg.CityModel).Coroutine();
            //         }
            //         break;
            //     }
            // }
        }
    }
}