using  UnityEngine;
using System.Collections.Generic;
using  System;
using Random = System.Random;

namespace ET
{
    [ObjectSystem]
    public class SEntityComponentAwakeBase : AwakeSystem<SEntityComponent>
    {
        private SEntityComponent self;

        public override void Awake(SEntityComponent self)
        {
            this.self = self;
            self.id = "" + Time.time + UnityEngine.Random.Range(1, 100);
        }
    }
    
    public static class SEntityComponentSystem
    {
        public static void SetEntity(this SEntityComponent self, GameObject unit)
        {
            self.creatureObj = unit;
        }
    }
}