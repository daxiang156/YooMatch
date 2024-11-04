using System.IO;
using System.Collections.Generic;
using ET.Codes.Hotfix.GameLogic.Common;
using UnityEngine.UIElements;

namespace ET
{
    /// <summary>
    /// 登录模块的对外接口
    /// </summary>
    public class LoginModule
    {
        private static LoginModule instance;
        public static LoginModule Instance
        {
            get
            {
                if(instance == null) 
                {
                    instance = new LoginModule();
                }
                return instance;
            }
        }
        
        public LoginModule()
        {
        }

        /// <summary>
        /// 获取上次保存的服务器ID
        /// </summary>
        /// <returns></returns>
        public int CheckLastServerId()
        {
            if (FileSystem.Instance.IsExists("LastSerID.txt"))
            {
                string content = FileSystem.Instance.LoadText("LastSerID.txt");
                string[] strContent = content.Split('|');
                int outid = 0;
                int[] lastServerId = new int[2];
                for (int i = 0; i < strContent.Length; i++)
                {
                    if (int.TryParse(strContent[i], out outid))
                    {
                        lastServerId[i] = outid;
                    }
                }
                if (lastServerId[1] != 0)
                    return lastServerId[1];
                else return lastServerId[0];
            }
            return -1;
        }

        /// <summary>
        /// 处理登录返回
        /// </summary>
        /// <param name="result"></param>
        public void OnS2CLoginResult()
        {
        }
    }
}