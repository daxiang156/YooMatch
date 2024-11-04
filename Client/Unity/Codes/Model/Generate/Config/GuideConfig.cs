using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class GuideConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static GuideConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static GuideConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(GuideConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GuideConfig> dict = new Dictionary<int, GuideConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GuideConfig> list = new List<GuideConfig>();
		
        public GuideConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (GuideConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GuideConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GuideConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (GuideConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GuideConfig> GetAll()
        {
            return this.dict;
        }

        public GuideConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GuideConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string guideTxtIn { get; set; }
		[ProtoMember(3)]
		public string guideTxtEn { get; set; }
		[ProtoMember(4)]
		public string guideTxtJP { get; set; }
		[ProtoMember(5)]
		public string guideTxtThailand { get; set; }
		[ProtoMember(6)]
		public string guideTxtVietnamese { get; set; }
		[ProtoMember(7)]
		public string guideTxtHindi { get; set; }
		[ProtoMember(8)]
		public string guideTxtSpanish { get; set; }
		[ProtoMember(9)]
		public string guideTxtBrazil { get; set; }
		[ProtoMember(11)]
		public int guidePeople { get; set; }
		[ProtoMember(12)]
		public int posX { get; set; }
		[ProtoMember(13)]
		public int posY { get; set; }
		[ProtoMember(14)]
		public int fingerPosX { get; set; }
		[ProtoMember(15)]
		public int fingerPosY { get; set; }
		[ProtoMember(16)]
		public int rotation { get; set; }
		[ProtoMember(17)]
		public int btnPosX { get; set; }
		[ProtoMember(18)]
		public int btnPosY { get; set; }
		[ProtoMember(19)]
		public int btnWide { get; set; }
		[ProtoMember(20)]
		public int btnHeight { get; set; }
		[ProtoMember(21)]
		public int anchors { get; set; }
		[ProtoMember(22)]
		public int fouceGuide { get; set; }

	}
}
