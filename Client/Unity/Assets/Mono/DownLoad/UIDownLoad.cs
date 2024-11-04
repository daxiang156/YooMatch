using ET.DownLoad;
using ILRuntime.Runtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIDownLoad : MonoBehaviour
    {
        /// <summary>
        /// 当前进度
        /// </summary>
        public Slider slider;
        /// <summary>
        /// 进度值
        /// </summary>
        public Text txt_value;

        public static UIDownLoad instance;
        private bool IsLoadCode = false;
        private Action callBack;
        //private bool ConfigABState = false;
        private void Start()
        {
            EventDispatcher.AddObserver(this, EventName.LoadingCloseListen, (object[] info) =>
            {
                EndLoadMainCity();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, EventName.LoadingPress, (object[] info) =>
            {
                float per = (float) info[0];
                this.slider.value = per;
                string strProgress = string.Format("{0}%", Math.Round(this.slider.value,2) * 100);
                this.txt_value.text = strProgress;
                return false;
            }, null);
            EventDispatcher.AddObserver(this,EventName.ConfigABLoadFinish , (object[] info) =>
            {
                //ConfigABState = true;
                if(this.slider.value == 1)
                    this.EndLoadMainCity();
                return false;
            }, null);
        }
        private void EndLoadMainCity()
        {
            this.slider.value = 1;
            Dispose();
            Log.Info("进入游戏");
            EventDispatcher.PostEvent(EventName.DownLoadFinish,null);
        }

        public void Dispose()
        {
            //Log.Info("关闭下载进度1");
            EventDispatcher.RemoveObserver(EventName.LoadingCloseListen);
            EventDispatcher.RemoveObserver(EventName.LoadingPress);
            EventDispatcher.RemoveObserver(EventName.ConfigABLoadFinish);
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
        }
        public void DownLoadFinishCallBack(Action callBack)
        {
            this.callBack = callBack;
        }
        private int progressIndex = 0;
        public int ProgressIndex
        {
            set { 
                progressIndex = value;
                if (progressIndex == 4)
                {
                    AssetBundleDownload.Instance.m_IsDownLoadOver = true;
                    DownLoadMgr.Instance.SaveVersionToLocal();
                }
            }
        }
        /// <summary>
        /// 0 - 10   热更前版本文件检测
        /// </summary>
        private void DoProgress1()
        {
            if (this.slider.value < 0.1f)
            {
                this.slider.value += 0.001f;
                string strProgress = string.Format("{0}%", Math.Round(this.slider.value,2) * 100);
                this.txt_value.text = strProgress;
            }
        }
        /// <summary>
        /// 第二部分，热更前版本文件检测 --》热更
        /// </summary>
        private void Doprogress2()
        {
            //显示下载进度
            if (AssetBundleDownload.Instance.m_TotalCount > 0 && !AssetBundleDownload.Instance.m_IsDownLoadOver)
            {
                int totalCompleteCount = AssetBundleDownload.Instance.CurrCompleteTotalCount();
                totalCompleteCount = totalCompleteCount == 0 ? 1 : totalCompleteCount;

                long totalCompleteSize = AssetBundleDownload.Instance.CurrCompleteTotalSize();

                //string str = string.Format("正在下载{0}/{1}", totalCompleteCount, this.m_TotalCount);
                if (totalCompleteCount == AssetBundleDownload.Instance.m_TotalCount)
                {
                    Log.Info("所有资源下载完成");
                    AssetBundleDownload.Instance.m_IsDownLoadOver = true;
                    DownLoadMgr.Instance.SaveVersionToLocal();
                }

                double tempValue = Math.Round(totalCompleteSize / (float)AssetBundleDownload.Instance.m_TotalSize, 2) * 100;
                double value = tempValue * 0.8;
                string strProgress = string.Format("{0}%",10 + value.ToInt32());
                Log.Info(strProgress);
                this.txt_value.text = strProgress;
                float value1 = 10f + value.ToFloat();
                Log.Info("value:" + value1/100);
                this.slider.value = value1/100;
            }
        }
        /// <summary>
        /// 第三部分进度，热更完--》loading加载结束
        /// </summary>
        private void DoProgress3()
        {
            if (this.slider.value < 1f)
            {
                this.slider.value += 0.001f;
                string strProgress = string.Format("{0}%", Math.Round(this.slider.value, 2) * 100);
                this.txt_value.text = strProgress;
            }
        }
        /// <summary>
        /// 无需更新
        /// </summary>
        public void DoProgress4()
        {
            if (this.slider.value < 1f)
            {
                this.slider.value += 0.005f;
                string strProgress = string.Format("{0}%", Math.Round(this.slider.value, 2) * 100);
                this.txt_value.text = strProgress;
            }
        }

        private void Update()
        {
            if (progressIndex == 4)
            {
                if (AssetBundleDownload.Instance.m_IsDownLoadOver && this.IsLoadCode == false)
                {
        //            CodeLoader.Instance.LoadCode();
                    this.IsLoadCode = true;
                }
                DoProgress4();
            }
            else
            {
                if (AssetBundleDownload.Instance.m_IsDownLoadOver && this.IsLoadCode == false)
                {
          //          CodeLoader.Instance.LoadCode();
                    this.IsLoadCode = true;
                    progressIndex = 3;
                }
                else
                {
                    if (progressIndex == 1)
                    {
                        DoProgress1();
                    }
                    else if (progressIndex == 2)
                    {
                        Doprogress2();
                    }
                    else if (progressIndex == 3)
                    {
                        DoProgress3();
                    }
                }
            }
        }
    }
}