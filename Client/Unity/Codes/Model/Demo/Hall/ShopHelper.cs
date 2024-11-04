namespace ET
{
    public enum ShopType
    {
        defalt = 0,
        MbShop,
    }

    public class ShopHelper
    {
        
        private static ShopHelper _instance;

        public static ShopHelper GetInstance()
        {
            if (_instance == null)
                _instance = new ShopHelper();
            return _instance;
        }

        public ShopType openType = ShopType.defalt;
    }
}