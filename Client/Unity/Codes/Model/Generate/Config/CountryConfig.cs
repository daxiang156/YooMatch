using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class CountryConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static CountryConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static CountryConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(CountryConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, CountryConfig> dict = new Dictionary<int, CountryConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<CountryConfig> list = new List<CountryConfig>();
		
        public CountryConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (CountryConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public CountryConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CountryConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (CountryConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CountryConfig> GetAll()
        {
            return this.dict;
        }

        public CountryConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class CountryConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Country { get; set; }
		[ProtoMember(3)]
		public string Simple { get; set; }

	}
}
