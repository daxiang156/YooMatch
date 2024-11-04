using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class UIHelper
    {
        /// <summary>
        /// 默认打开这些界面时收起上下左右侧面UI
        /// </summary>
        private static List<string> expendList = new List<string>()
        {
            UIType.UICharge,UIType.UIShop,UIType.UISetUp,UIType.UIMBLoadingNew,UIType.UIPop,UIType.UIQuest
        };
        /// <summary>
        /// data = 传参
        /// isrepeat = 是否需要重复打开
        /// </summary>
        public static async ETTask<UI> Create(Scene scene, string uiType, UILayer uiLayer,object data = null,bool isRepeat = false, int index = 100,int otherUIAni = 1)
        {
            for (int i = 0; i < expendList.Count; i++)
            {
                if (expendList[i].Equals(uiType))
                {
                    otherUIAni = 0; //收起状态
                    break;
                }
            }

            return await scene.GetComponent<UIComponent>().Create(uiType, uiLayer, data, isRepeat, index,otherUIAni);
        }
        
        public static async ETTask Remove(Scene scene, string uiType,int otherUIAni = 1)
        {
            if (scene == null)
                return;
            for (int i = 0; i < expendList.Count; i++)
            {
                if (expendList[i].Equals(uiType))
                {
                    otherUIAni = 1;     //展开状态
                    break;
                }
            }

            scene.GetComponent<UIComponent>().Remove(uiType,otherUIAni);
            await ETTask.CompletedTask;
        }

        public static UI Get(Scene scene, string uiType)
        {
            return scene.GetComponent<UIComponent>().Get(uiType);
        }

        public static async void ShowPop(Scene scene, string text, Action sureCallback = null, bool isNet = false, Action cancelCallback = null, string title = "",UILayer layer = UILayer.Mid)
        {
            UI ui = UIHelper.Get(scene, UIType.UIPop);
            if (ui == null)
            {
                TipInfoComponent data = Game.Scene.GetComponent<TipInfoComponent>();
                if (data == null)
                {
                    data = Game.Scene.AddComponent<TipInfoComponent>();
                }

                data.isNet = isNet;
                data.text = text;
                data.titleStr = title;
                data.sureCallback = sureCallback;
                data.cancelCallback = cancelCallback;
                await Create(scene, UIType.UIPop, layer);
            }

            await ETTask.CompletedTask;
        }
        
        
        private static bool isLoadTipsing = false;
        public static async void ShowTip(Scene scene, string text)
        {
            if (isLoadTipsing)
            {
                return;
            }

            TipInfoComponent data = Game.Scene.GetComponent<TipInfoComponent>();
            if (data == null)
            {
                data = Game.Scene.AddComponent<TipInfoComponent>();
            }
            data.text = text;
            UI ui = UIHelper.Get(scene, UIType.UITip);
            if (ui != null)
            {
                Log.Console("上一个提示界面还没有消失");
                await ETTask.CompletedTask;
                return;
            }

            isLoadTipsing = true;
            await Create(scene, UIType.UITip, UILayer.Mid);
            isLoadTipsing = false;
            await ETTask.CompletedTask;
        }
        
        public static async void ShowTip(Scene scene, int errorId)
        {
            if (errorId == 110313)
            {
                HallHelper.gateSession.Dispose();
                EventDispatcher.PostEvent(EventName.ConnectMySerInGame, null);
                return;
            }

            ErrorCodeConfig config = ErrorCodeConfigCategory.Instance.Get(errorId);
            if(!string.IsNullOrEmpty(config.English))
                ShowTip(scene, config.English);
            await ETTask.CompletedTask;
        }
        
        public static GameObject LoadModel(string modelName, GameObject parent)
        {
            if (modelName == "")
            {
                return null;
            }
            ResourcesComponent.Instance.LoadBundle(modelName + ".unity3d");
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset(modelName + ".unity3d", modelName);
            
            GameObject model = UnityEngine.Object.Instantiate(bundleGameObject, parent.transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
            return model;
        }
        
        public static GameObject LoadModelByCityModel(string modelName, GameObject parent,Action<GameObject> callBack = null)
        {
            if (modelName == "")
            {
                return null;
            }
            string abName = modelName.Substring(0,3) + ".unity3d";
            abName = abName.ToLower();
            Log.Info("abName:::" + abName + "modelName:::" + modelName);
            
            EventDispatcher.PostEvent(EventName.NetWaitUI, null, true);
            DynamicDownLoadMgr.GetInstance().LoadModel<GameObject>(abName,modelName,(GameObject model)=>{
                if (model != null)
                {
                    model.transform.SetParent(parent.transform);
                    model.transform.localPosition = Vector3.zero;
                    model.transform.localRotation = Quaternion.identity;
                    model.transform.localScale = Vector3.one;
                    callBack?.Invoke(model);
                }
                else
                {
                    callBack?.Invoke(null);
                }
                EventDispatcher.PostEvent(EventName.NetWaitUI, null, false);
            }).Coroutine();
            return null;
        }
    }
}