namespace ET
{
    public class VibrationHelper
    {
        private static VibrationHelper instance;

        public static VibrationHelper GetInstance()
        {
            if (instance == null)
                instance = new VibrationHelper();
            return instance;
        }
        /// <summary>
        /// 统一调次方法震屏
        /// </summary>
        public void SendVibration(float intensity = 1, float sharpness = 1,int vibrationType = -1,float duration = 0)
        {
            if (HallInfoComponent.Instance.VibrationState == 0)
            {
                return;
            }
            
            EventDispatcher.PostEvent(EventName.Vibration,this,intensity,sharpness,vibrationType,duration);
        }
    }
}