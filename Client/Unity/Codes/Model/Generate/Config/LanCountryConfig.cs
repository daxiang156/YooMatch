using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class LanCountryConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static LanCountryConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static LanCountryConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(LanCountryConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LanCountryConfig> dict = new Dictionary<int, LanCountryConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LanCountryConfig> list = new List<LanCountryConfig>();
		
        public LanCountryConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (LanCountryConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LanCountryConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LanCountryConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (LanCountryConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LanCountryConfig> GetAll()
        {
            return this.dict;
        }

        public LanCountryConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LanCountryConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string language { get; set; }
		[ProtoMember(3)]
		public string countrys { get; set; }
		[ProtoMember(8)]
		public int guidePeople { get; set; }
		[ProtoMember(9)]
		public int posX { get; set; }
		[ProtoMember(10)]
		public int posY { get; set; }
		[ProtoMember(11)]
		public int fingerPosX { get; set; }
		[ProtoMember(12)]
		public int fingerPosY { get; set; }
		[ProtoMember(13)]
		public int rotation { get; set; }
		[ProtoMember(14)]
		public int btnPosX { get; set; }
		[ProtoMember(15)]
		public int btnPosY { get; set; }
		[ProtoMember(16)]
		public int btnWide { get; set; }
		[ProtoMember(17)]
		public int btnHeight { get; set; }
		[ProtoMember(18)]
		public int anchors { get; set; }
		[ProtoMember(19)]
		public int fouceGuide { get; set; }

	}
}
