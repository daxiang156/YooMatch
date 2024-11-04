using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class FireBComponent: Entity, IAwake, IDestroy
    {
        public static FireBComponent Instance;
        public int initFinish = 0;
    }
}