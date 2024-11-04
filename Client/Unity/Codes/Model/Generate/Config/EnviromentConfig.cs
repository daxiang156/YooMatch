using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class EnviromentConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static EnviromentConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static EnviromentConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(EnviromentConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EnviromentConfig> dict = new Dictionary<int, EnviromentConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EnviromentConfig> list = new List<EnviromentConfig>();
		
        public EnviromentConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (EnviromentConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EnviromentConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EnviromentConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (EnviromentConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EnviromentConfig> GetAll()
        {
            return this.dict;
        }

        public EnviromentConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EnviromentConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public int param1 { get; set; }
		[ProtoMember(4)]
		public string param2 { get; set; }

	}
}
