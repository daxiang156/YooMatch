using UnityEngine;
using System;

namespace ET
{
    public class SEntityComponent : Entity, IAwake, IUpdate, IDestroy
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        public String id;
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 pos = new Vector3(0, 600, 0);
        /// <summary>
        /// 角度
        /// </summary>
        public Vector3 angle;
        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 scale;
        /// <summary>
        /// 实体
        /// </summary>
        public GameObject creatureObj;
        /// <summary>
        /// 速度
        /// </summary>
        public float speed;
        /// <summary>
        /// 出生点
        /// </summary>
        public Vector3 originPos;
    }
}