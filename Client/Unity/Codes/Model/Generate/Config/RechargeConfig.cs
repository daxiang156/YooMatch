using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class RechargeConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static RechargeConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static RechargeConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(RechargeConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RechargeConfig> dict = new Dictionary<int, RechargeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RechargeConfig> list = new List<RechargeConfig>();
		
        public RechargeConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (RechargeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public RechargeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RechargeConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (RechargeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RechargeConfig> GetAll()
        {
            return this.dict;
        }

        public RechargeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RechargeConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string productId { get; set; }
		[ProtoMember(3)]
		public int type { get; set; }
		[ProtoMember(4)]
		public string Name { get; set; }
		[ProtoMember(5)]
		public string Price { get; set; }
		[ProtoMember(6)]
		public string RewardStr { get; set; }
		[ProtoMember(7)]
		public int sale { get; set; }
		[ProtoMember(8)]
		public int rank { get; set; }

	}
}
