using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class EntityMoveHelper
    {
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static async ETTask<int> MoveToAsync(this SEntity unit, Vector3 targetPos, ETCancellationToken cancellationToken = null)
        {
            C2M_PathfindingResult msg = new C2M_PathfindingResult() {X = targetPos.x, Y = targetPos.y, Z = targetPos.z};
            unit.ZoneScene().GetComponent<SessionComponent>().Session.Send(msg);

            ObjectWait objectWait = unit.GetComponent<ObjectWait>();
            
            // 要取消上一次的移动协程
            objectWait.Notify(new WaitType.Wait_UnitStop() { Error = WaitTypeError.Cancel });
            
            // 一直等到unit发送stop
            WaitType.Wait_UnitStop waitUnitStop = await objectWait.Wait<WaitType.Wait_UnitStop>(cancellationToken);
            return waitUnitStop.Error;
        }
        
        public static async ETTask<bool> MoveToAsync(this SEntity unit, List<Vector3> path)
        {
            float speed = 3;//unit.GetComponent<EntityMoveComponent>().Speed;
            EntityMoveComponent moveComponent = unit.GetComponent<EntityMoveComponent>();
            bool ret = await moveComponent.MoveToAsync(path, speed);
            return ret;
        }
    }
}