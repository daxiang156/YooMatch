using UnityEngine;

namespace ET
{
    public class UIAniEffectComponent : Entity, IAwake<object>, IDestroy,IUpdate
    {
        public GameObject aniParent;
        public GameObject bundleGameObject;
    }
}