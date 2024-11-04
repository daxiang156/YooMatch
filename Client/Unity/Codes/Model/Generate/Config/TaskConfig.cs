using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class TaskConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static TaskConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static TaskConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(TaskConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, TaskConfig> dict = new Dictionary<int, TaskConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<TaskConfig> list = new List<TaskConfig>();
		
        public TaskConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (TaskConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public TaskConfig Get(int id)
        {
            this.dict.TryGetValue(id, out TaskConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (TaskConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, TaskConfig> GetAll()
        {
            return this.dict;
        }

        public TaskConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class TaskConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public int descLangId { get; set; }
		[ProtoMember(4)]
		public int IsTag { get; set; }
		[ProtoMember(5)]
		public int btype { get; set; }
		[ProtoMember(6)]
		public int type { get; set; }
		[ProtoMember(7)]
		public int param1 { get; set; }
		[ProtoMember(8)]
		public int condition { get; set; }
		[ProtoMember(9)]
		public string reward { get; set; }
		[ProtoMember(10)]
		public string maxNum { get; set; }
		[ProtoMember(11)]
		public int resetTime { get; set; }
		[ProtoMember(12)]
		public int order { get; set; }

	}
}
