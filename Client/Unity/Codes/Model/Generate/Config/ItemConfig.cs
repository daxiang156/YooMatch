using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ItemConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static ItemConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static ItemConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(ItemConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ItemConfig> dict = new Dictionary<int, ItemConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ItemConfig> list = new List<ItemConfig>();
		
        public ItemConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (ItemConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ItemConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ItemConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (ItemConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ItemConfig> GetAll()
        {
            return this.dict;
        }

        public ItemConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ItemConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public string Name { get; set; }
		[ProtoMember(4)]
		public string Desc { get; set; }
		[ProtoMember(5)]
		public long MaxNum { get; set; }
		[ProtoMember(6)]
		public int type { get; set; }
		[ProtoMember(7)]
		public int stype { get; set; }
		[ProtoMember(8)]
		public int dropId { get; set; }
		[ProtoMember(9)]
		public string modelName { get; set; }
		[ProtoMember(10)]
		public string Icon { get; set; }
		[ProtoMember(11)]
		public string price { get; set; }
		[ProtoMember(12)]
		public long limitTime { get; set; }
		[ProtoMember(14)]
		public int useType { get; set; }

	}
}
