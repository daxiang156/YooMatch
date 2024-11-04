using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ActivityConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static ActivityConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static ActivityConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(ActivityConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ActivityConfig> dict = new Dictionary<int, ActivityConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ActivityConfig> list = new List<ActivityConfig>();
		
        public ActivityConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (ActivityConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ActivityConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ActivityConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (ActivityConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ActivityConfig> GetAll()
        {
            return this.dict;
        }

        public ActivityConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ActivityConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Desc { get; set; }
		[ProtoMember(3)]
		public string goodsId { get; set; }
		[ProtoMember(4)]
		public long startTime { get; set; }
		[ProtoMember(5)]
		public long endTime { get; set; }

	}
}
