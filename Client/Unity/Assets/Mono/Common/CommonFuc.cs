using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

namespace ET
{
    public static class CommonFuc
    {
        /// <summary>         
        /// 秒转化为时钟格式  
        /// </summary>        
        /// <param name="timeTotal"></param>
        /// <returns></returns>
        public static string TimeInt2Txt(int timeTotal)
        {
            if (timeTotal < 60)
            {
                return "00:" + AddZero(timeTotal);
            }
            else if(timeTotal < 3600)
            {
                return AddZero(timeTotal / 60) + ":" + AddZero(timeTotal % 60);
            }
            else
            {
                return AddZero(timeTotal / 3600) + ":" + AddZero(timeTotal % 3600 / 60) + ":" + AddZero(timeTotal % 60);
            }
        }
    
        public static string AddZero(int num)
        {
            if (num < 10)
            {
                return "0" + num;
            }
            else
            {
                return num.ToString();
            }
        }
    
        /// <summary>
        /// 加载特效  
        /// </summary>           
        /// <param name="path"></param>
        /// <param name="Pose"></param>
        /// <param name="scale ，放大位数"></param>
        /// <param name="delayDestroy，销毁时间，0表示不销毁"></param>
        /// <returns></returns>
        public static GameObject LoadResource(string path, Vector3 Pose, float scale,float delayDestroy = 0)
        {            
            GameObject effect = Resources.Load(path) as GameObject;                
            GameObject effect2 = UnityEngine.Object.Instantiate(effect, Pose, Quaternion.identity);
            effect2.transform.localScale = Vector3.one * scale;
            if (delayDestroy != 0)
            {
                effect2.AddComponent<DestroyDelay>().timeDelay = delayDestroy;
            }
            return effect2;
        }
    
