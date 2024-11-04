using UnityEngine;
using  UnityEngine.SceneManagement;
namespace ET
{
    public class GoogleLoginDataMgr
    {
        private string googleOrIosUid = "";
        private static GoogleLoginDataMgr _instance;
        public static GoogleLoginDataMgr GetInstance()
        {
            if (_instance == null)
                _instance = new GoogleLoginDataMgr();
            return _instance;
        }
        public GoogleLoginDataMgr()
        {
           
        }



        /// <summary>
        /// google是否已登录
        /// </summary>
        public string GoogleOrIosUid
        {
            get
            {
                return this.googleOrIosUid;
            }
            set
            {
                this.googleOrIosUid = value;
            }
        }
    }
}