using UnityEngine;

namespace ET
{
    public static class SEntityFactory
    {
        public static SEntity Create(Scene currentScene, SEntityInfo entityInfo)
        {
            BattleComponent battleComponent = currentScene.GetComponent<BattleComponent>();
            SEntity entity = battleComponent.AddChildWithId<SEntity, int>(entityInfo.Id, entityInfo.configId);
            //battleComponent.AddCreate(entity);
	        
            // NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
            // for (int i = 0; i < unitInfo.Ks.Count; ++i)
            // {
            //     numericComponent.Set(unitInfo.Ks[i], unitInfo.Vs[i]);
            // }
	        
            //entity.AddComponent<EntityMoveComponent>();
            // using (ListComponent<Vector3> list = ListComponent<Vector3>.Create())
            // {
            //     list.Add(entity.Position);
            //     list.Add(new Vector3(0, 0, 120));
            //     entity.MoveToAsync(list).Coroutine();
            // }


            entity.AddComponent<ObjectWait>();

            entity.AddComponent<XunLuoPathComponent>();
	        
            //Game.EventSystem.Publish(new EventTypeView.AfterEntityCreate() {sEntity = entity});
            return entity;
        }
    }
}