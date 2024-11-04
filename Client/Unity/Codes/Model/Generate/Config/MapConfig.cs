using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class MapConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static MapConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static MapConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(MapConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapConfig> dict = new Dictionary<int, MapConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapConfig> list = new List<MapConfig>();
		
        public MapConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (MapConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MapConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MapConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (MapConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapConfig> GetAll()
        {
            return this.dict;
        }

        public MapConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int mapType { get; set; }
		[ProtoMember(4)]
		public int condition { get; set; }
		[ProtoMember(5)]
		public string source { get; set; }
		[ProtoMember(6)]
		public int PlayType { get; set; }
		[ProtoMember(7)]
		public int PlayerMax { get; set; }
		[ProtoMember(8)]
		public int CampNum { get; set; }
		[ProtoMember(9)]
		public int resultType { get; set; }
		[ProtoMember(10)]
		public string reward1 { get; set; }
		[ProtoMember(11)]
		public string reward2 { get; set; }
		[ProtoMember(12)]
		public string reward3 { get; set; }
		[ProtoMember(13)]
		public string failReward { get; set; }

	}
}
