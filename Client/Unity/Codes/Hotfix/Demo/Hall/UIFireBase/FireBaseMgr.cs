namespace ET
{
    public class FireBaseMgr
    {
        private static FireBaseMgr _instance;

        public static FireBaseMgr GetInstance()
        {
            if (_instance == null)
                _instance = new FireBaseMgr();
            return _instance;
        }

        public void SendFireBaseTokenToServer()
        {
            Log.Info("SendFireBaseTokenToServer::" + GameDataMgr.Instance.FireBaseToken);
            if (string.IsNullOrEmpty(GameDataMgr.Instance.FireBaseToken))
            {
                GameDataMgr.Instance.FireBaseToken = "fireBaseTokenTest";
            }

            C2M_SendFireBaseToken cmd = new C2M_SendFireBaseToken();
            cmd.fireBaseToken = GameDataMgr.Instance.FireBaseToken;
            HallHelper.gateSession.Call(cmd).Coroutine();
        }
    }
}