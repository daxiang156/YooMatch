using UnityEngine;

namespace ET
{
    public class SEntity : Entity, IAwake<int>
    {
        
        public int ConfigId; //配置表id

        public EntityConfig Config => EntityConfigCategory.Instance.Get(this.ConfigId);
        
        private Vector3 position; //坐标

        public Vector3 Position
        {
            get => this.position;
            set
            {
                Vector3 oldPos = this.position;
                this.position = value;
                Game.EventSystem.Publish(new EventType.EntityChangePosition(){ sEntity = this, OldPos = oldPos });
            }
        }
        
        public Vector3 Forward
        {
            get => this.Rotation * Vector3.forward;
            set => this.Rotation = Quaternion.LookRotation(value, Vector3.up);
        }

        private Quaternion rotation;
        public Quaternion Rotation
        {
            get => this.rotation;
            set
            {
                this.rotation = value;
                Game.EventSystem.Publish(new EventType.EntityChangeRotation() {sEntity = this});
            }
        }
    }
}