using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class GameResultConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static GameResultConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static GameResultConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(GameResultConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GameResultConfig> dict = new Dictionary<int, GameResultConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GameResultConfig> list = new List<GameResultConfig>();
		
        public GameResultConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (GameResultConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GameResultConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GameResultConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (GameResultConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GameResultConfig> GetAll()
        {
            return this.dict;
        }

        public GameResultConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GameResultConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string mapID { get; set; }
		[ProtoMember(3)]
		public string Name { get; set; }
		[ProtoMember(4)]
		public string Rank { get; set; }
		[ProtoMember(5)]
		public string reward { get; set; }

	}
}
