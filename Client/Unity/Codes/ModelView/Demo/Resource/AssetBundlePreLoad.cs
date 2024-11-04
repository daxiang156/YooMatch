using System.Collections.Generic;
using ET;

namespace Assets.Mono.DownLoad
{
    public class AssetBundlePreLoad
    {
        private static AssetBundlePreLoad _instance;

        public static AssetBundlePreLoad GetInstance()
        {
            if (_instance == null)
                _instance = new AssetBundlePreLoad();
            return _instance;
        }
    }
}