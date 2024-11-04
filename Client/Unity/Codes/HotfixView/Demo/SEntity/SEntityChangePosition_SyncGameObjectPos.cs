using UnityEngine;

namespace ET
{
    public class SEntityChangePosition_SyncGameObjectPos: AEvent<EventType.EntityChangePosition>
    {
        protected override async ETTask Run(EventType.EntityChangePosition args)
        {
            SEntityComponent gameObjectComponent = args.sEntity.GetComponent<SEntityComponent>();
            if (gameObjectComponent == null)
            {
                return;
            }
            Transform transform = gameObjectComponent.creatureObj.transform;
            transform.position = args.sEntity.Position;
            await ETTask.CompletedTask;
        }
    }
}