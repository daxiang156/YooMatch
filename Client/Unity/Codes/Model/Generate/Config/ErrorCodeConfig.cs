using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ErrorCodeConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static ErrorCodeConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static ErrorCodeConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(ErrorCodeConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ErrorCodeConfig> dict = new Dictionary<int, ErrorCodeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ErrorCodeConfig> list = new List<ErrorCodeConfig>();
		
        public ErrorCodeConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (ErrorCodeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ErrorCodeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ErrorCodeConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (ErrorCodeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ErrorCodeConfig> GetAll()
        {
            return this.dict;
        }

        public ErrorCodeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ErrorCodeConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Text { get; set; }
		[ProtoMember(3)]
		public int PoupType { get; set; }
		[ProtoMember(4)]
		public string English { get; set; }

	}
}
