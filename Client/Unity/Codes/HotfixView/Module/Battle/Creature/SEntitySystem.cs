namespace ET
{
    public class SEntitySystem : AwakeSystem<SEntity, int>
    {
        private SEntity self;

        public override void Awake(SEntity self, int configId)
        {
            this.self = self;
            this.self.ConfigId = configId;
        }
    }
}