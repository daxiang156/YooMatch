using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LanguageConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static LanguageConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static LanguageConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(LanguageConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LanguageConfig> dict = new Dictionary<int, LanguageConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LanguageConfig> list = new List<LanguageConfig>();
		
        public LanguageConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (LanguageConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LanguageConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LanguageConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (LanguageConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LanguageConfig> GetAll()
        {
            return this.dict;
        }

        public LanguageConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LanguageConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public string English { get; set; }
		[ProtoMember(4)]
		public string Indonesian { get; set; }
		[ProtoMember(5)]
		public string Vietnam { get; set; }
		[ProtoMember(6)]
		public string Thai { get; set; }
		[ProtoMember(7)]
		public string Brazil { get; set; }
		[ProtoMember(8)]
		public string India { get; set; }
		[ProtoMember(9)]
		public string Spanish { get; set; }

	}
}
