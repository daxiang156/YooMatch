using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using ET.DownLoad;

namespace ET
{
    public class DynamicDownLoadRoutine
    {
        private UnityWebRequest webRequest = new UnityWebRequest();
        public static DynamicDownLoadRoutine Create()
        {
            DynamicDownLoadRoutine routine = new DynamicDownLoadRoutine();
            return routine;
        }
        public async ETTask Load<T>(string path,Action<T> callBack) where T : UnityEngine.Object
        {
            await StartLoad(path, callBack);
        }
        public async ETTask StartLoad<T>(string path, Action<T> callBack = null) where T : UnityEngine.Object
        {
            await TimerComponent.Instance.WaitAsync(1);
            string localPath = Path.Combine(PathHelper.AppHotfixResPath, path);
            if (File.Exists(localPath))
            {
                localPath = "file://" + localPath;
                webRequest = UnityWebRequest.Get(localPath);
                this.webRequest.SendWebRequest().completed += (AsyncOperation state) =>
                {
                    string type = typeof(T).Name;
                    if (this.webRequest == null || this.webRequest.error != null)
                    {
                        if(callBack != null)
                            callBack(null);
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
                    }
                    this.LoadFinish();
                };
            }
            else
            {
                Log.Info("down load from cdn");
                string tempPath = "DynamicDownLoad/" + path;
                string cdnPath = Path.Combine( DownLoadMgr.DownLoadUrl(),tempPath);    //资源下载路径
                if (GameDataMgr.Instance.Platflam() == PlatForm.IOS)
                {
                    cdnPath = cdnPath.Replace("/IOS", "");
                }
                else
                {
                    cdnPath = cdnPath.Replace("/Android", "");   
                }
                
                this.webRequest = UnityWebRequest.Get(cdnPath);
                this.webRequest.SendWebRequest().completed += (AsyncOperation state)=>{
                   
                    if (this.webRequest.isDone)
                    {
                        if (this.webRequest != null && this.webRequest.error == null)
                        {
                            string hotfixResPath = Path.Combine(PathHelper.AppHotfixResPath, path);
                            if (!File.Exists(hotfixResPath))
                            {
                                CreateDirectory(hotfixResPath);
                            }

                            byte[] data = this.webRequest.downloadHandler.data;
                            using (FileStream fs = new FileStream(hotfixResPath,FileMode.Create))
                            {
                                fs.Write(data,0,data.Length);
                            }
                        }
                    }       
                    
                    string type = typeof(T).Name;
                    if (this.webRequest == null || this.webRequest.error != null)
                    {
                        if(callBack != null)
                            callBack(null);
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
                    }
                    this.LoadFinish();
                };
            }
        }

        public async ETTask StartDownLoadModel(string path, Action<bool> callBack)
        {
            string localPath = Path.Combine(PathHelper.AppHotfixResPath, path);
            string resPath = Path.Combine(PathHelper.AppResPath4Web,path);
            if (File.Exists(localPath))
            {
                Log.Info("模型存在::" + localPath);
                callBack?.Invoke(true);
                return;
            }
            bool isRequestFinish = false;
            this.webRequest = UnityWebRequest.Get(resPath);
            this.webRequest.SendWebRequest().completed += (AsyncOperation state1) =>
            {
                if (this.webRequest != null && string.IsNullOrEmpty(this.webRequest.error))
                {
                    Log.Info("模型存在::" + resPath);
                    this.webRequest?.Dispose();
                    this.webRequest = null;
                    isRequestFinish = true;
                    callBack?.Invoke(true);
                }
                else
                {
                    Log.Info("down load from cdn");
                    string cdnPath = Path.Combine( DownLoadMgr.DownLoadUrl(),path);    //资源下载路径
                    this.webRequest = UnityWebRequest.Get(cdnPath);
                    this.webRequest.SendWebRequest().completed += (AsyncOperation state)=>{
                        if (this.webRequest == null)
                        {
                            return;
                        }
                        if (this.webRequest.isDone)
                        {
                            if (this.webRequest != null && this.webRequest.error == null)
                            {
                                string hotfixResPath = Path.Combine(PathHelper.AppHotfixResPath, path);
                                if (!File.Exists(hotfixResPath))
                                {
                                    CreateDirectory(hotfixResPath);
                                }

                                byte[] data = this.webRequest.downloadHandler.data;
                                using (FileStream fs = new FileStream(hotfixResPath,FileMode.Create))
                                {
                                    fs.Write(data,0,data.Length);
                                }
                                Log.Info("模型下载完成：：" + path);
                                callBack?.Invoke(true);
                                isRequestFinish = true;
                            }
                            else
                            {
                                //下载失败
                                callBack?.Invoke(false);
                                isRequestFinish = true;
                            }
                        }
                    };
                }
            };
            await TimerComponent.Instance.WaitAsync(5000);
            if (!isRequestFinish)
            {
                Log.Info("model = " + path + " downLoad from cdn out time == 5s");
                callBack?.Invoke(false);
                this.webRequest?.Dispose();
                this.webRequest = null;
            }
            await ETTask.CompletedTask;
        }

        private void LoadFinish()
        {
            webRequest?.Dispose();
            webRequest = null;
        }

        private void CreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            int ILastIndex = path.LastIndexOf('/');
            if (ILastIndex > 0)
            {
                string parentPath = path.Substring(0, ILastIndex);
                Log.Info("createDirectory::" + parentPath);
                if (Directory.Exists(parentPath) == false)
                {
                    Directory.CreateDirectory(parentPath);
                }
            }
        }
    }
}