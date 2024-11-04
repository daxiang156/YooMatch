namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        /// <summary>
        /// need to select account skin
        /// </summary>
        public const int ERR_SelectAccountSkin = 1;

        public const int ERR_NetworkError = 20001;

        /// <summary>
        /// login account server fail
        /// </summary>
        public const int ERR_LoginError = 20002;    //连接失败

        /// <summary>
        /// login account or password error
        /// </summary>
        public const int ERR_LoginAccountOrPassWord = 20003;        //账号 或 密码错误

        /// <summary>
        /// login fail in black list
        /// </summary>
        public const int ERR_LoginFail_BlackList = 20004;       //登录账号在黑名单中
        /// <summary>
        /// repeat login
        /// </summary>
        public const int ERR_Login_Repeat = 20005;              //重复登录
        /// <summary>
        /// repeat create role
        /// </summary>
        public const int ERR_TokenError = 20006;              //令牌token错误
        /// <summary>
        /// repeat create role
        /// </summary>
        public const int Error_RoleNameIsNull = 20007;        //名字为空
        /// <summary>
        /// repeat create role
        /// </summary>
        public const int Error_RoleNameSame = 20008;        //游戏角色名相同
        /// <summary>
        /// repeat create role
        /// </summary>
        public const int Error_RequestRepeatedly = 20009;        //重复请求
        /// <summary>
        /// repeat login role
        /// </summary>
        public const int Error_RequestSceneTypeError = 20010;        //区服类型错误
        /// <summary>
        /// repeat login role
        /// </summary>
        public const int Error_StartConfigError = 20011;        //配置表错误
        /// <summary>
        /// repeat login role
        /// </summary>
        public const int Error_OtherAccountLogin = 20012;        //账号在其他地方登陆
        /// <summary>
        /// repeat login role
        /// </summary>
        public const int Error_SessionPlayerError = 20013;        //GateSession错误
        /// <summary>
        /// repeat login role
        /// </summary>
        public const int Error_NonePlayerError = 20014;        //Gate未找到玩家
        /// <summary>
        /// repeat login role
        /// </summary>
        public const int Error_SessionStateError = 20015;        //GateSession状态错误
        /// <summary>
        /// repeat login role
        /// </summary>
        public const int Error_EnterGameError = 20016;        //角色进入逻辑服错误
        /// <summary>
        /// repeat login role
        /// </summary>
        public const int Error_ReEnterGameError = 20017;        //二次登陆失败
        /// <summary>
        /// repeat login role
        /// </summary>
        public const int Error_ReEnterGameError2 = 20018;        //二次登陆失败2
        
        //-------------------------绑定邮箱 start --------------------------//
        public const int Error_Email_Format = 200200;                     // 邮箱格式错误
        public const int Error_Email_CodeTimeOut = 200201;              // 验证码过期
        public const int Error_Email_CodeError = 200202;                //验证码错误
        public const int Error_Email_EmailNotBind = 200203;             //邮件未绑定
        public const int Error_Email_BindOtherAccout = 200204;          //邮箱已绑定其它账号
        public const int Error_Email_haveBind = 200205;                 //当前账号已经绑定邮箱 
        //-------------------------绑定邮箱 end --------------------------//

        // 好友
        public const int Error_Friend_ComIsNull = 200400;                     // FriendComponent is null
        public const int Error_Friend_Param = 200401;                     // 参数错误
        public const int Error_Friend_UnitComNull = 200402;                     // unitComponent is null
        public const int Error_Friend_IsInFriendList = 200403;                     //已经在自己好友列表
        public const int Error_Friend_IsInFriendReqList = 200404;                     //已经在自己好友申请列表
        public const int Error_Friend_NotInFriendReqList = 200405;                     //不在自己好友申请列表
        public const int Error_Friend_NotFound = 200406;                     //没有搜索到玩家
        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常
    }
}