namespace ET
{
    public class MBMethod
    {
        private static MBMethod instance;
        public static MBMethod Instance
        {
            get
            {
                if (instance == null)
                    instance = new MBMethod();
                return instance;
            }
        }

        public static int MBEventFruitType(int addNum, int reNum)
        {
            return 100 + addNum * 10 + reNum;
        }
        
        public static bool IsClickFruit(int eventType)
        {
            return (eventType >= 100 && eventType < 200) || eventType == 1;
        }
        
        public static bool IsAttackFruit(int eventType)
        {
            return eventType > 100 && eventType < 200;
        }
        
        public static int GetClickFruitAdd(int eventType)
        {
            return eventType % 100 / 10;
        }
        
        public static int GetClickFruitRe(int eventType)
        {
            return eventType % 10;
        }
    }
}