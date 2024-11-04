using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class GameInfoAwakeSystem: AwakeSystem<GameInfoComponent>
    {
        public override void Awake(GameInfoComponent self)
        {
            GameInfoComponent.Instance = self;
        }
    }
}