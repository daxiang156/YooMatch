using  System.Collections.Generic;
using  System;
using UnityEngine;

namespace ET
{
    public class EntityMgr
    {
        private static EntityMgr instance;
        public static EntityMgr Instance
        {
            get
            {
                if(instance == null) 
                {
                    instance = new EntityMgr();
                }
                return instance;
            }
        }

        /// <summary>
        /// 逻辑对象列表
        /// </summary>
        private List<SEntityComponent> m_Objects = new List<SEntityComponent>();

        private Transform parent;
        private GameObject entityObj;
        public void Init(Transform parent, GameObject entityObj)
        {
            this.parent = parent;
            this.entityObj = entityObj;
        }

        /// <summary>
        /// 创建一个玩家
        /// </summary>
        /// <param name="entityData">玩家数据</param>
        /// <returns></returns>
        public SEntityComponent CreateEntity(SEntityComponent entityData)
        {
            SEntityComponent entity = new SEntityComponent();
            GameObject obj = GameObject.Instantiate(this.entityObj) as GameObject;
            obj.transform.parent = this.parent;
            obj.transform.localPosition = entityData.pos;
            obj.transform.localRotation = Quaternion.Euler(entityData.angle);
            obj.transform.localScale = entityData.scale;
            //entity.Init(obj, entityData);
            //EntityMgr.Instance.AddObject(entity);
            return entity;
        }
        
        /// <summary>
        /// 添加一个游戏对象
        /// </summary>
        /// <param name="logicalObject"></param>
        public void AddObject(SEntityComponent logicalObject)
        {
            m_Objects.Add(logicalObject);
        }
    }
    
}