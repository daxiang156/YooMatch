using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ClientSceneConfigCategory : ProtoObject
    {
		[ProtoIgnore]
        [BsonIgnore]
		private static ClientSceneConfigCategory _instance = null;
		[ProtoIgnore]
        [BsonIgnore]
        public static ClientSceneConfigCategory Instance
		{
			get
			{
				if(_instance == null)
				{
					ConfigInit.LoadOneConfig(typeof(ClientSceneConfigCategory));
				}
				return _instance;
			}
		}
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ClientSceneConfig> dict = new Dictionary<int, ClientSceneConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ClientSceneConfig> list = new List<ClientSceneConfig>();
		
        public ClientSceneConfigCategory()
        {
            _instance = this;
        }
		
        public override void EndInit()
        {
            foreach (ClientSceneConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ClientSceneConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ClientSceneConfig item);

            if (item == null)
            {
                Console.WriteLine($"配置找不到，配置表名: {nameof (ClientSceneConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ClientSceneConfig> GetAll()
        {
            return this.dict;
        }

        public ClientSceneConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ClientSceneConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int Process { get; set; }
		[ProtoMember(3)]
		public int Zone { get; set; }
		[ProtoMember(4)]
		public string SceneType { get; set; }
		[ProtoMember(5)]
		public string Name { get; set; }
		[ProtoMember(6)]
		public int OuterPort { get; set; }

	}
}
