using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ET.DownLoad
{
    /// <summary>
    /// 下载管理器
    /// </summary>
    public class DownLoadMgr : Singleton<DownLoadMgr>
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        public const int DownLoadTimeOut = 20;
        /// <summary>
        /// 测试服 cdn
        /// </summary>
        private const string DownLoadBaseUrl_Test = "https://seameta-food.oss-cn-hangzhou.aliyuncs.com/Test/";
        /// <summary>
		/// 正式服CDN
		/// </summary>
		private const string DownLoadBaseUrl_Formal = "https://singapore-seameta-prod.oss-ap-southeast-1.aliyuncs.com/";
		/// <summary>
		/// cdn上的version文件内容
		/// </summary>
		private VersionConfig remoteVersionConfig;
		/// <summary>
		/// 本地version文件内容
		/// </summary>
		private VersionConfig streamingVersionConfig;
		public static string DownLoadUrl()
		{
			if (string.IsNullOrEmpty(GameDataMgr.Instance.IPAdress))
				return "";
			string baseUrl = "";
			if (GameDataMgr.Instance.IPAdress == "114.55.11.158:10005" || GameDataMgr.Instance.IPAdress == "8.219.196.181:10005")
			{
				baseUrl = DownLoadBaseUrl_Test;
			}
			else
			{
				baseUrl = DownLoadBaseUrl_Formal;
			}
			string realUrl = "";
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        realUrl = baseUrl + "Android/";
#elif UNITY_ANDROID
        realUrl = baseUrl + "Android/";
#elif UNITY_IPHONE
		realUrl = baseUrl + "IOS/";
#endif
            return realUrl;
		}

        /// <summary>
        /// 下载器数量
        /// </summary>
        public const int DownLoadRoutineNum = 5;
        /// <summary>
        /// 本地资源路径
        /// </summary>
        public string LocalFilePath = Application.persistentDataPath + "/";

        /// <summary>
        /// 需要下载的数据列表
        /// </summary>
        private List<FileVersionInfo> m_NeedDownloadDataList = new List<FileVersionInfo>();

        private long TotalSize;
        /// <summary>
        /// 检测版本更新
        /// </summary>
        public void InitCheckVersion()
        { 
	        string strVersionPath = DownLoadUrl() + "Version.txt";        //版本文件
			Log.Info("cdn地址：：" + strVersionPath);
			
			//TimeHelper.ExecutionTime("version版本对比：：" , 1);
	        AssetBundleDownload.Instance.InitServerVersion(strVersionPath,OnInitVersionCallBack);
	        //TimeHelper.ExecutionTime("version版本对比：：" , 2);
        }
        /// <summary>
        /// 封装server版本文件成功
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public VersionConfig PackDownLoadData(string content)
        {
	        VersionConfig versionconfig = JsonHelper.FromJson<VersionConfig>(content);
	        return versionconfig;
        }
            
        /// <summary>
        /// version版本控制文件下载成功
        /// </summary>
        /// <param name="list"></param>
        private void OnInitVersionCallBack(VersionConfig remoteVersionConfig,VersionConfig streamingVersionConfig)
        {
	        this.remoteVersionConfig = remoteVersionConfig;
	        this.streamingVersionConfig = streamingVersionConfig;
	        //TimeHelper.ExecutionTime("version对比：：" , 1);
			m_NeedDownloadDataList.Clear();
			if (remoteVersionConfig == null || streamingVersionConfig == null)
			{
                AssetBundleDownload.Instance.StartDownLoadFile(this.m_NeedDownloadDataList, this.TotalSize);
				return;
            }
	        // 获取远程的Version.txt
	        //string versionUrl = "";
	        // 获取streaming目录的Version.txt
	        string versionPath = Path.Combine(PathHelper.AppResPath4Web, "Version.txt");
			Log.Info("versionPath::" + versionPath);
	        Log.Info(versionPath);
	        // 删除本地多余文件
	        DirectoryInfo directoryInfo = new DirectoryInfo(PathHelper.AppHotfixResPath);
	        if (directoryInfo.Exists)
	        {
		        FileInfo[] fileInfos = directoryInfo.GetFiles();
		        foreach (FileInfo fileInfo in fileInfos)
		        {
			        if (remoteVersionConfig.FileInfoDict.ContainsKey(fileInfo.Name))
			        {
				        continue;
			        }
                
			        if (fileInfo.Name == "Version.txt")
			        {
				        continue;
			        }
			        fileInfo.Delete();
		        }
	        }
	        else
	        {
		        directoryInfo.Create();
	        }
	        // 对比MD5  先对比本地文件MD5 本地没有此文件就对比本地version
	        foreach (FileVersionInfo fileVersionInfo in remoteVersionConfig.FileInfoDict.Values)
	        {
				if (fileVersionInfo.File.IndexOf("scene_city.unity3d") != -1 ||
					fileVersionInfo.File.IndexOf("pvp04_cs1.unity3d") != -1 ||
					fileVersionInfo.File.IndexOf("scene_pvp01.unity3d") != -1 ||
					fileVersionInfo.File.IndexOf("scene_pvp02.unity3d") != -1)
				{
					Log.Info("scene_city.unity3d 不更新");
					continue;
				}

				bool flag = false;
				foreach (var value in NoUpDate.NoUpDateDic)
				{
					if (fileVersionInfo.File.IndexOf(value.Key) !=-1)
					{
						Log.Info(fileVersionInfo.File + "  no update!");
						flag = true;
						break;
					}
				}
				if(flag)
					continue;
				// 对比md5
		        if (streamingVersionConfig != null)
		        {
			        string localFileMD5 = GetBundleMD5(streamingVersionConfig, fileVersionInfo.File);
			        if (fileVersionInfo.MD5.Equals(localFileMD5) || fileVersionInfo.File.Equals("Version.txt"))
			        {
				        continue;
			        }
			        else
			        {
				        Log.Info("热更需要下载的文件::" + fileVersionInfo.File);
			        }
                    this.m_NeedDownloadDataList.Add(fileVersionInfo);       //加入下载列表
                    this.TotalSize += fileVersionInfo.Size;
                }    
	        }
	        Log.Info("热更需要更新的文件个数::" + this.m_NeedDownloadDataList.Count);
	        Log.Info("对比完成，开始下载文件");
            AssetBundleDownload.Instance.StartDownLoadFile(this.m_NeedDownloadDataList,this.TotalSize);
        }
		/// <summary>
		/// 下载完成把CDN上的version文件保存在本地
		/// </summary>
        public void SaveVersionToLocal()
		{
			string localVersionPath = Path.Combine(PathHelper.AppHotfixResPath, "Version.txt");;
	        if (this.remoteVersionConfig != null && this.streamingVersionConfig != null
				&& this.remoteVersionConfig.Version != this.streamingVersionConfig.Version)
	        {
		        Log.Info("SaveVersionToLocal");
		        string tempJson = LitJson.JsonMapper.ToJson(this.remoteVersionConfig);
		        File.WriteAllText(localVersionPath, tempJson, Encoding.ASCII);
	        }
        }

        public  string GetBundleMD5(VersionConfig streamingVersionConfig, string bundleName)
        {
	        string path = Path.Combine(PathHelper.AppHotfixResPath, bundleName);
	        if (File.Exists(path))
	        {
				string md5 = MD5Helper.FileMD5(path);
                return md5;
	        }

			if (streamingVersionConfig.FileInfoDict.ContainsKey(bundleName))
			{
				return streamingVersionConfig.FileInfoDict[bundleName].MD5;
			}

			return "";
        }
    }
}