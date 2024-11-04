using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class EntityConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static EntityConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static EntityConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(EntityConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EntityConfig> dict = new Dictionary<int, EntityConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EntityConfig> list = new List<EntityConfig>();
		
        public EntityConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (EntityConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EntityConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EntityConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (EntityConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EntityConfig> GetAll()
        {
            return this.dict;
        }

        public EntityConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EntityConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int Type { get; set; }
		[ProtoMember(3)]
		public string ModelName { get; set; }
		[ProtoMember(4)]
		public string Name { get; set; }
		[ProtoMember(5)]
		public string Desc { get; set; }
		[ProtoMember(6)]
		public int Position { get; set; }
		[ProtoMember(7)]
		public int Height { get; set; }
		[ProtoMember(8)]
		public int Weight { get; set; }

	}
}
