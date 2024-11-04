using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StartMachineConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static StartMachineConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static StartMachineConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(StartMachineConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StartMachineConfig> dict = new Dictionary<int, StartMachineConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StartMachineConfig> list = new List<StartMachineConfig>();
		
        public StartMachineConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (StartMachineConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public StartMachineConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StartMachineConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (StartMachineConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StartMachineConfig> GetAll()
        {
            return this.dict;
        }

        public StartMachineConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StartMachineConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string InnerIP { get; set; }
		[ProtoMember(3)]
		public string OuterIP { get; set; }
		[ProtoMember(4)]
		public string WatcherPort { get; set; }

	}
}
