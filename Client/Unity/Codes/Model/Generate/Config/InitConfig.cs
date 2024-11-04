using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class InitConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static InitConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static InitConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(InitConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, InitConfig> dict = new Dictionary<int, InitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<InitConfig> list = new List<InitConfig>();
		
        public InitConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (InitConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public InitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out InitConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (InitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, InitConfig> GetAll()
        {
            return this.dict;
        }

        public InitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class InitConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int PlatformCoin { get; set; }
		[ProtoMember(3)]
		public int GameCoin { get; set; }
		[ProtoMember(4)]
		public int Level { get; set; }
		[ProtoMember(5)]
		public int SkinId { get; set; }
		[ProtoMember(6)]
		public int CityMapId { get; set; }
		[ProtoMember(7)]
		public int singleMax { get; set; }
		[ProtoMember(8)]
		public int PickUpCd { get; set; }
		[ProtoMember(9)]
		public int physical { get; set; }
		[ProtoMember(10)]
		public string initItem { get; set; }
		[ProtoMember(11)]
		public int chatLimite { get; set; }
		[ProtoMember(12)]
		public int turnTableNum { get; set; }
		[ProtoMember(13)]
		public int turnTableCD { get; set; }
		[ProtoMember(14)]
		public int funGameTicket { get; set; }
		[ProtoMember(15)]
		public int gridCost { get; set; }
		[ProtoMember(16)]
		public string boosLevel { get; set; }

	}
}
