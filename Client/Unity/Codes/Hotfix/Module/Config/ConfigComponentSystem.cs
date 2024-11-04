using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ET
{
	[ObjectSystem]
    public class ConfigAwakeSystem : AwakeSystem<ConfigComponent>
    {
        public override void Awake(ConfigComponent self)
        {
	        ConfigComponent.Instance = self;
        }
    }
    
    [ObjectSystem]
    public class ConfigDestroySystem : DestroySystem<ConfigComponent>
    {
	    public override void Destroy(ConfigComponent self)
	    {
		    ConfigComponent.Instance = null;
	    }
    }
    
    public static class ConfigComponentSystem
	{
		public static void LoadOneConfig(this ConfigComponent self, Type configType)
		{
			Log.Console("configType:" + configType.Name);
			byte[] oneConfigBytes = self.ConfigLoader.GetOneConfigBytes(configType.Name);

			object category = ProtobufHelper.FromBytes(configType, oneConfigBytes, 0, oneConfigBytes.Length);

			self.AllConfig[configType] = category;
		}
		
		public static Type GetTypeByName(this ConfigComponent self, string configName)
		{
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));
			Type type = null;
			foreach (var item in types)
			{
				if (item.Name == configName)
				{
					type = item;
				}
			}
			return type;
		}
		
		public static void LoadOneConfigByName(this ConfigComponent self, string configName)
		{
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));
			
			Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();
			self.ConfigLoader.GetAllConfigBytes(configBytes);

			foreach (Type type in types)
			{
				self.LoadOneInThread(type, configBytes);
			}
		}
		
		public static void Load(this ConfigComponent self)
		{
			self.AllConfig.Clear();
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));
			
			Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();
			self.ConfigLoader.GetAllConfigBytes(configBytes);

			foreach (Type type in types)
			{
				self.LoadOneInThread(type, configBytes);
			}
		}
		
		private static List<Type> unNeedvList = new List<Type>();
		private static string[] unNeedConfig =
		{
			"ActivityConfig", 
			"AIConfig",
			"ClientSceneConfig",
			"DropBag",
			"EntityConfig",
			"EnviromentConfig",
			"ExchangeSkin",
			"FuncConfig",
			"MapUnitConfig1",
			"StartMachineConfig",
			"StartSceneConfig",
			"UnitConfig",
			"StartProcessConfig",
			"StartZoneConfig",
			"GameResultConfig",
			// "ActivityGoodsConfig",
			// "RechargeConfig", 
			// "BigTurnTable",
		};
		public static void LoadNeedConfig(this ConfigComponent self)
		{
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));
			foreach (var item in types)
			{
				if (item.Name.StartsWith("MBLv"))
				{
					unNeedvList.Add(item);
				}
				else
				{
					for (int i = 0; i < unNeedConfig.Length; i++)
					{
						if (item.Name == unNeedConfig[i] + "Category")
						{
							unNeedvList.Add(item);
						}
					}
				}
			}

			for (int i = 0; i < unNeedvList.Count; i++)
			{
				foreach (var item in types)
				{
					if (item.Name == unNeedvList[i].Name)
					{
						types.Remove(item);
						break;
					}
				}
			}
			Log.Console("同步加载表：" + types.Count);

			Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();
			self.ConfigLoader.GetAllConfigBytes(configBytes);
			foreach (Type type in types)
			{
				//Log.Error("同步加载表：" + type.Name);
				self.LoadOneInThread(type, configBytes);
			}
		}

		public static HashSet<Type> GetConfigsOtherMBLv()
		{
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));
			foreach (var item in types)
			{
				if (item.Name.StartsWith("MBLv"))
					types.Remove(item);
			}
			return types;
		}
		
		public static async ETTask LoadMBLvAsync(this ConfigComponent self)
		{
			Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();
			self.ConfigLoader.GetAllConfigBytes(configBytes);

			Log.Console("动态加载表：" + unNeedvList.Count);
			using (ListComponent<Task> listTasks = ListComponent<Task>.Create())
			{
				foreach (Type type in unNeedvList)
				{
					Task task = Task.Run(() => self.LoadOneInThread(type, configBytes));
					listTasks.Add(task);
				}
				await Task.WhenAll(listTasks.ToArray());
			}
		}

		public static async ETTask LoadLeftAsync(this ConfigComponent self)
		{
			//self.AllConfig.Clear();
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));
			foreach (var item in self.AllConfig)
			{
				if (types.Contains(item.Key))
					types.Remove(item.Key);
			}
			
			Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();
			self.ConfigLoader.GetAllConfigBytes(configBytes);

			Log.Console("动态加载表：" + types.Count);
			using (ListComponent<Task> listTasks = ListComponent<Task>.Create())
			{
				foreach (Type type in types)
				{
					Task task = Task.Run(() => self.LoadOneInThread(type, configBytes));
					listTasks.Add(task);
				}

				await Task.WhenAll(listTasks.ToArray());
			}
		}
		
		public static async ETTask LoadAsync(this ConfigComponent self)
		{
			self.AllConfig.Clear();
			HashSet<Type> types = Game.EventSystem.GetTypes(typeof (ConfigAttribute));
			
			Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();
			self.ConfigLoader.GetAllConfigBytes(configBytes);

			Log.Console("动态加载表：" + types.Count);
			using (ListComponent<Task> listTasks = ListComponent<Task>.Create())
			{
				foreach (Type type in types)
				{
					Task task = Task.Run(() => self.LoadOneInThread(type, configBytes));
					listTasks.Add(task);
				}

				await Task.WhenAll(listTasks.ToArray());
			}
		}

		public static int loadNum = 0;
		private static void LoadOneInThread(this ConfigComponent self, Type configType, Dictionary<string, byte[]> configBytes)
		{
			byte[] oneConfigBytes = configBytes[configType.Name];

			object category = ProtobufHelper.FromBytes(configType, oneConfigBytes, 0, oneConfigBytes.Length);

			lock (self)
			{
				loadNum++;
				//Log.Console(loadNum + ":加载表：" + configType.Name);
				self.AllConfig[configType] = category;	
			}
		}
	}
}