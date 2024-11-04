using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ActivityGoodsConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static ActivityGoodsConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static ActivityGoodsConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(ActivityGoodsConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ActivityGoodsConfig> dict = new Dictionary<int, ActivityGoodsConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ActivityGoodsConfig> list = new List<ActivityGoodsConfig>();
		
        public ActivityGoodsConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (ActivityGoodsConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ActivityGoodsConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ActivityGoodsConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (ActivityGoodsConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ActivityGoodsConfig> GetAll()
        {
            return this.dict;
        }

        public ActivityGoodsConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ActivityGoodsConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Desc { get; set; }
		[ProtoMember(3)]
		public int type { get; set; }
		[ProtoMember(4)]
		public string goods { get; set; }
		[ProtoMember(5)]
		public string cost { get; set; }
		[ProtoMember(6)]
		public int buyTimes { get; set; }
		[ProtoMember(7)]
		public string icon { get; set; }

	}
}
