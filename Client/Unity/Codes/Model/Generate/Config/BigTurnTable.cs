using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class BigTurnTableCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static BigTurnTableCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static BigTurnTableCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(BigTurnTableCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BigTurnTable> dict = new Dictionary<int, BigTurnTable>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BigTurnTable> list = new List<BigTurnTable>();
		
        public BigTurnTableCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (BigTurnTable config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BigTurnTable Get(int id)
        {
            this.dict.TryGetValue(id, out BigTurnTable item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (BigTurnTable)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BigTurnTable> GetAll()
        {
            return this.dict;
        }

        public BigTurnTable GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BigTurnTable: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string descEn { get; set; }
		[ProtoMember(3)]
		public int probability { get; set; }
		[ProtoMember(4)]
		public string reward { get; set; }

	}
}
