namespace ET
{
    [ObjectSystem]
    public class UITipInfoSystem : AwakeSystem<TipInfoComponent>
    {
        public override void Awake(TipInfoComponent self)
        {
            TipInfoComponent.Instance = self;
        }
    }

    public static class TipInfoSystem
    {
        public static void InitTipInfo(this TipInfoComponent self, int itemId, int itemNum, int usdeNum = -1000)
        {
            self.itemId = itemId;
            self.itemNum = itemNum;
            self.usdeNum = usdeNum;
        }
    }
}