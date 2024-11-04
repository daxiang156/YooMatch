using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class AttrEffectConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static AttrEffectConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static AttrEffectConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(AttrEffectConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AttrEffectConfig> dict = new Dictionary<int, AttrEffectConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AttrEffectConfig> list = new List<AttrEffectConfig>();
		
        public AttrEffectConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (AttrEffectConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AttrEffectConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AttrEffectConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (AttrEffectConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AttrEffectConfig> GetAll()
        {
            return this.dict;
        }

        public AttrEffectConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AttrEffectConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(3)]
		public int LangId { get; set; }
		[ProtoMember(4)]
		public int AttrType { get; set; }
		[ProtoMember(5)]
		public string Value { get; set; }
		[ProtoMember(6)]
		public int DescLangId { get; set; }
		[ProtoMember(7)]
		public string icon { get; set; }
		[ProtoMember(8)]
		public int order { get; set; }

	}
}
