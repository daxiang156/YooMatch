using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class DropBagCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static DropBagCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static DropBagCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(DropBagCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DropBag> dict = new Dictionary<int, DropBag>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DropBag> list = new List<DropBag>();
		
        public DropBagCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (DropBag config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public DropBag Get(int id)
        {
            this.dict.TryGetValue(id, out DropBag item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (DropBag)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DropBag> GetAll()
        {
            return this.dict;
        }

        public DropBag GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DropBag: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Name { get; set; }
		[ProtoMember(3)]
		public string reward { get; set; }

	}
}
