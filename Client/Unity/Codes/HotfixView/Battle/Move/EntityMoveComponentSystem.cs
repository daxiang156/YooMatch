using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [Timer(TimerType.EntityMoveTimer)]
    public class EntityMoveTimer: ATimer<EntityMoveComponent>
    {
        public override void Run(EntityMoveComponent self)
        {
            try
            {
                self.MoveForward(false);
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }
    
    [ObjectSystem]
    public class EntityMoveComponentDestroySystem: DestroySystem<EntityMoveComponent>
    {
        public override void Destroy(EntityMoveComponent self)
        {
            self.Clear();
        }
    }

    [ObjectSystem]
    public class EntityMoveComponentAwakeSystem: AwakeSystem<EntityMoveComponent>
    {
        public override void Awake(EntityMoveComponent self)
        {
            self.StartTime = 0;
            self.StartPos = Vector3.zero;
            self.NeedTime = 0;
            self.MoveTimer = 0;
            self.Callback = null;
            self.Targets.Clear();
            self.Speed = 3;
            self.N = 0;
            self.TurnTime = 0;
        }
    }

    public static class EntityMoveComponentSystem
    {
        public static bool IsArrived(this EntityMoveComponent self)
        {
            return self.Targets.Count == 0;
        }

        public static bool ChangeSpeed(this EntityMoveComponent self, float speed)
        {
            if (self.IsArrived())
            {
                return false;
            }

            if (speed < 0.0001)
            {
                return false;
            }
            
            Unit unit = self.GetParent<Unit>();

            using (ListComponent<Vector3> path = ListComponent<Vector3>.Create())
            {
                self.MoveForward(true);
                
                path.Add(unit.Position); // 第一个是Unit的pos
                for (int i = self.N; i < self.Targets.Count; ++i)
                {
                    path.Add(self.Targets[i]);
                }
                self.MoveToAsync(path, speed).Coroutine();
            }
            return true;
        }

        public static async ETTask<bool> MoveToAsync(this EntityMoveComponent self, List<Vector3> target, float speed, int turnTime = 100, ETCancellationToken cancellationToken = null)
        {
            self.Stop();

            foreach (Vector3 v in target)
            {
                self.Targets.Add(v);
            }

            self.IsTurnHorizontal = true;
            self.TurnTime = turnTime;
            self.Speed = speed;
            ETTask<bool> tcs = ETTask<bool>.Create(true);
            self.Callback = (ret) => { tcs.SetResult(ret); };

            Game.EventSystem.Publish(new EventType.SEntityMoveStart(){SEntity = self.GetParent<SEntity>()});
            
            self.StartMove();
            
            void CancelAction()
            {
                self.Stop();
            }
            
            bool moveRet;
            try
            {
                cancellationToken?.Add(CancelAction);
                moveRet = await tcs;
            }
            finally
            {
                cancellationToken?.Remove(CancelAction);
            }

            if (moveRet)
            {
                Game.EventSystem.Publish(new EventType.SEntityMoveStop(){SEntity = self.GetParent<SEntity>()});
            }
            return moveRet;
        }

        public static void MoveForward(this EntityMoveComponent self, bool needCancel)
        {
            SEntity unit = self.GetParent<SEntity>();
            
            long timeNow = TimeHelper.ClientNow();
            long moveTime = timeNow - self.StartTime;

            while (true)
            {
                if (moveTime <= 0)
                {
                    return;
                }
                
                // 计算位置插值
                if (moveTime >= self.NeedTime)
                {
                    unit.Position = self.NextTarget;
                    if (self.TurnTime > 0)
                    {
                        unit.Rotation = self.To;
                    }
                }
                else
                {
                    // 计算位置插值
                    float amount = moveTime * 1f / self.NeedTime;
                    if (amount > 0)
                    {
                        Vector3 newPos = Vector3.Lerp(self.StartPos, self.NextTarget, amount);
                        unit.Position = newPos;
                    }
                    
                    // 计算方向插值
                    if (self.TurnTime > 0)
                    {
                        amount = moveTime * 1f / self.TurnTime;
                        Quaternion q = Quaternion.Slerp(self.From, self.To, amount);
                        unit.Rotation = q;
                    }
                }

                moveTime -= self.NeedTime;

                // 表示这个点还没走完，等下一帧再来
                if (moveTime < 0)
                {
                    return;
                }
                
                // 到这里说明这个点已经走完
                
                // 如果是最后一个点
                if (self.N >= self.Targets.Count - 1)
                {
                    unit.Position = self.NextTarget;
                    unit.Rotation = self.To;

                    Action<bool> callback = self.Callback;
                    self.Callback = null;

                    self.Clear();
                    callback?.Invoke(!needCancel);
                    return;
                }

                self.SetNextTarget();
            }
        }

        private static void StartMove(this EntityMoveComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            
            self.BeginTime = TimeHelper.ClientNow();
            self.StartTime = self.BeginTime;
            self.SetNextTarget();

            self.MoveTimer = TimerComponent.Instance.NewFrameTimer(TimerType.EntityMoveTimer, self);
        }

        private static void SetNextTarget(this EntityMoveComponent self)
        {

            SEntity unit = self.GetParent<SEntity>();

            ++self.N;

            // 时间计算用服务端的位置, 但是移动要用客户端的位置来插值
            Vector3 v = self.GetFaceV();
            float distance = v.magnitude;
            
            // 插值的起始点要以unit的真实位置来算
            self.StartPos = unit.Position;

            self.StartTime += self.NeedTime;
            
            self.NeedTime = (long) (distance / self.Speed * 1000);

            
            if (self.TurnTime > 0)
            {
                // 要用unit的位置
                Vector3 faceV = self.GetFaceV();
                if (faceV.sqrMagnitude < 0.0001f)
                {
                    return;
                }
                self.From = unit.Rotation;
                
                if (self.IsTurnHorizontal)
                {
                    faceV.y = 0;
                }

                if (Math.Abs(faceV.x) > 0.01 || Math.Abs(faceV.z) > 0.01)
                {
                    self.To = Quaternion.LookRotation(faceV, Vector3.up);
                }

                return;
            }
            
            if (self.TurnTime == 0) // turn time == 0 立即转向
            {
                Vector3 faceV = self.GetFaceV();
                if (self.IsTurnHorizontal)
                {
                    faceV.y = 0;
                }

                if (Math.Abs(faceV.x) > 0.01 || Math.Abs(faceV.z) > 0.01)
                {
                    self.To = Quaternion.LookRotation(faceV, Vector3.up);
                    unit.Rotation = self.To;
                }
            }
        }

        private static Vector3 GetFaceV(this EntityMoveComponent self)
        {
            return self.NextTarget - self.PreTarget;
        }

        public static bool FlashTo(this EntityMoveComponent self, Vector3 target)
        {
            Unit unit = self.GetParent<Unit>();
            unit.Position = target;
            return true;
        }

        public static void Stop(this EntityMoveComponent self)
        {
            if (self.Targets.Count > 0)
            {
                self.MoveForward(true);
            }

            self.Clear();
        }

        public static void Clear(this EntityMoveComponent self)
        {
            self.StartTime = 0;
            self.StartPos = Vector3.zero;
            self.BeginTime = 0;
            self.NeedTime = 0;
            TimerComponent.Instance?.Remove(ref self.MoveTimer);
            self.Targets.Clear();
            self.Speed = 0;
            self.N = 0;
            self.TurnTime = 0;
            self.IsTurnHorizontal = false;

            if (self.Callback != null)
            {
                Action<bool> callback = self.Callback;
                self.Callback = null;
                callback.Invoke(false);
            }
        }
    }
}