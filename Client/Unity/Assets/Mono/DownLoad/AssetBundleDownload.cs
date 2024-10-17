using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ET.DownLoad;
using UnityEngine;
using UnityEngine.Networking;

namespace ET
{
    /// <summary>
    /// 下载器
    /// </summary>
    public class AssetBundleDownload: MonoSingleton<AssetBundleDownload>
    {
        private string m_VersionUrl;
        private Action<VersionConfig,VersionConfig> m_OnInitVersion;
        /// <summary>
        /// 下载器数组
        /// </summary>
        private AssetBundleDownloadRoutine[] m_Routine = new AssetBundleDownloadRoutine[DownLoadMgr.DownLoadRoutineNum];
        /// <summary>
        /// 下载器索引
        /// </summary>
        private int m_RoutineIndex = 0;

        /// <summary>
        /// 需要下载的总大小
        /// </summary>
        public long m_TotalSize;
        /// <summary>
        /// 需要下载的总数量
        /// </summary>
        public int m_TotalCount;
        /// <summary>
        /// 是否下载完成
        /// </summary>
        public bool m_IsDownLoadOver = false;
        /// <summary>
        /// 当前已经下载的文件总大小
        /// </summary>
        /// <returns></returns>
        public long CurrCompleteTotalSize()
        {
            long completeTotalSize = 0;
            for (int i = 0; i < this.m_Routine.Length; i++)
            {
                if(this.m_Routine[i] == null)
                    continue;
                completeTotalSize += this.m_Routine[i].DownLoadSize;
            }

            return completeTotalSize;
        }
        
        /// <summary>
        /// 当前已经下载的文件总数量
        /// </summary>
        /// <returns></returns>
        public int CurrCompleteTotalCount()
        {
            int completeTotalCount = 0;
            for (int i = 0; i < this.m_Routine.Length; i++)
            {
                if(this.m_Routine[i] == null)
                    continue;
                completeTotalCount += this.m_Routine[i].CompleteCount;
            }

            return completeTotalCount;
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            ////显示下载进度
            //if (this.m_TotalCount > 0 && !m_IsDownLoadOver)
            //{
            //    int totalCompleteCount = this.CurrCompleteTotalCount();
            //    totalCompleteCount = totalCompleteCount == 0 ? 1 : totalCompleteCount;

            //    long totalCompleteSize = this.CurrCompleteTotalSize();

            //    //string str = string.Format("正在下载{0}/{1}", totalCompleteCount, this.m_TotalCount);
            //    if (totalCompleteCount == this.m_TotalCount)
            //    {
            //        Log.Info("所有资源下载完成");
            //        this.m_IsDownLoadOver = true;
            //    }

            //    double value = Math.Round(totalCompleteSize / (float)this.m_TotalSize, 2) * 100;
            //    string strProgress = string.Format("{0}%", value);
            //    Log.Info(strProgress);
            //    if (UIDownLoad.instance != null)
            //    {
            //        UIDownLoad.instance.txt_value.text = strProgress;
            //        UIDownLoad.instance.slider.value = totalCompleteSize / (float)this.m_TotalSize;
            //    }
            //}
        }

        /// <summary>
        /// 初始化服务器版本文件
        /// </summary>
        public void InitServerVersion(string url,Action<VersionConfig,VersionConfig> onInitVersion)
        {
            this.m_VersionUrl = url;
            this.m_OnInitVersion = onInitVersion;
            StartCoroutine(DownLoadVersion(this.m_VersionUrl));
        }
        
        
        /// <summary>
        /// 开始下载文件  
        /// </summary>
        public void StartDownLoadFile(List<FileVersionInfo> downLoadlist,long totalSize)
        {
            //无需下载直接进入游戏
            if (downLoadlist.Count == 0)
            {
                UIDownLoad.instance.ProgressIndex = 4;
                m_IsDownLoadOver = true;
                return;
            }
            UIDownLoad.instance.ProgressIndex = 2;
            //初始化下载器
            this.m_TotalCount = downLoadlist.Count;
            this.m_TotalSize = totalSize;
            for (int i = 0; i < m_Routine.Length; i++)
            {
                if (m_Routine[i] == null)
                {
                    m_Routine[i] = this.gameObject.AddComponent<AssetBundleDownloadRoutine>();
                }
            }
            //循环给下载器分配任务
            for (int i = 0; i < downLoadlist.Count; i++)
            {
                m_RoutineIndex = i % this.m_Routine.Length;   // 0 -4
                //其中一个下载器
                this.m_Routine[m_RoutineIndex].AddDownload(downLoadlist[i]);
            }
            
            //让下载器开始下载
            for (int i = 0; i < this.m_Routine.Length; i++)
            {
                if(this.m_Routine[i] == null)
                    continue;
                this.m_Routine[i].StartDownload();
            }
        }
        /// <summary>
        /// 下载版本控制文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private IEnumerator DownLoadVersion(string url)
        {
            Log.Info("开始下载版本控制文件::" + url);
            //读取本地文件
            EventDispatcher.PostEvent(EventName.LoadingPress, null, 0.05f);
            string localVersionPath = "file://" + Path.Combine(PathHelper.AppHotfixResPath, "Version.txt");
            if (File.Exists(localVersionPath))
            {
                Log.Info("hot version path exit");
            }
            else
            {
                Log.Info("hot version path not exit");
                localVersionPath = Path.Combine(PathHelper.AppResPath4Web, "Version.txt");
            }
            UnityWebRequest localReuqest = new UnityWebRequest();
            localReuqest = UnityWebRequest.Get(localVersionPath);
            yield return localReuqest.SendWebRequest();
            string localContent = localReuqest.downloadHandler.text;
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SendWebRequest();
            float timeOut = Time.time;
            float progress = request.downloadProgress;
            while (request != null && !request.isDone)
            {
                if (progress < request.downloadProgress)
                {
                    timeOut = Time.time;
                    progress = request.downloadProgress;
                }

                if ((Time.time - timeOut) > DownLoadMgr.DownLoadTimeOut)
                {
                    Log.Error("下载超时：：" + url);
                    this.m_OnInitVersion(null, null);
                    yield break;
                }
            }
            yield return request;
            if (request != null && request.error == null)
            {
                string content = request.downloadHandler.text;
                EventDispatcher.PostEvent(EventName.LoadingPress, null, 0.08f);
                if (this.m_OnInitVersion != null)
                {
                    VersionConfig remoteVersion = DownLoadMgr.Instance.PackDownLoadData(content);
                    VersionConfig localVersion = DownLoadMgr.Instance.PackDownLoadData(localContent);
                    if (remoteVersion.Version == localVersion.Version)
                    {
                        Log.Info("CDN version版本号：：" + remoteVersion.Version + "__本地 version版本号：：" + localVersion.Version  + "__version版本号一致，无需更新");
                        this.m_OnInitVersion(null, null);
                        yield break;
                    }
                    //Log.Info("本地version：：" + localContent);
                    this.m_OnInitVersion(remoteVersion, localVersion);
                }
            }
            else
            {
                Log.Error("下载失败：：" + request.error);
                this.m_OnInitVersion(null, null);
            }
            EventDispatcher.PostEvent(EventName.LoadingPress, null, 0.1f);
        }

        public class AcceptAllCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }
    }
}