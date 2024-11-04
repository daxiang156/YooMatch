using  System.Collections.Generic;

namespace ET
{
    public class BattleComponent : Entity, IAwake, IUpdate, IDestroy
    {
        public List<SEntity> createList = new List<SEntity>();

        public bool isStart = false;
        //public List<SUnit> sUnitList = new List<SUnit>();
    }
}