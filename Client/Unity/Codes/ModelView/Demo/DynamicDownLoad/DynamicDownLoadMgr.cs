using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using UnityEngine;
using UnityEngine.Networking;

namespace ET
{
    public class DynamicDownLoadMgr
    {
        private static DynamicDownLoadMgr _instance;
        public Init InitMono;
        public static DynamicDownLoadMgr GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DynamicDownLoadMgr();
            }
            return _instance;
        }
        public DynamicDownLoadMgr()
        {
            GameObject go = GameObject.Find("Global");
            InitMono = go.GetComponent<Init>();
        }
        /// <summary>
        /// 从AB里加载图片,只会从本地AB里查找
        /// </summary>
        public async ETTask LoadSpriteFromAB(string ABName,string picName,Action<Sprite> callBack = null)
        {
            ABName += ".unity3d";
            ResourcesLoaderComponent resouceComp = GlobalComponent.Instance.scene.GetComponent<ResourcesLoaderComponent>();
            await resouceComp.LoadAsync(ABName);
            Dictionary<string, UnityEngine.Object> dic = ResourcesComponent.Instance.GetBundleAll(ABName);
            if (dic.ContainsKey(picName))
            {
                if (dic[picName] is Sprite)
                {
                    callBack?.Invoke(dic[picName] as Sprite);
                }else if (dic[picName] is Texture)
                {
                    Texture2D texture2d = dic[picName] as Texture2D;
                    Sprite sp = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
                    callBack?.Invoke(sp);
                }
            }
        }
        
        /// <summary>
        /// 加载单张图片,本地没有从CDN上加载
        /// </summary>
        public async ETTask Load<T>(string path,Action<T> callBack = null) where T : UnityEngine.Object
        {
            Log.Info("start load" + path);
            DynamicDownLoadRoutine routine = new DynamicDownLoadRoutine();
            await routine.Load(path,callBack);
        }

        public async ETTask LoadFromLocal<T>(string path, Action<T> callBack = null, Action<string> txtCallBack = null) where T : UnityEngine.Object
        {
            await TimerComponent.Instance.WaitAsync(1);
            string hotfixPath = Path.Combine(PathHelper.AppHotfixResPath, path);
            if (File.Exists(hotfixPath))
            {
                string localPath = "file://" + Path.Combine(PathHelper.AppHotfixResPath, path);
                UnityWebRequest webRequest = UnityWebRequest.Get(localPath);
                webRequest.SendWebRequest().completed += (AsyncOperation state) =>
                {
                    string type = typeof(T).Name;
                    if (webRequest == null)
                    {
                        Log.Info("webrequest == null");
                    }
                    else
                    {
                        Log.Info("webrequest name::" + localPath + "__state::" + webRequest.error);
                    }

                    if (webRequest == null || webRequest.error != null)
                    {
                        if(callBack != null)
                            callBack(null);
                        if(txtCallBack != null)
                            txtCallBack(null);
                    }
                    else
                    {
                        if (type == "Sprite")
                        {
                            if (callBack != null)
                            {
                                Texture2D texture2D = new Texture2D(10, 10);
                                texture2D.LoadImage(webRequest.downloadHandler.data);
                                Sprite sp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
                                callBack((T)(sp as UnityEngine.Object));
                            }
                        }

                        if (type == "TextAsset")
                        {
                            txtCallBack(webRequest.downloadHandler.text);
                        }
                        webRequest?.Dispose();
                        webRequest = null;
                    }
                };
            }
            else
            {
                callBack(null);
            }
        }

        private List<string> aniList = new List<string>(){};
        public async ETTask LoadModel<T>(string ABName = "a01.unity3d",string modelName = "A01L01_player",Action<T> callBack = null) where T : UnityEngine.Object
        {
            ABName = ABName.ToLower();
            //先判断动画AB文件
            int downLoadNum = 0;
            for (int i = 0; i < this.aniList.Count; i++)
            {
                string localPath = Path.Combine(PathHelper.AppHotfixResPath, this.aniList[i]);
                if (!File.Exists(localPath))
                {
                    Log.Info("开始下载动画文件" + "ABName::" + this.aniList[i]);
                    DynamicDownLoadRoutine routine = new DynamicDownLoadRoutine();
                    await routine.StartDownLoadModel(this.aniList[i], async (bool state) =>
                    {
                        downLoadNum++;
                        if (downLoadNum == this.aniList.Count)
                        {
                            await StartLoadModel(ABName,modelName,callBack);
                            downLoadNum = 0;
                        }
                    });
                }
                else
                {
                    downLoadNum++;
                }
            }

            if (downLoadNum == this.aniList.Count)
            {
                await StartLoadModel(ABName,modelName,callBack);
            }
        }

        private async ETTask StartLoadModel<T>(string ABName = "a01.unity3d",string modelName = "A01L01_player",Action<T> callBack = null) where T : UnityEngine.Object
        {
            if (!File.Exists(ABName))
            {
                Log.Info("开始下载" + "ABName::" + ABName);
                DynamicDownLoadRoutine routine = new DynamicDownLoadRoutine();
                await routine.StartDownLoadModel(ABName, async (bool state) =>
                {
                    if (state)
                    {
                        Log.Info("从bundle里加载模型::" + ABName + " modelName::" + modelName);
                        ResourcesLoaderComponent resouceComp = GlobalComponent.Instance.scene.GetComponent<ResourcesLoaderComponent>();
                        await resouceComp.LoadAsync(ABName);
                        UnityEngine.Object model = (GameObject)ResourcesComponent.Instance.GetAsset(ABName, modelName);
                        Log.Info("从bundle里加载模型1111::" + ABName + " modelName::" + modelName);
                        callBack?.Invoke((T)GameObject.Instantiate(model));
                    }
                    else
                    {
                        callBack?.Invoke(null);
                    }
                });
            }
        }

        public bool IsFileExists(string path)
        {
            string localPath = Path.Combine(PathHelper.AppHotfixResPath, path);
            if (File.Exists(localPath))
            {
                return true;
            }
            return false;
        }
        
        public void PreDownLoadBg()
        {
            // string path = "MBBg/mb_back_" + (MBDataComponent.Instance.curPlayLevel + 1) + ".jpg";
            // if (!DynamicDownLoadMgr.GetInstance().IsFileExists(path))
            // {
            //     DynamicDownLoadMgr.GetInstance().Load<Sprite>(path).Coroutine();
            // }
            //
            // path = "MBAnimal/mb_back_" + (MBDataComponent.Instance.curPlayLevel + 1) + "_C.png";
            // if (!DynamicDownLoadMgr.GetInstance().IsFileExists(path))
            // {
            //     DynamicDownLoadMgr.GetInstance().Load<Sprite>(path).Coroutine();
            // }
        }
    }
}