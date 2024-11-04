using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class funyGameConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static funyGameConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static funyGameConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(funyGameConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, funyGameConfig> dict = new Dictionary<int, funyGameConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<funyGameConfig> list = new List<funyGameConfig>();
		
        public funyGameConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (funyGameConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public funyGameConfig Get(int id)
        {
            this.dict.TryGetValue(id, out funyGameConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (funyGameConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, funyGameConfig> GetAll()
        {
            return this.dict;
        }

        public funyGameConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class funyGameConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string reward1 { get; set; }
		[ProtoMember(3)]
		public string reward2 { get; set; }

	}
}
