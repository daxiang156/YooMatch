using System;
using System.Collections.Generic;

namespace ET.Codes.ModelView.Demo.Resource
{
    public class ConfigPreLoad
    {
        private static ConfigPreLoad _instance;

        public static ConfigPreLoad GetInstance()
        {
            if (_instance == null)
                _instance = new ConfigPreLoad();
            return _instance;
        }
        private List<Type> preList = new List<Type>()
        {
            typeof(LanguageConfigCategory),
            typeof(InitConfigCategory),
            //typeof(SkinConfigCategory),
        };
        public async ETTask PreAsyncLoadConfig()
        {
            for (int i = 0; i < preList.Count; i++)
            {
                await AsyncLoadConfig(preList[i]);
            }
        }
        public async ETTask AsyncLoadConfig(Type configType)
        {
            ConfigComponent comp = Game.Scene.GetComponent<ConfigComponent>();
            if(comp.AllConfig.ContainsKey(configType))
                return;
            await TimerComponent.Instance.WaitAsync(1);
            Log.Console("configType:" + configType.Name);
            byte[] oneConfigBytes = comp.ConfigLoader.GetOneConfigBytes(configType.Name);
            object category = ProtobufHelper.FromBytes(configType, oneConfigBytes, 0, oneConfigBytes.Length);
            comp.AllConfig[configType] = category;
        }
    }
}