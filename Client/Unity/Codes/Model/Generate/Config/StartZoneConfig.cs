using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StartZoneConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static StartZoneConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static StartZoneConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(StartZoneConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StartZoneConfig> dict = new Dictionary<int, StartZoneConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StartZoneConfig> list = new List<StartZoneConfig>();
		
        public StartZoneConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (StartZoneConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public StartZoneConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StartZoneConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (StartZoneConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StartZoneConfig> GetAll()
        {
            return this.dict;
        }

        public StartZoneConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StartZoneConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string DBConnection { get; set; }
		[ProtoMember(3)]
		public string DBName { get; set; }

	}
}