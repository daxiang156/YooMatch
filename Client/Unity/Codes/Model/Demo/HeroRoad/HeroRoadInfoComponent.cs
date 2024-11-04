using System.Collections.Generic;

namespace ET
{
    public class HeroRoadInfoComponent : Entity, IAwake,IDestroy
    {
        public int curProgress;
        public List<HeroItemInfo> AchList = new List<HeroItemInfo>();
    }
    
    public class HeroItemInfo
    {
        public int AchId;
        public int state;
    }
}