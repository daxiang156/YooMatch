using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class AccountInfoComponent : Entity, IAwake,IDestroy
    {
        public string Token;
        public long AccountId;

        public int AccountSkinId = 0;

        /// <summary>
        /// long gate ip
        /// </summary>
        public string Address;

        public long GateID;

        /// <summary>
        /// 
        /// </summary>
        public string GateToken;

        /// <summary>
        /// gateID
        /// </summary>
        public string GateAdress;
        /// <summary>
        /// 账号名字
        /// </summary>
        public string AccountName;
        /// <summary>
        /// 负载均衡服务器的token
        /// </summary>
        public string realToken;

        /// <summary>
        /// 负载均衡服务器地址
        /// </summary>
        public string realAddress;

        public string account;
        /// <summary>
        /// 用户地区
        /// </summary>
        public string userCountry;
    }
}
