using System;

namespace ET
{
    public class ConfigInit
    {
        public static void LoadOneConfig( Type configType)
        {
            ConfigComponent comp = Game.Scene.GetComponent<ConfigComponent>();
            Log.Console("configType:" + configType.Name);
            byte[] oneConfigBytes = comp.ConfigLoader.GetOneConfigBytes(configType.Name);

            object category = ProtobufHelper.FromBytes(configType, oneConfigBytes, 0, oneConfigBytes.Length);

            comp.AllConfig[configType] = category;
        }
    }
}