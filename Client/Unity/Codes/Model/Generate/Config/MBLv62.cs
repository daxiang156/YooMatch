using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class MBLv62Category : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static MBLv62Category _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static MBLv62Category Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(MBLv62Category));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MBLv62> dict = new Dictionary<int, MBLv62>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MBLv62> list = new List<MBLv62>();
		
        public MBLv62Category()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (MBLv62 config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MBLv62 Get(int id)
        {
            this.dict.TryGetValue(id, out MBLv62 item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (MBLv62)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MBLv62> GetAll()
        {
            return this.dict;
        }

        public MBLv62 GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MBLv62: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int Row { get; set; }
		[ProtoMember(3)]
		public int Num { get; set; }
		[ProtoMember(4)]
		public int RowOffset { get; set; }
		[ProtoMember(5)]
		public int NumOffset { get; set; }
		[ProtoMember(6)]
		public string ItemList { get; set; }

	}
}
