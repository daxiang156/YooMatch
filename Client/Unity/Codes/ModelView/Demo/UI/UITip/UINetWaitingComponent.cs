using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UINetWaitingComponent: Entity, IAwake, IDestroy
    {
        public GameObject wait;
        public GameObject normal;
        public GameObject AdLoading;
    }
}