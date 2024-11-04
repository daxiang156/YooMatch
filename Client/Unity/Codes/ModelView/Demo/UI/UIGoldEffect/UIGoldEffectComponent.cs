using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class UIGoldEffectComponent : Entity, IAwake<object>, IDestroy,IUpdate
    {
        public GameObject goldParent;
        public GameObject go_goldEffect;
        public List<GameObject> listGold;

        public Vector3 InitPosition;
        public float deltaTime = 0f;
        public Vector3 mTempPosition;
    }
}