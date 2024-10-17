//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace ET.DownLoad
{
    /// <summary>
    /// AB下载器
    /// </summary>
    public class AssetBundleDownloadRoutine: MonoBehaviour
    {
        /// <summary>
        /// 此下载器需要下载的列表
        /// </summary>
        private List<FileVersionInfo> m_List = new List<FileVersionInfo>();

        /// <summary>
        /// 当前正在下载的任务
        /// </summary>
        private FileVersionInfo m_currDownLoadFile;

        /// <summary>
        /// 需要下载的数量
        /// </summary>
        private int NeedDownloadCount
        {
            set;
            get;
        }
        /// <summary>
        /// 已经下载完成的数量
        /// </summary>
        public int CompleteCount
        {
            set;
            get;
        }

        private long m_DownLoadSize;     //已经下载好的文件的总大小
        private int m_CurDownLoadSize;  //当前在的文件的大小；

        /// <summary>
        /// 此下载器已经下载的文件大小
        /// </summary>
        public long DownLoadSize
        {
            get
            {
                return this.m_DownLoadSize + this.m_CurDownLoadSize;
            }
        }
        /// <summary>
        /// 是否开始下载
        /// </summary>
        private bool IsStartDownLoad
        {
            set;
            get;
        }

        /// <summary>
        /// 添加下载任务
        /// </summary>
        public void AddDownload(FileVersionInfo fileInfo)
        {
            this.m_List.Add(fileInfo);
        }
        /// <summary>
        /// 下载器开始下载任务
        /// </summary>
        public void StartDownload()
        {
            this.IsStartDownLoad = true;
            this.NeedDownloadCount = this.m_List.Count;
            
        }

        private void Update()
        {
            if (this.IsStartDownLoad)
            {
                this.IsStartDownLoad = false;
                StartCoroutine(this.DownLoadData());
            }
        }
        private int count = 0;
        private IEnumerator DownLoadData()
        {
            if(this.NeedDownloadCount == 0 || this.m_List.Count < 1)
                yield break;
            this.m_currDownLoadFile = this.m_List[0];
            string fileUrl = DownLoadMgr.DownLoadUrl() + this.m_currDownLoadFile.File;    //资源下载路径
            //得到本地路径
            string localFilePath = Path.Combine(PathHelper.AppHotfixResPath, this.m_currDownLoadFile.File);
            UnityWebRequest request = new UnityWebRequest();
            request = UnityWebRequest.Get(fileUrl);
            request.SendWebRequest();
            float progress = request.downloadProgress;
            float timeOut = Time.time;
            while (request != null && !request.isDone)
            {
                if (progress < request.downloadProgress)
                {
                    timeOut = Time.time;
                    progress = request.downloadProgress;
                    this.m_CurDownLoadSize = (int)(this.m_currDownLoadFile.Size * progress);
                }

                //下载超时
                if ((Time.time - timeOut) > DownLoadMgr.DownLoadTimeOut)
                {
                    if (count >= 5)
                    {
                        Log.Error(this.m_currDownLoadFile.File + "下载失败跳过文件下载..");
                        this.m_List.RemoveAt(0);
                        this.CompleteCount++;
                        if (this.m_List.Count == 0)
                        {
                            this.m_List.Clear();
                        }
                        else
                        {
                            this.IsStartDownLoad = true;
                        }
                        yield break;
                    }
                    else
                    {
                        Log.Error("下载超时：：" + this.m_currDownLoadFile.File + "重新下载：" + count);
                        StartCoroutine(DownLoadData());
                        count++;
                        yield break;
                    }
                }
                //等一帧
                yield return null;
            }

            yield return request;
            //下载成功
            if (request != null && request.error == null)
            {
                string path = Path.Combine(PathHelper.AppHotfixResPath, this.m_currDownLoadFile.File);
                if (!File.Exists(path))
                {
                    CreateDirectory(path);
                }
                byte[] data = request.downloadHandler.data;
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    fs.Write(data, 0, data.Length);
                }
                Log.Info(this.m_currDownLoadFile.File + "::下载完成");
            }

            this.m_CurDownLoadSize = 0;
            this.m_DownLoadSize += this.m_currDownLoadFile.Size;
            //写入本地文件
            this.m_List.RemoveAt(0);
            this.CompleteCount++;
            if (this.m_List.Count == 0)
            {
                this.m_List.Clear();
            }
            else
            {
                this.IsStartDownLoad = true;
            }
            count = 0;
        }
        private void CreateDirectory(string path)
        {
            if (Directory.Exists(path) == true)
                return;
            int iLastIndex = path.LastIndexOf('/');
            if (iLastIndex > 0)
            {
                string parentPath = path.Substring(0, iLastIndex);
                if (Directory.Exists(parentPath) == false)
                {
                    Directory.CreateDirectory(parentPath);
                }
            }
        }
    }
}