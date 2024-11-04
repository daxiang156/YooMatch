using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StartProcessConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static StartProcessConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static StartProcessConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(StartProcessConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StartProcessConfig> dict = new Dictionary<int, StartProcessConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StartProcessConfig> list = new List<StartProcessConfig>();
		
        public StartProcessConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (StartProcessConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public StartProcessConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StartProcessConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (StartProcessConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StartProcessConfig> GetAll()
        {
            return this.dict;
        }

        public StartProcessConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StartProcessConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int MachineId { get; set; }
		[ProtoMember(3)]
		public int InnerPort { get; set; }
		[ProtoMember(4)]
		public string AppName { get; set; }

	}
}
