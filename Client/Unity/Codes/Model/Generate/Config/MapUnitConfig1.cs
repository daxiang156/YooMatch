using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class MapUnitConfig1Category : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static MapUnitConfig1Category _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static MapUnitConfig1Category Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(MapUnitConfig1Category));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapUnitConfig1> dict = new Dictionary<int, MapUnitConfig1>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapUnitConfig1> list = new List<MapUnitConfig1>();
		
        public MapUnitConfig1Category()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (MapUnitConfig1 config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MapUnitConfig1 Get(int id)
        {
            this.dict.TryGetValue(id, out MapUnitConfig1 item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (MapUnitConfig1)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapUnitConfig1> GetAll()
        {
            return this.dict;
        }

        public MapUnitConfig1 GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapUnitConfig1: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int AI_Type { get; set; }
		[ProtoMember(3)]
		public int Diam { get; set; }
		[ProtoMember(4)]
		public int rangeLeft { get; set; }
		[ProtoMember(5)]
		public int rangSkip { get; set; }

	}
}
