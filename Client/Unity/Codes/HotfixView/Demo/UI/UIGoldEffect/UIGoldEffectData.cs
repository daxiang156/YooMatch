using UnityEngine;
using System;
using UnityEngine.UI;

namespace ET
{
    public class UIGoldEffectData
    {
        public UIGoldEffectData()
        {
            
        }

        /// <summary>
        /// 显示金币特效个数
        /// </summary>
        public int showNum = 10;
        /// <summary>
        /// 目标位置  世界坐标系
        /// </summary>
        public Vector3 targetPos;
        /// <summary>
        /// 旋转坐标  世界坐标系
        /// </summary>
        public Vector3 rotationPos;
        /// <summary>
        /// 起点位置 世界坐标
        /// </summary>
        public Vector3 resoucePos;
        /// <summary>
        /// 目标文本框
        /// </summary>
        public Text txt_num;
        /// <summary>
        /// 新金币数量
        /// </summary>
        public int newValue;
        /// <summary>
        /// 旧金币数
        /// </summary>
        public int oldValue;
        
        public Action<int> callBack;
    }
}