        /// <summary>
        /// 当前场景
        /// </summary>
        /// <returns></returns>
        public static bool IsMainCity()
        {
            Scene scene = SceneManager.GetActiveScene ();
            string sceneName;
            if (scene.name == "scene_City")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public static string CurScene()
        {
            Scene scene = SceneManager.GetActiveScene ();
            return scene.name;
        }

        public static string NumToGameStr(int num)
        {
            if (num < 1000)
                return num.ToString();
            else
            {
                string str = (num / 1000) + "K";
                return str;
            }
        }
        
        public static int SetItemId(int row, int no, int level)
        {
            row = row >= 0 ? row : 100 - row;
            no = no >= 0? no : 100 - no;
            //level = level * 1000000;
            int dictonaryId = level * 1000000 + row * 1000 + no;
//            Debug.Log(dictonaryId);
            return dictonaryId;
        }

        public static Vector3 GetItemID(int dictonaryId)
        {
            int level = dictonaryId / 1000000;
            int row = (dictonaryId / 1000  % 1000);
            if (row >= 100)
                row = -(row - 100);
            int no = (dictonaryId % 1000);
            if (no >= 100)
                no = -(no - 100);
            Vector3 pos = new Vector3(row, no, level);
            return pos;
        }

        public static List<int> GetRoundId(int dicId, int level = 0)
        {
            List<int> idList = new List<int>();
            Vector3 pos = GetItemID(dicId);
            idList.Add(SetItemId((int)pos.x, (int)pos.y, level));
            idList.Add(SetItemId((int)pos.x - 1, (int)pos.y, level));
            idList.Add(SetItemId((int)pos.x - 1, (int)pos.y + 1, level));
            idList.Add(SetItemId((int)pos.x - 1, (int)pos.y - 1, level));
            idList.Add(SetItemId((int)pos.x + 1, (int)pos.y, level));
            idList.Add(SetItemId((int)pos.x + 1, (int)pos.y + 1, level));
            idList.Add(SetItemId((int)pos.x + 1, (int)pos.y - 1, level));
            idList.Add(SetItemId((int)pos.x, (int)pos.y + 1, level));
            idList.Add(SetItemId((int)pos.x, (int)pos.y - 1, level));
            return idList;
        }
        
        public static List<int> GetIceId(int dicId, int level = 0)
        {
            List<int> idList = new List<int>();
            Vector3 pos = GetItemID(dicId);
            idList.Add(SetItemId((int)pos.x - 2, (int)pos.y, level));
            idList.Add(SetItemId((int)pos.x - 2, (int)pos.y + 1, level));
            idList.Add(SetItemId((int)pos.x - 2, (int)pos.y - 1, level));
            idList.Add(SetItemId((int)pos.x + 2, (int)pos.y, level));
            idList.Add(SetItemId((int)pos.x + 2, (int)pos.y + 1, level));
            idList.Add(SetItemId((int)pos.x + 2, (int)pos.y - 1, level));
            idList.Add(SetItemId((int)pos.x, (int)pos.y + 2, level));
            idList.Add(SetItemId((int)pos.x + 1, (int)pos.y + 2, level));
            idList.Add(SetItemId((int)pos.x - 1, (int)pos.y + 2, level));
            idList.Add(SetItemId((int)pos.x, (int)pos.y - 2, level));
            idList.Add(SetItemId((int)pos.x + 1, (int)pos.y - 2, level));
            idList.Add(SetItemId((int)pos.x - 1, (int)pos.y - 2, level));
            return idList;
        }
        
        public static string GetCountryByIp(string userIp = "")
        {
            // IpInfo ipInfo = new IpInfo();
            // try
            // {
            //     //userIp = "202.20.107.255";
            //     //userIp = "140.227.126.41";
            //     // string info = new WebClient().DownloadString("http://ipinfo.io/" + userIp);
            //     // ipInfo = LitJson.JsonMapper.ToObject<IpInfo>(info);
            //     // RegionInfo myRI1 = new RegionInfo(ipInfo.country);
            //     
            //     //string info0 = new WebClient().DownloadString(string.Format("https://ipapi.co/{0}/json/", "45.8.25.61"));
            //     string info = new WebClient().DownloadString(string.Format("http://ip-api.com/json"));
            //     ipInfo = LitJson.JsonMapper.ToObject<IpInfo>(info);
            //     GameDataMgr.Instance.country = ipInfo.country;
            //     GameDataMgr.Instance.region = ipInfo.region;
            //     GameDataMgr.Instance.city = ipInfo.city;
            //     for (int i = 0; i < PhotonRegion.photonIp.Length; i++)
            //     {
            //         string[] region = PhotonRegion.photonIp[i];
            //         for (int j = 0; j < region.Length; j++)
            //         {
            //             if (ipInfo.country == region[j])
            //             {
            //                 GameDataMgr.Instance.photonIp = PhotonIp.photonAllIp[i];
            //                 break;
            //             }
            //         }
            //     }
            //
            //     if (GameDataMgr.Instance.country == "United States")
            //     {
            //         string[] region = PhotonRegion.photonIp[1];
            //         for (int i = 0; i < region.Length; i++)
            //         {
            //             if (ipInfo.region == region[i])
            //             {
            //                 GameDataMgr.Instance.photonIp = PhotonIp.photonAllIp[1];
            //                 break;
            //             }
            //         }
            //     }
            //
            //     if (GameDataMgr.Instance.photonIp == "")
            //     {
            //         GameDataMgr.Instance.photonIp = PhotonIp.photonAllIp[9];
            //     }
            //
            //     Debug.Log(" 国家是:" + GameDataMgr.Instance.country + ". 州/省份:" + GameDataMgr.Instance.region);
            //     Debug.Log("GameDataMgr地区的值是" + GameDataMgr.Instance.photonIp);
            // }
            // catch (Exception)
            // {
            //     ipInfo.country = null;
            // }
            //
            // return ipInfo.country;
            return "";
        }

        public static void RemoveAllListen(Toggle tgl)
        {
            tgl.onValueChanged.RemoveAllListeners();
        }

        public static int GetDefaultValue(int itemId, string itemStrExcel)
        {
            string[] itemStrs = itemStrExcel.Split(',');
            int defaultValue = 0;
            for (int j = 0; j < itemStrs.Length; j++)
            {
                string[] itemStr = itemStrs[j].Split(':');
                if (int.Parse(itemStr[0]) == itemId)
                {
                    defaultValue = int.Parse(itemStr[1]);
                }
            }
            return defaultValue;
        }

        public static void ClearPlayerPres()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        
        public static void LoadClothes(Transform tran, string path = "")
        {
            if (!string.IsNullOrEmpty(path))
            {
                Material clothesMaterial = Resources.Load("Material/" + path) as Material;
                if (clothesMaterial != null)
                {
                    SkinnedMeshRenderer meshObj = null;
                    for (int i = 0; i < tran.childCount; i++)
                    {
                        if (tran.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null)
                        {
                            meshObj = tran.GetChild(i).GetComponent<SkinnedMeshRenderer>();
                            meshObj.material = clothesMaterial;
                            //break;
                        }
                    }
                    if (meshObj == null)
                    {
                        Debug.LogError("Find clothes skin error. path = " + path);
                    }
                }
                else
                {
                    Debug.Log("切换服装，材质球找不到：" + path);
                }
            }
        }

        public static void ScrollValueChanged(ScrollRect scrollRect, Action<Vector2> callback)
        {
            scrollRect.onValueChanged.AddListener((Vector2 pos) =>
            {
                callback(pos);
            });
        }

        public static void SetAsLast(Transform tran)
        {
            tran.SetAsLastSibling();
        }
    }
}