using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class UIAniEffectData
    {
        /// <summary>
        /// 显示星级特效个数
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
        public List<Vector3> resoucePos;
        /// <summary>
        /// 特效播完
        /// </summary>
        public Action<int> callBack;
    }
}