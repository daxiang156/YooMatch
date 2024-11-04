using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    public enum TaskState
    {
        geting = 0,
        getingReward = 1,
        finished = 2,
    }
    public class TaskType
    {
        public const int GetResource = 1;
        public const int Buy = 2;
        public const int Skin = 3;
        public const int TotalCoin = 4;
        public const int PassLv = 5;
        public const int DailyLogin = 6;
        public const int LoginTime = 7;
        public const int PassLvTarget = 8;
        public const int OnlineTime = 9;
        public const int AddFriend = 10;
        public const int UseItem = 11;
        public const int PlayMB = 12;
        public const int TimeTotal = 13;
        public const int FunyGameRemove = 14;
        public const int UpRank = 16;
    }
    
    public class QuestHelper
    {
        private static QuestHelper _instance;

        public static QuestHelper GetInstance()
        {
            if (_instance == null)
                _instance = new QuestHelper();
            return _instance;
        }

        public List<TaskStruct> taskList;
        public int HalfHourTaskId = 602;

        private long lastResetTime = 0;

        public TaskState GetHalfTaskState()
        {
            if ((int) (LastResetTime - TimeInfo.Instance.ClientNow() / 1000) > 0)
                return TaskState.finished;
            else
            {
                return TaskState.getingReward;
            }
        }

        public List<TaskStruct> GetTaskList()
        {
            return this.taskList;
            // if(this.taskList != null && this.taskList.Count > 0)
            //     return this.taskList;
            // string taskStr = PlayerPrefs.GetString(ItemSaveStr.TaskList, "");
            //
            // DateTime date = TimeHelper.DateTimeNow();
            // String timeStr = date.ToString("HH:mm:ss");
            // taskList = new List<TaskStruct>();
            // if (string.IsNullOrEmpty(taskStr))
            // {
            //     List<TaskConfig> taskConfigs = TaskConfigCategory.Instance.GetAll().Values.ToList();
            //     for (int i = 0; i < taskConfigs.Count; i++)
            //     {
            //         TaskStruct taskStruct = new TaskStruct();
            //         taskStruct.TaskId = taskConfigs[i].Id;
            //         if (taskStruct.TaskId == this.HalfHourTaskId)
            //         {
            //             taskStruct.State = (int)GetHalfTaskState();
            //         }
            //         else
            //         {
            //             taskStruct.Total = taskConfigs[i].condition;
            //             taskStruct.Progress = 0;
            //             if(taskStruct.Progress >= taskStruct.Total)
            //                 taskStruct.State = (int)TaskState.getingReward;
            //             else
            //                 taskStruct.State = (int)TaskState.geting;
            //         }
            //         taskList.Add(taskStruct);
            //     }
            // }
            // else
            // {
            //     string[] taskOneStr = taskStr.Split(',');
            //     for (int k = 0; k < taskOneStr.Length; k++)
            //     {
            //         string[] taskInfo = taskOneStr[k].Split(':');
            //         TaskStruct taskStruct = new TaskStruct();
            //         int taskId = int.Parse(taskInfo[0]);
            //         TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskId);
            //         taskStruct.TaskId = int.Parse(taskInfo[0]);
            //         taskStruct.Total = taskConfig.condition;
            //         taskStruct.Progress = int.Parse(taskInfo[1]);
            //         taskStruct.State = int.Parse(taskInfo[2]);
            //         taskList.Add(taskStruct);
            //     }
            // }
            // return this.taskList;
        }

        /// <summary>
        /// 是否是消消乐解锁皮肤关卡
        /// </summary>
        /// <returns></returns>
        public int IsMbTaskSkinReward(int passLv)
        {
            if (this.taskList == null)
                return 0;
            for (int i = 0; i < this.taskList.Count; i++)
            {
                if (this.taskList[i].State == (int)TaskState.getingReward)
                {
                    TaskConfig cfg = TaskConfigCategory.Instance.Get(this.taskList[i].TaskId);
                    if (cfg.type == TaskType.PassLv && passLv == cfg.param1)
                    {
                        string[] rewards = cfg.reward.Split(',');
                        for (int n = 0; n < rewards.Length; n++)
                        {
                            string[] rewards1 = rewards[n].Split(':');
                            if (int.Parse(rewards1[0]) == 2)
                            {
                                return cfg.Id;
                            }
                        }
                    }
                }
            }
            return 0;
        }

        public void GetReward(int TaskId)
        {
            if (TaskId == this.HalfHourTaskId)
            {
                TaskConfig taskConfig = TaskConfigCategory.Instance.Get(TaskId);
                LastResetTime = TimeInfo.Instance.ClientNow() / 1000 + taskConfig.resetTime * 60;
            }
            //this.SaveTask();
        }

        public void SaveTask()
        {
            List<TaskStruct> taskStructs = GetTaskList();
            String taskStr = "";
            for (int i = 0; i < taskStructs.Count; i++)
            {
                string ts = taskStructs[i].TaskId + ":" + taskStructs[i].Progress + ":" + taskStructs[i].State;
                if (i == taskStructs.Count - 1)
                {
                    taskStr += ts;
                }
                else
                {
                    taskStr += ts + ",";
                }
            }
            PlayerPrefs.SetString(ItemSaveStr.TaskList, taskStr);
        }

        public long LastResetTime
        {
            set
            {
                //Log.Error("LastResetTime");
                this.lastResetTime = value;
                //PlayerPrefs.SetString(ItemSaveStr.HalfHourTask, value.ToString());
            }
            get
            {
                // lastResetTime = long.Parse(PlayerPrefs.GetString(ItemSaveStr.HalfHourTask, "0"));
                // if (lastResetTime == 0)
                // {
                //     this.lastResetTime = TimeInfo.Instance.ClientNow() / 1000 + 30 * 60;
                //     PlayerPrefs.SetString(ItemSaveStr.HalfHourTask, lastResetTime.ToString());
                // }
                return this.lastResetTime;
            }
        }
    }
}