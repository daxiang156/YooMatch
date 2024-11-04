using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(M2C_TestResponse))]
	[Message(OuterOpcode.C2M_TestRequest)]
	[ProtoContract]
	public partial class C2M_TestRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string request { get; set; }

	}

	[Message(OuterOpcode.M2C_TestResponse)]
	[ProtoContract]
	public partial class M2C_TestResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string response { get; set; }

	}

	[ResponseType(nameof(Actor_TransferResponse))]
	[Message(OuterOpcode.Actor_TransferRequest)]
	[ProtoContract]
	public partial class Actor_TransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.Actor_TransferResponse)]
	[ProtoContract]
	public partial class Actor_TransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_EnterMap))]
	[Message(OuterOpcode.C2G_EnterMap)]
	[ProtoContract]
	public partial class C2G_EnterMap: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int MapId { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterMap)]
	[ProtoContract]
	public partial class G2C_EnterMap: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

// 自己unitId
		[ProtoMember(4)]
		public long MyId { get; set; }

	}

	[Message(OuterOpcode.MoveInfo)]
	[ProtoContract]
	public partial class MoveInfo: Object
	{
		[ProtoMember(1)]
		public List<float> X = new List<float>();

		[ProtoMember(2)]
		public List<float> Y = new List<float>();

		[ProtoMember(3)]
		public List<float> Z = new List<float>();

		[ProtoMember(4)]
		public float A { get; set; }

		[ProtoMember(5)]
		public float B { get; set; }

		[ProtoMember(6)]
		public float C { get; set; }

		[ProtoMember(7)]
		public float W { get; set; }

		[ProtoMember(8)]
		public int TurnSpeed { get; set; }

	}

	[Message(OuterOpcode.UnitInfo)]
	[ProtoContract]
	public partial class UnitInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public int Type { get; set; }

		[ProtoMember(4)]
		public int CurSkinId { get; set; }

		[ProtoMember(5)]
		public List<int> Ks = new List<int>();

		[ProtoMember(6)]
		public List<long> Vs = new List<long>();

	}

	[Message(OuterOpcode.M2C_CreateUnits)]
	[ProtoContract]
	public partial class M2C_CreateUnits: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode.M2C_CreateMyUnit)]
	[ProtoContract]
	public partial class M2C_CreateMyUnit: Object, IActorMessage
	{
		[ProtoMember(1)]
		public UnitInfo Unit { get; set; }

	}

	[Message(OuterOpcode.M2C_StartSceneChange)]
	[ProtoContract]
	public partial class M2C_StartSceneChange: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long SceneInstanceId { get; set; }

		[ProtoMember(2)]
		public string SceneName { get; set; }

		[ProtoMember(3)]
		public int MapId { get; set; }

		[ProtoMember(4)]
		public long HouseId { get; set; }

		[ProtoMember(5)]
		public int RealPlayer { get; set; }

		[ProtoMember(6)]
		public int RandNum { get; set; }

	}

	[Message(OuterOpcode.M2C_RemoveUnits)]
	[ProtoContract]
	public partial class M2C_RemoveUnits: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<long> Units = new List<long>();

	}

	[Message(OuterOpcode.C2M_PathfindingResult)]
	[ProtoContract]
	public partial class C2M_PathfindingResult: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.C2M_Stop)]
	[ProtoContract]
	public partial class C2M_Stop: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	[ProtoContract]
	public partial class M2C_PathfindingResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

		[ProtoMember(5)]
		public List<float> Xs = new List<float>();

		[ProtoMember(6)]
		public List<float> Ys = new List<float>();

		[ProtoMember(7)]
		public List<float> Zs = new List<float>();

	}

	[Message(OuterOpcode.M2C_Stop)]
	[ProtoContract]
	public partial class M2C_Stop: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float A { get; set; }

		[ProtoMember(7)]
		public float B { get; set; }

		[ProtoMember(8)]
		public float C { get; set; }

		[ProtoMember(9)]
		public float W { get; set; }

	}

	[ResponseType(nameof(G2C_Ping))]
	[Message(OuterOpcode.C2G_Ping)]
	[ProtoContract]
	public partial class C2G_Ping: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Ping)]
	[ProtoContract]
	public partial class G2C_Ping: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Time { get; set; }

	}

	[Message(OuterOpcode.G2C_Test)]
	[ProtoContract]
	public partial class G2C_Test: Object, IMessage
	{
	}

	[ResponseType(nameof(M2C_Reload))]
	[Message(OuterOpcode.C2M_Reload)]
	[ProtoContract]
	public partial class C2M_Reload: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.M2C_Reload)]
	[ProtoContract]
	public partial class M2C_Reload: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_Login))]
	[Message(OuterOpcode.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Address { get; set; }

		[ProtoMember(2)]
		public long Key { get; set; }

		[ProtoMember(3)]
		public long GateId { get; set; }

	}

	[ResponseType(nameof(G2C_LoginGate))]
	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.G2C_TestHotfixMessage)]
	[ProtoContract]
	public partial class G2C_TestHotfixMessage: Object, IMessage
	{
		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[ResponseType(nameof(M2C_TestRobotCase))]
	[Message(OuterOpcode.C2M_TestRobotCase)]
	[ProtoContract]
	public partial class C2M_TestRobotCase: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[Message(OuterOpcode.M2C_TestRobotCase)]
	[ProtoContract]
	public partial class M2C_TestRobotCase: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[ResponseType(nameof(M2C_TransferMap))]
	[Message(OuterOpcode.C2M_TransferMap)]
	[ProtoContract]
	public partial class C2M_TransferMap: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TransferMap)]
	[ProtoContract]
	public partial class M2C_TransferMap: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_TransferMap))]
	[Message(OuterOpcode.C2G_HelloWorld)]
	[ProtoContract]
	public partial class C2G_HelloWorld: Object, IMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string str { get; set; }

	}

//登录账号服务器
	[ResponseType(nameof(A2C_LoginAccount))]
	[Message(OuterOpcode.C2A_LoginAccount)]
	[ProtoContract]
	public partial class C2A_LoginAccount: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string AccountName { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

		[ProtoMember(3)]
		public string DeviceId { get; set; }

		[ProtoMember(4)]
		public string AppsFlyerId { get; set; }

	}

//登录返回	AccountSkinId == 0 表示需要进行默认选角
	[Message(OuterOpcode.A2C_LoginAccount)]
	[ProtoContract]
	public partial class A2C_LoginAccount: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(2)]
		public long AccountId { get; set; }

		[ProtoMember(3)]
		public string Address { get; set; }

		[ProtoMember(4)]
		public long GateId { get; set; }

		[ProtoMember(5)]
		public int AccountSkinId { get; set; }

		[ProtoMember(6)]
		public string UserName { get; set; }

		[ProtoMember(7)]
		public string UserIp { get; set; }

		[ProtoMember(8)]
		public long DeleteTime { get; set; }

		[ProtoMember(9)]
		public string LastDeviceId { get; set; }

	}

//第一次登录账号需要选定默认皮肤
	[ResponseType(nameof(A2C_SelectAccountSkin))]
	[Message(OuterOpcode.C2A_SelectAccountSkin)]
	[ProtoContract]
	public partial class C2A_SelectAccountSkin: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int AccountSkinId { get; set; }

		[ProtoMember(2)]
		public long AccountId { get; set; }

		[ProtoMember(3)]
		public string name { get; set; }

	}

//选定皮肤后在连接网关
	[Message(OuterOpcode.A2C_SelectAccountSkin)]
	[ProtoContract]
	public partial class A2C_SelectAccountSkin: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public int AccountSkinId { get; set; }

		[ProtoMember(3)]
		public string name { get; set; }

	}

//被其他玩家挤下线
	[Message(OuterOpcode.A2C_Disconnect)]
	[ProtoContract]
	public partial class A2C_Disconnect: Object, IMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

	}

// message ServerInfoProto
// {
// 	int32 Id = 1;
// 	int32 Status = 2;
// 	string ServerName = 3;
// }
// //获取服务器列表
// //ResponseType A2C_GetServerInfos
// message C2A_GetServerInfos //IRequest
// {
// 	int32 RpcId = 90;
// 	string Token = 1;
// 	int64 AccountId = 2;
// }
// message A2C_GetServerInfos	//IResponse
// {
// 	int32 RpcId = 90;
// 	int32 Error = 91;
// 	string message = 92;
// 	repeated ServerInfoProto ServerInfoList = 1;
// }
	[Message(OuterOpcode.RoleInfoProto)]
	[ProtoContract]
	public partial class RoleInfoProto: Object
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public string Name { get; set; }

		[ProtoMember(3)]
		public int State { get; set; }

		[ProtoMember(4)]
		public long AccountId { get; set; }

		[ProtoMember(5)]
		public long LastLoginTime { get; set; }

		[ProtoMember(6)]
		public long CreateTime { get; set; }

		[ProtoMember(7)]
		public int AvatarId { get; set; }

	}

//请求创建角色
	[ResponseType(nameof(A2C_CreateRole))]
	[Message(OuterOpcode.C2A_CreateRole)]
	[ProtoContract]
	public partial class C2A_CreateRole: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(2)]
		public long AccountId { get; set; }

		[ProtoMember(3)]
		public string Name { get; set; }

		[ProtoMember(4)]
		public int AvatarId { get; set; }

	}

	[Message(OuterOpcode.A2C_CreateRole)]
	[ProtoContract]
	public partial class A2C_CreateRole: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(93)]
		public RoleInfoProto RoleInfo { get; set; }

	}

//请求负载均衡服务器信息
	[ResponseType(nameof(A2C_GetRealmKey))]
	[Message(OuterOpcode.C2A_GetRealmKey)]
	[ProtoContract]
	public partial class C2A_GetRealmKey: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(2)]
		public int ServerId { get; set; }

		[ProtoMember(3)]
		public long AccountId { get; set; }

	}

	[Message(OuterOpcode.A2C_GetRealmKey)]
	[ProtoContract]
	public partial class A2C_GetRealmKey: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string RealmKey { get; set; }

		[ProtoMember(2)]
		public string RealmAddress { get; set; }

	}

//登陆负载均衡服务器
	[ResponseType(nameof(R2C_LoginRealm))]
	[Message(OuterOpcode.C2R_LoginRealm)]
	[ProtoContract]
	public partial class C2R_LoginRealm: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string RealmTokenKey { get; set; }

	}

	[Message(OuterOpcode.R2C_LoginRealm)]
	[ProtoContract]
	public partial class R2C_LoginRealm: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string GateSessionKey { get; set; }

		[ProtoMember(2)]
		public string GateAddress { get; set; }

	}

//登陆网关服务器
	[ResponseType(nameof(G2C_LoginGameGate))]
	[Message(OuterOpcode.C2G_LoginGameGate)]
	[ProtoContract]
	public partial class C2G_LoginGameGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Key { get; set; }

		[ProtoMember(2)]
		public long RoleId { get; set; }

		[ProtoMember(3)]
		public long AccountId { get; set; }

		[ProtoMember(4)]
		public int AccountSkinId { get; set; }

		[ProtoMember(5)]
		public string Name { get; set; }

		[ProtoMember(6)]
		public string Country { get; set; }

		[ProtoMember(7)]
		public string Region { get; set; }

		[ProtoMember(8)]
		public string DeviceId { get; set; }

		[ProtoMember(9)]
		public string AppsFlyerId { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGameGate)]
	[ProtoContract]
	public partial class G2C_LoginGameGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

//请求进入游戏逻辑服
	[ResponseType(nameof(G2C_EnterGame))]
	[Message(OuterOpcode.C2G_EnterGame)]
	[ProtoContract]
	public partial class C2G_EnterGame: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long HouseId { get; set; }

		[ProtoMember(2)]
		public int IsReLogin { get; set; }

		[ProtoMember(3)]
		public string ClientVersion { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterGame)]
	[ProtoContract]
	public partial class G2C_EnterGame: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long MyId { get; set; }

		[ProtoMember(2)]
		public long HouseId { get; set; }

		[ProtoMember(3)]
		public string Country { get; set; }

		[ProtoMember(4)]
		public string Region { get; set; }

		[ProtoMember(5)]
		public int GuideStep { get; set; }

	}

	[Message(OuterOpcode.M2C_RoleInfo)]
	[ProtoContract]
	public partial class M2C_RoleInfo: Object, IActorMessage
	{
	}

//请求进入房间
	[ResponseType(nameof(M2C_EnterHouse))]
	[Message(OuterOpcode.C2M_EnterHouse)]
	[ProtoContract]
	public partial class C2M_EnterHouse: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_EnterHouse)]
	[ProtoContract]
	public partial class M2C_EnterHouse: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long MyId { get; set; }

	}

	[Message(OuterOpcode.M2C_NoticeUnitNumeric)]
	[ProtoContract]
	public partial class M2C_NoticeUnitNumeric: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int NumericType { get; set; }

		[ProtoMember(4)]
		public long newValue { get; set; }

	}

//皮肤兑换上行
	[ResponseType(nameof(M2C_EXCHANGE_SKIN))]
	[Message(OuterOpcode.C2M_EXCHANGE_SKIN)]
	[ProtoContract]
	public partial class C2M_EXCHANGE_SKIN: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int skinId { get; set; }

	}

//皮肤兑换下行
	[Message(OuterOpcode.M2C_EXCHANGE_SKIN)]
	[ProtoContract]
	public partial class M2C_EXCHANGE_SKIN: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SkinId { get; set; }

	}

	[Message(OuterOpcode.ItemInfo)]
	[ProtoContract]
	public partial class ItemInfo: Object
	{
		[ProtoMember(1)]
		public int ItemId { get; set; }

		[ProtoMember(2)]
		public long ItemUId { get; set; }

		[ProtoMember(3)]
		public long ItemNum { get; set; }

		[ProtoMember(4)]
		public long endTime { get; set; }

	}

//游戏中客户端拉取背包数据上行
	[ResponseType(nameof(M2C_GetBag_List))]
	[Message(OuterOpcode.C2M_GetBag_List)]
	[ProtoContract]
	public partial class C2M_GetBag_List: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

//玩家上线同步背包数据
	[Message(OuterOpcode.M2C_GetBag_List)]
	[ProtoContract]
	public partial class M2C_GetBag_List: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<ItemInfo> ItemList = new List<ItemInfo>();

	}

//客户端同步道具数量上行
	[ResponseType(nameof(M2C_SynItem))]
	[Message(OuterOpcode.C2M_SynItem)]
	[ProtoContract]
	public partial class C2M_SynItem: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<ItemInfo> ItemList = new List<ItemInfo>();

		[ProtoMember(2)]
		public List<ItemInfo> ResourceList = new List<ItemInfo>();

	}

//客户端同步道具数量下行
	[Message(OuterOpcode.M2C_SynItem)]
	[ProtoContract]
	public partial class M2C_SynItem: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<ItemInfo> ItemList = new List<ItemInfo>();

	}

	[ResponseType(nameof(M2C_EnterMap))]
	[Message(OuterOpcode.C2M_EnterMap)]
	[ProtoContract]
	public partial class C2M_EnterMap: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int MapId { get; set; }

	}

	[Message(OuterOpcode.M2C_EnterMap)]
	[ProtoContract]
	public partial class M2C_EnterMap: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

// 自己unitId
		[ProtoMember(4)]
		public long MyId { get; set; }

	}

	[Message(OuterOpcode.AchievementItemInfo)]
	[ProtoContract]
	public partial class AchievementItemInfo: Object
	{
		[ProtoMember(1)]
		public int AchId { get; set; }

		[ProtoMember(2)]
		public byte state { get; set; }

	}

//获取成就信息上行
	[ResponseType(nameof(M2C_GetAchievementInfo))]
	[Message(OuterOpcode.C2M_GetAchievementInfo)]
	[ProtoContract]
	public partial class C2M_GetAchievementInfo: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

//获取成就信息下行
	[Message(OuterOpcode.M2C_GetAchievementInfo)]
	[ProtoContract]
	public partial class M2C_GetAchievementInfo: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<AchievementItemInfo> AchList = new List<AchievementItemInfo>();

		[ProtoMember(2)]
		public long curProgress { get; set; }

		[ProtoMember(3)]
		public int curId { get; set; }

	}

//领取成就奖励上行
	[ResponseType(nameof(M2C_GetAchievementReward))]
	[Message(OuterOpcode.C2M_GetAchievementReward)]
	[ProtoContract]
	public partial class C2M_GetAchievementReward: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int AchId { get; set; }

		[ProtoMember(2)]
		public bool offline { get; set; }

	}

//领取成就奖励下行		失败不返回
	[Message(OuterOpcode.M2C_GetAchievementReward)]
	[ProtoContract]
	public partial class M2C_GetAchievementReward: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int AchId { get; set; }

		[ProtoMember(2)]
		public List<AchievevmentReward> rewardList = new List<AchievevmentReward>();

	}

	[Message(OuterOpcode.AchievevmentReward)]
	[ProtoContract]
	public partial class AchievevmentReward: Object
	{
		[ProtoMember(1)]
		public int itemId { get; set; }

		[ProtoMember(2)]
		public int itemNum { get; set; }

		[ProtoMember(3)]
		public int itemType { get; set; }

	}

	[ResponseType(nameof(M2C_ChangeCurSkin))]
	[Message(OuterOpcode.C2M_ChangeCurSkin)]
	[ProtoContract]
	public partial class C2M_ChangeCurSkin: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int SkinId { get; set; }

	}

	[Message(OuterOpcode.M2C_ChangeCurSkin)]
	[ProtoContract]
	public partial class M2C_ChangeCurSkin: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SkinId { get; set; }

	}

	[Message(OuterOpcode.M2C_NoticeChangeSkin)]
	[ProtoContract]
	public partial class M2C_NoticeChangeSkin: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int SkinId { get; set; }

	}

	[ResponseType(nameof(M2C_ChangeSkinAppearance))]
	[Message(OuterOpcode.C2M_ChangeSkinAppearance)]
	[ProtoContract]
	public partial class C2M_ChangeSkinAppearance: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkinId { get; set; }

		[ProtoMember(2)]
		public int AppearanceId { get; set; }

	}

	[Message(OuterOpcode.M2C_ChangeSkinAppearance)]
	[ProtoContract]
	public partial class M2C_ChangeSkinAppearance: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SkinId { get; set; }

		[ProtoMember(2)]
		public int AppearanceId { get; set; }

	}

	[Message(OuterOpcode.M2C_NoticeChangeSkinAppearance)]
	[ProtoContract]
	public partial class M2C_NoticeChangeSkinAppearance: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int SkinId { get; set; }

		[ProtoMember(3)]
		public int AppearanceId { get; set; }

	}

	[Message(OuterOpcode.SkinInfo)]
	[ProtoContract]
	public partial class SkinInfo: Object
	{
		[ProtoMember(1)]
		public int SkinId { get; set; }

		[ProtoMember(2)]
		public int CurAppearanceId { get; set; }

		[ProtoMember(3)]
		public List<int> AppearanceList = new List<int>();

		[ProtoMember(4)]
		public List<long> WearList = new List<long>();

	}

//请求玩家皮肤数据
	[Message(OuterOpcode.C2M_SkinInfo_List)]
	[ProtoContract]
	public partial class C2M_SkinInfo_List: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

//同步玩家皮肤数据
	[Message(OuterOpcode.M2C_SkinInfo_List)]
	[ProtoContract]
	public partial class M2C_SkinInfo_List: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<SkinInfo> SkinList = new List<SkinInfo>();

		[ProtoMember(2)]
		public int newSkinId { get; set; }

	}

//同步其他玩家皮肤数据
	[Message(OuterOpcode.M2C_OtherSkinInfoList)]
	[ProtoContract]
	public partial class M2C_OtherSkinInfoList: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int SkinId { get; set; }

		[ProtoMember(3)]
		public int CurAppearanceId { get; set; }

		[ProtoMember(4)]
		public List<int> WearList = new List<int>();

	}

	[ResponseType(nameof(M2C_ChangeSkinWear))]
	[Message(OuterOpcode.C2M_ChangeSkinWear)]
	[ProtoContract]
	public partial class C2M_ChangeSkinWear: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkinId { get; set; }

		[ProtoMember(2)]
		public int WearId { get; set; }

		[ProtoMember(3)]
		public long ItemUId { get; set; }

	}

	[Message(OuterOpcode.M2C_ChangeSkinWear)]
	[ProtoContract]
	public partial class M2C_ChangeSkinWear: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SkinId { get; set; }

		[ProtoMember(2)]
		public int WearId { get; set; }

		[ProtoMember(3)]
		public long ItemUId { get; set; }

	}

	[Message(OuterOpcode.M2C_NoticeSkinWear)]
	[ProtoContract]
	public partial class M2C_NoticeSkinWear: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int SkinId { get; set; }

		[ProtoMember(3)]
		public int WearId { get; set; }

		[ProtoMember(4)]
		public long ItemConfigId { get; set; }

	}

	[Message(OuterOpcode.FuncInfo)]
	[ProtoContract]
	public partial class FuncInfo: Object
	{
		[ProtoMember(1)]
		public int funcId { get; set; }

		[ProtoMember(2)]
		public byte state { get; set; }

	}

//玩家上线主动同步功能开启列表
	[Message(OuterOpcode.M2C_SynFuncList)]
	[ProtoContract]
	public partial class M2C_SynFuncList: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<FuncInfo> FuncList = new List<FuncInfo>();

	}

//主动同步新功能开启
	[Message(OuterOpcode.M2C_SynFuncOpen)]
	[ProtoContract]
	public partial class M2C_SynFuncOpen: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<FuncInfo> FuncList = new List<FuncInfo>();

	}

	[Message(OuterOpcode.RedPoint)]
	[ProtoContract]
	public partial class RedPoint: Object
	{
		[ProtoMember(1)]
		public int RedId { get; set; }

		[ProtoMember(2)]
		public byte state { get; set; }

	}

//上线主动推送红点逻辑
	[Message(OuterOpcode.M2C_SynRedPoint)]
	[ProtoContract]
	public partial class M2C_SynRedPoint: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RedPoint> RedList = new List<RedPoint>();

	}

//同步单个红点状态
	[Message(OuterOpcode.M2C_SynSingleRedPoint)]
	[ProtoContract]
	public partial class M2C_SynSingleRedPoint: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RedPoint> RedList = new List<RedPoint>();

	}

//请求加入匹配
	[ResponseType(nameof(M2C_JoinMatch))]
	[Message(OuterOpcode.C2M_JoinMatch)]
	[ProtoContract]
	public partial class C2M_JoinMatch: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MpaId { get; set; }

	}

	[Message(OuterOpcode.M2C_JoinMatch)]
	[ProtoContract]
	public partial class M2C_JoinMatch: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long MatchId { get; set; }

		[ProtoMember(2)]
		public int RandNum { get; set; }

	}

//请求取消匹配
	[ResponseType(nameof(M2C_CancelMatch))]
	[Message(OuterOpcode.C2M_CancelMatch)]
	[ProtoContract]
	public partial class C2M_CancelMatch: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MpaId { get; set; }

	}

	[Message(OuterOpcode.M2C_CancelMatch)]
	[ProtoContract]
	public partial class M2C_CancelMatch: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//同步匹配信息
	[Message(OuterOpcode.M2C_SyncMathcInfo)]
	[ProtoContract]
	public partial class M2C_SyncMathcInfo: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int MatchNum { get; set; }

		[ProtoMember(2)]
		public string playerName { get; set; }

		[ProtoMember(3)]
		public int curSkinId { get; set; }

		[ProtoMember(4)]
		public int pvpLevelStar { get; set; }

	}

	[Message(OuterOpcode.HouseObjInfo)]
	[ProtoContract]
	public partial class HouseObjInfo: Object
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public int CampId { get; set; }

		[ProtoMember(3)]
		public int PosId { get; set; }

		[ProtoMember(4)]
		public bool isRobot { get; set; }

		[ProtoMember(5)]
		public string playerName { get; set; }

		[ProtoMember(6)]
		public int CurSkindId { get; set; }

		[ProtoMember(7)]
		public int pvpLevelStar { get; set; }

	}

	[Message(OuterOpcode.M2C_SyncHouseStart)]
	[ProtoContract]
	public partial class M2C_SyncHouseStart: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int MatchNum { get; set; }

		[ProtoMember(2)]
		public long HouseId { get; set; }

		[ProtoMember(3)]
		public List<HouseObjInfo> ObjInfo = new List<HouseObjInfo>();

	}

	[Message(OuterOpcode.M2C_SyncHouseObjInfo)]
	[ProtoContract]
	public partial class M2C_SyncHouseObjInfo: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int MatchNum { get; set; }

		[ProtoMember(2)]
		public long HouseId { get; set; }

		[ProtoMember(3)]
		public List<HouseObjInfo> ObjInfo = new List<HouseObjInfo>();

	}

	[Message(OuterOpcode.C2M_CreatHouseRobotSucceed)]
	[ProtoContract]
	public partial class C2M_CreatHouseRobotSucceed: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

//副本房间已经准备好，通知客户端开始
	[Message(OuterOpcode.M2C_HouseStart)]
	[ProtoContract]
	public partial class M2C_HouseStart: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long HouseId { get; set; }

		[ProtoMember(2)]
		public long StartTime { get; set; }

	}

	[Message(OuterOpcode.M2C_SyncEnterCity)]
	[ProtoContract]
	public partial class M2C_SyncEnterCity: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long CityId { get; set; }

	}

//玩家退出玩法房间
	[ResponseType(nameof(M2C_ExitPlayHouse))]
	[Message(OuterOpcode.C2M_ExitPlayHouse)]
	[ProtoContract]
	public partial class C2M_ExitPlayHouse: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_ExitPlayHouse)]
	[ProtoContract]
	public partial class M2C_ExitPlayHouse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long CityId { get; set; }

	}

//玩家上线主动同步成就信息
	[Message(OuterOpcode.M2C_SynAchievementList)]
	[ProtoContract]
	public partial class M2C_SynAchievementList: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<AchievementItemInfo> AchList = new List<AchievementItemInfo>();

		[ProtoMember(2)]
		public long curProgress { get; set; }

		[ProtoMember(3)]
		public int curId { get; set; }

	}

	[ResponseType(nameof(M2C_GameResult))]
	[Message(OuterOpcode.C2M_GameResult)]
	[ProtoContract]
	public partial class C2M_GameResult: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public List<RankInfo> RankInfo = new List<RankInfo>();

		[ProtoMember(3)]
		public List<Reward> RewardList = new List<Reward>();

		[ProtoMember(4)]
		public int resultState { get; set; }

		[ProtoMember(5)]
		public int funyGameId { get; set; }

		[ProtoMember(6)]
		public int funyGameProgress { get; set; }

		[ProtoMember(7)]
		public bool offline { get; set; }

		[ProtoMember(8)]
		public List<FunyLevelEvent> levelEvent = new List<FunyLevelEvent>();

	}

// 消消乐关卡事件
	[Message(OuterOpcode.FunyLevelEvent)]
	[ProtoContract]
	public partial class FunyLevelEvent: Object
	{
		[ProtoMember(1)]
		public int event_id { get; set; }

		[ProtoMember(2)]
		public int event_value { get; set; }

	}

	[Message(OuterOpcode.RankInfo)]
	[ProtoContract]
	public partial class RankInfo: Object
	{
		[ProtoMember(1)]
		public long roleId { get; set; }

		[ProtoMember(2)]
		public byte rank { get; set; }

	}

//竞速结算
	[Message(OuterOpcode.M2C_GameResult)]
	[ProtoContract]
	public partial class M2C_GameResult: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public byte Rank { get; set; }

		[ProtoMember(3)]
		public int level { get; set; }

		[ProtoMember(4)]
		public int EXP { get; set; }

		[ProtoMember(5)]
		public List<Reward> RewardList = new List<Reward>();

		[ProtoMember(6)]
		public int funyGameId { get; set; }

		[ProtoMember(7)]
		public int passNum { get; set; }

	}

	[Message(OuterOpcode.Reward)]
	[ProtoContract]
	public partial class Reward: Object
	{
		[ProtoMember(1)]
		public int itemId { get; set; }

		[ProtoMember(2)]
		public int itemNum { get; set; }

	}

//加道具
	[ResponseType(nameof(M2C_GM_AddItem))]
	[Message(OuterOpcode.C2M_GM_AddItem)]
	[ProtoContract]
	public partial class C2M_GM_AddItem: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string itemStr { get; set; }

		[ProtoMember(3)]
		public bool offline { get; set; }

	}

	[Message(OuterOpcode.M2C_GM_AddItem)]
	[ProtoContract]
	public partial class M2C_GM_AddItem: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public bool isSuc { get; set; }

	}

//请求地图数据上行
	[ResponseType(nameof(M2C_GetMapList))]
	[Message(OuterOpcode.C2M_GetMapList)]
	[ProtoContract]
	public partial class C2M_GetMapList: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_GetMapList)]
	[ProtoContract]
	public partial class M2C_GetMapList: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int curMapType { get; set; }

		[ProtoMember(2)]
		public List<int> mapList = new List<int>();

	}

//解锁地图上行
	[ResponseType(nameof(M2C_UnLockMap))]
	[Message(OuterOpcode.C2M_UnLockMap)]
	[ProtoContract]
	public partial class C2M_UnLockMap: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int mapType { get; set; }

	}

//解锁地图下行
	[Message(OuterOpcode.M2C_UnLockMap)]
	[ProtoContract]
	public partial class M2C_UnLockMap: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int mapType { get; set; }

	}

//------------------------ 活动 start --------------------------
//单个活动商品数据
	[Message(OuterOpcode.ActivityGoods)]
	[ProtoContract]
	public partial class ActivityGoods: Object
	{
		[ProtoMember(1)]
		public int GoodsId { get; set; }

		[ProtoMember(2)]
		public byte Type { get; set; }

		[ProtoMember(3)]
		public int Num { get; set; }

		[ProtoMember(4)]
		public byte Sale { get; set; }

		[ProtoMember(5)]
		public int BuyTimes { get; set; }

		[ProtoMember(6)]
		public List<int> CostId = new List<int>();

		[ProtoMember(7)]
		public List<int> CostNum = new List<int>();

		[ProtoMember(8)]
		public int ItemId { get; set; }

		[ProtoMember(9)]
		public int itemNum { get; set; }

		[ProtoMember(10)]
		public string Icon { get; set; }

	}

//单个活动数据
	[Message(OuterOpcode.ActivityItem)]
	[ProtoContract]
	public partial class ActivityItem: Object
	{
		[ProtoMember(1)]
		public int ActId { get; set; }

		[ProtoMember(2)]
		public long StartTime { get; set; }

		[ProtoMember(3)]
		public long EndTime { get; set; }

	}

//上线同步活动列表
	[Message(OuterOpcode.M2C_SynActivityList)]
	[ProtoContract]
	public partial class M2C_SynActivityList: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<ActivityItem> ActList = new List<ActivityItem>();

	}

//请求活动数据上行
	[ResponseType(nameof(M2C_GetActivityDetail))]
	[Message(OuterOpcode.C2M_GetActivityDetail)]
	[ProtoContract]
	public partial class C2M_GetActivityDetail: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int ActId { get; set; }

	}

//请求活动数据下行
	[Message(OuterOpcode.M2C_GetActivityDetail)]
	[ProtoContract]
	public partial class M2C_GetActivityDetail: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int ActId { get; set; }

		[ProtoMember(2)]
		public List<ActivityGoods> GoodsList = new List<ActivityGoods>();

		[ProtoMember(3)]
		public long StartTime { get; set; }

		[ProtoMember(4)]
		public long EndTime { get; set; }

	}

//购买商品上行
	[ResponseType(nameof(M2C_BuyGoods))]
	[Message(OuterOpcode.C2M_BuyGoods)]
	[ProtoContract]
	public partial class C2M_BuyGoods: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ActId { get; set; }

		[ProtoMember(2)]
		public int GoodsId { get; set; }

		[ProtoMember(3)]
		public bool offline { get; set; }

	}

//购买商品下行
	[Message(OuterOpcode.M2C_BuyGoods)]
	[ProtoContract]
	public partial class M2C_BuyGoods: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int ActId { get; set; }

		[ProtoMember(2)]
		public int GoodsId { get; set; }

		[ProtoMember(3)]
		public int BuyTimes { get; set; }

	}

//------------------------ 活动 end --------------------------
//------------------------ 金币拾取 start --------------------
	[ResponseType(nameof(M2C_PickUpGoldInfo))]
	[Message(OuterOpcode.C2M_PickUpGoldInfo)]
	[ProtoContract]
	public partial class C2M_PickUpGoldInfo: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

	}

	[Message(OuterOpcode.M2C_PickUpGoldInfo)]
	[ProtoContract]
	public partial class M2C_PickUpGoldInfo: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public List<GoldInfo> GoldList = new List<GoldInfo>();

	}

	[Message(OuterOpcode.GoldInfo)]
	[ProtoContract]
	public partial class GoldInfo: Object
	{
		[ProtoMember(1)]
		public byte GoldIndex { get; set; }

	}

	[ResponseType(nameof(M2C_PickUpGold))]
	[Message(OuterOpcode.C2M_PickUpGold)]
	[ProtoContract]
	public partial class C2M_PickUpGold: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public byte GoldIndex { get; set; }

		[ProtoMember(2)]
		public long GoldNum { get; set; }

		[ProtoMember(3)]
		public int MapId { get; set; }

	}

	[Message(OuterOpcode.M2C_PickUpGold)]
	[ProtoContract]
	public partial class M2C_PickUpGold: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public byte GoldIndex { get; set; }

		[ProtoMember(3)]
		public bool Result { get; set; }

		[ProtoMember(4)]
		public long GoldNum { get; set; }

	}

//------------------------ 金币拾取 end --------------------------
//------------------------ 人物同步 start --------------------
	[Message(OuterOpcode.C2M_SyncPlayer)]
	[ProtoContract]
	public partial class C2M_SyncPlayer: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public SyncPlayerInfo PlayerInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_SyncPlayer)]
	[ProtoContract]
	public partial class M2C_SyncPlayer: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public SyncPlayerInfo player { get; set; }

	}

	[Message(OuterOpcode.SyncPlayerInfo)]
	[ProtoContract]
	public partial class SyncPlayerInfo: Object
	{
		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public MyVector3 Pos { get; set; }

		[ProtoMember(3)]
		public MyVector3 Rot { get; set; }

		[ProtoMember(4)]
		public string AnimatorName { get; set; }

	}

	[Message(OuterOpcode.MyVector3)]
	[ProtoContract]
	public partial class MyVector3: Object
	{
		[ProtoMember(1)]
		public float x { get; set; }

		[ProtoMember(2)]
		public float y { get; set; }

		[ProtoMember(3)]
		public float z { get; set; }

	}

	[Message(OuterOpcode.BILog)]
	[ProtoContract]
	public partial class BILog: Object
	{
		[ProtoMember(1)]
		public long user_id { get; set; }

		[ProtoMember(2)]
		public string user_name { get; set; }

		[ProtoMember(3)]
		public string country { get; set; }

		[ProtoMember(4)]
		public int event_id { get; set; }

		[ProtoMember(5)]
		public string param1 { get; set; }

		[ProtoMember(6)]
		public string param2 { get; set; }

		[ProtoMember(7)]
		public string param3 { get; set; }

		[ProtoMember(8)]
		public string param4 { get; set; }

		[ProtoMember(9)]
		public string param5 { get; set; }

		[ProtoMember(10)]
		public string param6 { get; set; }

	}

	[Message(OuterOpcode.C2B_SyncBILog)]
	[ProtoContract]
	public partial class C2B_SyncBILog: Object, IActorBILogMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public BILog info { get; set; }

	}

	[ResponseType(nameof(M2C_Get_PhotoneKey))]
	[Message(OuterOpcode.C2M_Get_PhotoneKey)]
	[ProtoContract]
	public partial class C2M_Get_PhotoneKey: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_Get_PhotoneKey)]
	[ProtoContract]
	public partial class M2C_Get_PhotoneKey: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string randomKey { get; set; }

		[ProtoMember(2)]
		public string key { get; set; }

	}

	[Message(OuterOpcode.EventInfo)]
	[ProtoContract]
	public partial class EventInfo: Object
	{
		[ProtoMember(1)]
		public long id { get; set; }

		[ProtoMember(2)]
		public long event_type { get; set; }

		[ProtoMember(3)]
		public long event_value { get; set; }

	}

	[Message(OuterOpcode.C2M_Sync_HouseEvent)]
	[ProtoContract]
	public partial class C2M_Sync_HouseEvent: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public EventInfo event_info { get; set; }

	}

	[Message(OuterOpcode.M2C_Sync_HouseEvent)]
	[ProtoContract]
	public partial class M2C_Sync_HouseEvent: Object, IActorMessage
	{
		[ProtoMember(1)]
		public EventInfo event_info { get; set; }

	}

	[Message(OuterOpcode.ScoreInfo)]
	[ProtoContract]
	public partial class ScoreInfo: Object
	{
		[ProtoMember(1)]
		public int CampId { get; set; }

		[ProtoMember(2)]
		public int Score { get; set; }

	}

	[Message(OuterOpcode.M2C_Sync_HouseScoreInfo)]
	[ProtoContract]
	public partial class M2C_Sync_HouseScoreInfo: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<ScoreInfo> score_info = new List<ScoreInfo>();

	}

//---------------------    任务    start -------//
	[Message(OuterOpcode.TaskStruct)]
	[ProtoContract]
	public partial class TaskStruct: Object
	{
		[ProtoMember(1)]
		public int TaskId { get; set; }

		[ProtoMember(2)]
		public int Progress { get; set; }

		[ProtoMember(3)]
		public int Total { get; set; }

		[ProtoMember(4)]
		public int State { get; set; }

		[ProtoMember(5)]
		public long LastResetTime { get; set; }

		[ProtoMember(6)]
		public int IsTag { get; set; }

	}

//上线主动推送任务列表
	[Message(OuterOpcode.M2C_SynTaskList)]
	[ProtoContract]
	public partial class M2C_SynTaskList: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<TaskStruct> TaskList = new List<TaskStruct>();

	}

//主动刷新任务状态
	[Message(OuterOpcode.M2C_SynTaskState)]
	[ProtoContract]
	public partial class M2C_SynTaskState: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public TaskStruct Task { get; set; }

	}

	[ResponseType(nameof(M2C_GetTaskReward))]
	[Message(OuterOpcode.C2M_GetTaskReward)]
	[ProtoContract]
	public partial class C2M_GetTaskReward: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TaskId { get; set; }

		[ProtoMember(2)]
		public bool offline { get; set; }

	}

	[Message(OuterOpcode.M2C_GetTaskReward)]
	[ProtoContract]
	public partial class M2C_GetTaskReward: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int TaskId { get; set; }

		[ProtoMember(2)]
		public bool suc { get; set; }

	}

//---------------------   任务    end -------//
//---------------------   消消乐玩法  start -------//
//进入消消乐
	[ResponseType(nameof(M2C_EnterFunGame))]
	[Message(OuterOpcode.C2M_EnterFunGame)]
	[ProtoContract]
	public partial class C2M_EnterFunGame: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long funyGameId { get; set; }

		[ProtoMember(2)]
		public bool offline { get; set; }

		[ProtoMember(3)]
		public string level_difficulty { get; set; }

		[ProtoMember(4)]
		public string free_item_level { get; set; }

	}

	[Message(OuterOpcode.M2C_EnterFunGame)]
	[ProtoContract]
	public partial class M2C_EnterFunGame: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long funyGameId { get; set; }

	}

//同步操作
	[Message(OuterOpcode.C2M_SynFunGameState)]
	[ProtoContract]
	public partial class C2M_SynFunGameState: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PicId { get; set; }

		[ProtoMember(2)]
		public List<int> itemList = new List<int>();

		[ProtoMember(3)]
		public int eventType { get; set; }

		[ProtoMember(4)]
		public List<int> itemMaskList = new List<int>();

	}

	[Message(OuterOpcode.M2C_SynFunGameState)]
	[ProtoContract]
	public partial class M2C_SynFunGameState: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PicId { get; set; }

		[ProtoMember(2)]
		public long RoleId { get; set; }

		[ProtoMember(3)]
		public List<int> itemList = new List<int>();

		[ProtoMember(4)]
		public int eventType { get; set; }

		[ProtoMember(5)]
		public List<int> itemMaskList = new List<int>();

	}

//---------------------   消消乐玩法    end -------//
//排行榜
	[Message(OuterOpcode.RankInfoProto)]
	[ProtoContract]
	public partial class RankInfoProto: Object
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public string Name { get; set; }

		[ProtoMember(4)]
		public int Level { get; set; }

		[ProtoMember(5)]
		public int Count { get; set; }

		[ProtoMember(6)]
		public string Country { get; set; }

		[ProtoMember(7)]
		public string Region { get; set; }

		[ProtoMember(8)]
		public int ParamInt { get; set; }

		[ProtoMember(9)]
		public int SkinId { get; set; }

	}

	[ResponseType(nameof(Rank2C_GetRansInfo))]
	[Message(OuterOpcode.C2Rank_GetRansInfo)]
	[ProtoContract]
	public partial class C2Rank_GetRansInfo: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RankType { get; set; }

		[ProtoMember(2)]
		public string Region { get; set; }

		[ProtoMember(3)]
		public string Country { get; set; }

		[ProtoMember(4)]
		public int StartRank { get; set; }

		[ProtoMember(5)]
		public int RankRange { get; set; }

	}

	[Message(OuterOpcode.Rank2C_GetRansInfo)]
	[ProtoContract]
	public partial class Rank2C_GetRansInfo: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankInfoProto> RankInfoProtoList = new List<RankInfoProto>();

		[ProtoMember(2)]
		public int MyRank { get; set; }

		[ProtoMember(3)]
		public int ParamInt { get; set; }

		[ProtoMember(4)]
		public int Count { get; set; }

		[ProtoMember(5)]
		public int StartRank { get; set; }

	}

//---------------------   道具使用  start -------//
//道具使用
	[ResponseType(nameof(M2C_UseItem))]
	[Message(OuterOpcode.C2M_UseItem)]
	[ProtoContract]
	public partial class C2M_UseItem: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int itemId { get; set; }

		[ProtoMember(2)]
		public string param { get; set; }

		[ProtoMember(3)]
		public string param2 { get; set; }

		[ProtoMember(4)]
		public bool offline { get; set; }

		[ProtoMember(5)]
		public int isFree { get; set; }

	}

	[Message(OuterOpcode.M2C_UseItem)]
	[ProtoContract]
	public partial class M2C_UseItem: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//---------------------   道具使用    end -------//
//------------------------ 充值 start --------------------
	[Message(OuterOpcode.RechrgeOrder)]
	[ProtoContract]
	public partial class RechrgeOrder: Object
	{
		[ProtoMember(1)]
		public long orderId { get; set; }

		[ProtoMember(2)]
		public long unitId { get; set; }

		[ProtoMember(3)]
		public long accountId { get; set; }

		[ProtoMember(4)]
		public int productId { get; set; }

		[ProtoMember(5)]
		public byte orderState { get; set; }

	}

//向缓存服请求玩家充值订单(判断是否有未发奖的订单)
	[ResponseType(nameof(UnitCache2Other_GetOrder))]
	[Message(OuterOpcode.Other2UnitCache_GetOrder)]
	[ProtoContract]
	public partial class Other2UnitCache_GetOrder: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

//缓存服返回玩家未完成的订单
	[Message(OuterOpcode.UnitCache2Other_GetOrder)]
	[ProtoContract]
	public partial class UnitCache2Other_GetOrder: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RechrgeOrder> orderList = new List<RechrgeOrder>();

	}

//向缓存服请求存储玩家充值订单(补发奖励)
	[ResponseType(nameof(UnitCache2Other_SaveOrder))]
	[Message(OuterOpcode.Other2UnitCache_SaveOrder)]
	[ProtoContract]
	public partial class Other2UnitCache_SaveOrder: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public List<RechrgeOrder> orderList = new List<RechrgeOrder>();

	}

	[Message(OuterOpcode.UnitCache2Other_SaveOrder)]
	[ProtoContract]
	public partial class UnitCache2Other_SaveOrder: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public bool state { get; set; }

	}

//获得充值奖励
	[ResponseType(nameof(M2C_Get_RechargeReward))]
	[Message(OuterOpcode.C2M_Get_RechargeReward)]
	[ProtoContract]
	public partial class C2M_Get_RechargeReward: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Key { get; set; }

		[ProtoMember(92)]
		public bool IsTest { get; set; }

		[ProtoMember(93)]
		public string productId { get; set; }

		[ProtoMember(94)]
		public string platform { get; set; }

		[ProtoMember(95)]
		public string price { get; set; }

		[ProtoMember(96)]
		public string isoCurrencyCode { get; set; }

		[ProtoMember(97)]
		public bool offline { get; set; }

	}

	[Message(OuterOpcode.M2C_Get_RechargeReward)]
	[ProtoContract]
	public partial class M2C_Get_RechargeReward: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public bool state { get; set; }

		[ProtoMember(2)]
		public string productId { get; set; }

		[ProtoMember(3)]
		public string platform { get; set; }

	}

//------------------------ 充值 end --------------------
//---------------------   邮箱绑定  start -------//
//邮箱绑定信息
	[ResponseType(nameof(M2C_EmailInfo))]
	[Message(OuterOpcode.C2M_EmailInfo)]
	[ProtoContract]
	public partial class C2M_EmailInfo: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_EmailInfo)]
	[ProtoContract]
	public partial class M2C_EmailInfo: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string EmailId { get; set; }

	}

//请求绑定邮箱
	[ResponseType(nameof(M2C_StartBindEmail))]
	[Message(OuterOpcode.C2M_StartBindEmail)]
	[ProtoContract]
	public partial class C2M_StartBindEmail: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string EmailId { get; set; }

	}

	[Message(OuterOpcode.M2C_StartBindEmail)]
	[ProtoContract]
	public partial class M2C_StartBindEmail: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string EmailId { get; set; }

	}

//验证邮箱验证码
	[ResponseType(nameof(M2C_StartBindEmailCode))]
	[Message(OuterOpcode.C2M_StartBindEmailCode)]
	[ProtoContract]
	public partial class C2M_StartBindEmailCode: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public long Code { get; set; }

	}

	[Message(OuterOpcode.M2C_StartBindEmailCode)]
	[ProtoContract]
	public partial class M2C_StartBindEmailCode: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//找回账号获取验证码
	[ResponseType(nameof(M2C_FindAccountEmailCode))]
	[Message(OuterOpcode.C2M_FindAccountEmailCode)]
	[ProtoContract]
	public partial class C2M_FindAccountEmailCode: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string EmailId { get; set; }

	}

	[Message(OuterOpcode.M2C_FindAccountEmailCode)]
	[ProtoContract]
	public partial class M2C_FindAccountEmailCode: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(93)]
		public long Code { get; set; }

	}

//通过邮箱ID找回账号
	[ResponseType(nameof(M2C_FindAccountByEmail))]
	[Message(OuterOpcode.C2M_FindAccountByEmail)]
	[ProtoContract]
	public partial class C2M_FindAccountByEmail: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string EmailId { get; set; }

		[ProtoMember(92)]
		public long Code { get; set; }

	}

	[Message(OuterOpcode.M2C_FindAccountByEmail)]
	[ProtoContract]
	public partial class M2C_FindAccountByEmail: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(93)]
		public string AccountName { get; set; }

		[ProtoMember(94)]
		public long Code { get; set; }

	}

//请求绑定google和ios
	[ResponseType(nameof(M2C_BindOtherInfo))]
	[Message(OuterOpcode.C2M_BindOtherInfo)]
	[ProtoContract]
	public partial class C2M_BindOtherInfo: Object, IActorLocationRequest
	{
		[ProtoMember(91)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_BindOtherInfo)]
	[ProtoContract]
	public partial class M2C_BindOtherInfo: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Uid { get; set; }

	}

//请求绑定google和ios
	[ResponseType(nameof(M2C_StartBindByOther))]
	[Message(OuterOpcode.C2M_StartBindByOther)]
	[ProtoContract]
	public partial class C2M_StartBindByOther: Object, IActorLocationRequest
	{
		[ProtoMember(91)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int BindType { get; set; }

		[ProtoMember(2)]
		public string Uid { get; set; }

	}

	[Message(OuterOpcode.M2C_StartBindByOther)]
	[ProtoContract]
	public partial class M2C_StartBindByOther: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string AccountName { get; set; }

		[ProtoMember(2)]
		public string GoogleOrIosId { get; set; }

	}

//---------------------   邮箱绑定    end -------//
//删除角色请求
	[ResponseType(nameof(M2C_DeleteRole))]
	[Message(OuterOpcode.C2M_DeleteRole)]
	[ProtoContract]
	public partial class C2M_DeleteRole: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OpType { get; set; }

	}

	[Message(OuterOpcode.M2C_DeleteRole)]
	[ProtoContract]
	public partial class M2C_DeleteRole: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//获取删除角色状态
	[ResponseType(nameof(M2C_GetRoleStatus))]
	[Message(OuterOpcode.C2M_GetRoleStatus)]
	[ProtoContract]
	public partial class C2M_GetRoleStatus: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_GetRoleStatus)]
	[ProtoContract]
	public partial class M2C_GetRoleStatus: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long StatusTime { get; set; }

	}

//---------------------   消消乐玩法    end -------//
//好友
	[Message(OuterOpcode.FriendInfoProto)]
	[ProtoContract]
	public partial class FriendInfoProto: Object
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public string Name { get; set; }

		[ProtoMember(4)]
		public int Level { get; set; }

		[ProtoMember(5)]
		public int Count { get; set; }

		[ProtoMember(6)]
		public string Country { get; set; }

		[ProtoMember(7)]
		public string Region { get; set; }

		[ProtoMember(8)]
		public bool IsOnline { get; set; }

		[ProtoMember(9)]
		public long ApplyTime { get; set; }

		[ProtoMember(10)]
		public int SkinId { get; set; }

	}

//请求好友列表
	[ResponseType(nameof(M2C_GetFriendsInfo))]
	[Message(OuterOpcode.C2M_GetFriendsInfo)]
	[ProtoContract]
	public partial class C2M_GetFriendsInfo: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Type { get; set; }

	}

	[Message(OuterOpcode.M2C_GetFriendsInfo)]
	[ProtoContract]
	public partial class M2C_GetFriendsInfo: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<FriendInfoProto> FriendList = new List<FriendInfoProto>();

		[ProtoMember(2)]
		public int Type { get; set; }

	}

//好友请求		双方都需要返回
	[ResponseType(nameof(M2C_FriendRequest))]
	[Message(OuterOpcode.C2M_FriendRequest)]
	[ProtoContract]
	public partial class C2M_FriendRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.M2C_FriendRequest)]
	[ProtoContract]
	public partial class M2C_FriendRequest: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public FriendInfoProto friendInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_FriendRequestNotice)]
	[ProtoContract]
	public partial class M2C_FriendRequestNotice: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public FriendInfoProto friendInfo { get; set; }

	}

//好友请求回复		双方都需要返回
	[ResponseType(nameof(M2C_FriendReply))]
	[Message(OuterOpcode.C2M_FriendReply)]
	[ProtoContract]
	public partial class C2M_FriendReply: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int Type { get; set; }

	}

	[Message(OuterOpcode.M2C_FriendReply)]
	[ProtoContract]
	public partial class M2C_FriendReply: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int Type { get; set; }

	}

	[Message(OuterOpcode.M2C_FriendReplyNotice)]
	[ProtoContract]
	public partial class M2C_FriendReplyNotice: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public FriendInfoProto friendInfo { get; set; }

		[ProtoMember(2)]
		public int Type { get; set; }

	}

//聊天
	[Message(OuterOpcode.C2M_Chat)]
	[ProtoContract]
	public partial class C2M_Chat: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public ChatInfo chatInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_Chat)]
	[ProtoContract]
	public partial class M2C_Chat: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public ChatInfo chatInfo { get; set; }

	}

//请求聊天记录
	[ResponseType(nameof(M2C_ChatRecord))]
	[Message(OuterOpcode.C2M_ChatRecord)]
	[ProtoContract]
	public partial class C2M_ChatRecord: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

//聊天记录
	[Message(OuterOpcode.M2C_ChatRecord)]
	[ProtoContract]
	public partial class M2C_ChatRecord: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<ChatInfo> chatInfos = new List<ChatInfo>();

	}

	[Message(OuterOpcode.ChatInfo)]
	[ProtoContract]
	public partial class ChatInfo: Object
	{
		[ProtoMember(1)]
		public long ResUseId { get; set; }

		[ProtoMember(2)]
		public string sourceName { get; set; }

		[ProtoMember(3)]
		public int skinId { get; set; }

		[ProtoMember(4)]
		public byte ChatType { get; set; }

		[ProtoMember(5)]
		public long TargetUseId { get; set; }

		[ProtoMember(6)]
		public string Content { get; set; }

	}

//新手引导
	[ResponseType(nameof(M2C_GuideStep))]
	[Message(OuterOpcode.C2M_GuideStep)]
	[ProtoContract]
	public partial class C2M_GuideStep: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Step { get; set; }

	}

	[Message(OuterOpcode.M2C_GuideStep)]
	[ProtoContract]
	public partial class M2C_GuideStep: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//好友删除
	[ResponseType(nameof(M2C_FriendDelete))]
	[Message(OuterOpcode.C2M_FriendDelete)]
	[ProtoContract]
	public partial class C2M_FriendDelete: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.M2C_FriendDelete)]
	[ProtoContract]
	public partial class M2C_FriendDelete: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

//搜索玩家
	[ResponseType(nameof(M2C_FriendSearch))]
	[Message(OuterOpcode.C2M_FriendSearch)]
	[ProtoContract]
	public partial class C2M_FriendSearch: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string SearchName { get; set; }

	}

	[Message(OuterOpcode.M2C_FriendSearch)]
	[ProtoContract]
	public partial class M2C_FriendSearch: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<FriendInfoProto> SearchFriend = new List<FriendInfoProto>();

	}

//修正消消乐数据
	[Message(OuterOpcode.C2M_ModifyFunyGameData)]
	[ProtoContract]
	public partial class C2M_ModifyFunyGameData: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int funyGameId { get; set; }

		[ProtoMember(2)]
		public int funyGameProgress { get; set; }

	}

//切换主城log
	[Message(OuterOpcode.C2M_ChangeSceneByClient)]
	[ProtoContract]
	public partial class C2M_ChangeSceneByClient: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OpType { get; set; }

		[ProtoMember(2)]
		public long ClientTime { get; set; }

		[ProtoMember(3)]
		public bool offline { get; set; }

	}

//消消乐消除
	[Message(OuterOpcode.C2M_FunyGameRemove)]
	[ProtoContract]
	public partial class C2M_FunyGameRemove: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<int> RType = new List<int>();

		[ProtoMember(2)]
		public List<int> RCount = new List<int>();

	}

//向服务器发送firebasetoken
	[Message(OuterOpcode.C2M_SendFireBaseToken)]
	[ProtoContract]
	public partial class C2M_SendFireBaseToken: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string fireBaseToken { get; set; }

	}

//向服务器发送广告奖励
	[Message(OuterOpcode.C2M_SendGetADAward)]
	[ProtoContract]
	public partial class C2M_SendGetADAward: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<int> itemIds = new List<int>();

		[ProtoMember(2)]
		public List<int> itemNums = new List<int>();

	}

	[Message(OuterOpcode.ServerMessage)]
	[ProtoContract]
	public partial class ServerMessage: Object
	{
		[ProtoMember(1)]
		public int ServerMsgType { get; set; }

		[ProtoMember(2)]
		public List<string> msgParams = new List<string>();

	}

//服务器系统消息
	[Message(OuterOpcode.M2C_ServerMessage)]
	[ProtoContract]
	public partial class M2C_ServerMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int ServerMsgType { get; set; }

		[ProtoMember(2)]
		public List<string> msgParams = new List<string>();

	}

//广告相关log
	[Message(OuterOpcode.C2M_SendADLogByClient)]
	[ProtoContract]
	public partial class C2M_SendADLogByClient: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OpType { get; set; }

		[ProtoMember(2)]
		public long ClientTime { get; set; }

		[ProtoMember(3)]
		public bool offline { get; set; }

		[ProtoMember(4)]
		public int param1 { get; set; }

		[ProtoMember(5)]
		public int param2 { get; set; }

		[ProtoMember(6)]
		public int param5 { get; set; }

	}

//服务器缓存的全服通知消息，玩家上线主动通知
	[Message(OuterOpcode.M2C_ServerCashMessage)]
	[ProtoContract]
	public partial class M2C_ServerCashMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<ServerMessage> messageList = new List<ServerMessage>();

	}

//大转盘数据
	[ResponseType(nameof(M2C_BigTurnTableInfor))]
	[Message(OuterOpcode.C2M_BigTurnTableInfor)]
	[ProtoContract]
	public partial class C2M_BigTurnTableInfor: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.M2C_BigTurnTableInfor)]
	[ProtoContract]
	public partial class M2C_BigTurnTableInfor: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int LastTime { get; set; }

	}

//大转盘动画结束，发奖
	[ResponseType(nameof(M2C_BigTurnTableEnd))]
	[Message(OuterOpcode.C2M_BigTurnTableEnd)]
	[ProtoContract]
	public partial class C2M_BigTurnTableEnd: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int LastTime { get; set; }

		[ProtoMember(2)]
		public int ResultIndex { get; set; }

	}

//大转盘动画结束，发奖
	[Message(OuterOpcode.M2C_BigTurnTableEnd)]
	[ProtoContract]
	public partial class M2C_BigTurnTableEnd: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public int LastTime { get; set; }

		[ProtoMember(3)]
		public int ResultIndex { get; set; }

	}

	[ResponseType(nameof(M2C_ReName))]
	[Message(OuterOpcode.C2M_ReName)]
	[ProtoContract]
	public partial class C2M_ReName: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string NewName { get; set; }

	}

	[Message(OuterOpcode.M2C_ReName)]
	[ProtoContract]
	public partial class M2C_ReName: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string NewName { get; set; }

	}

//充值
	[Message(OuterOpcode.C2M_SendRechargeByClient)]
	[ProtoContract]
	public partial class C2M_SendRechargeByClient: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RechargeValue { get; set; }

		[ProtoMember(2)]
		public long ClientTime { get; set; }

	}

//客户端日志
	[Message(OuterOpcode.C2M_SendBILogByClient)]
	[ProtoContract]
	public partial class C2M_SendBILogByClient: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OpType { get; set; }

		[ProtoMember(2)]
		public long ClientTime { get; set; }

		[ProtoMember(3)]
		public int param1 { get; set; }

		[ProtoMember(4)]
		public int param2 { get; set; }

		[ProtoMember(5)]
		public int param3 { get; set; }

		[ProtoMember(6)]
		public int param4 { get; set; }

		[ProtoMember(7)]
		public int param5 { get; set; }

	}

//消消乐 pvp 结算
	[ResponseType(nameof(M2C_FunyGamePvpResult))]
	[Message(OuterOpcode.C2M_FunyGamePvpResult)]
	[ProtoContract]
	public partial class C2M_FunyGamePvpResult: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public int funyGameProgress { get; set; }

		[ProtoMember(3)]
		public List<RankInfo> RankInfo = new List<RankInfo>();

	}

//消消乐 pvp 结算
	[Message(OuterOpcode.M2C_FunyGamePvpResult)]
	[ProtoContract]
	public partial class M2C_FunyGamePvpResult: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

	}

	[Message(OuterOpcode.M2C_NoticeFunyGamePvpResult)]
	[ProtoContract]
	public partial class M2C_NoticeFunyGamePvpResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<PvpRankInfo> pvpRankInfo = new List<PvpRankInfo>();

	}

	[Message(OuterOpcode.PvpRankInfo)]
	[ProtoContract]
	public partial class PvpRankInfo: Object
	{
		[ProtoMember(1)]
		public long roleId { get; set; }

		[ProtoMember(2)]
		public byte rank { get; set; }

		[ProtoMember(3)]
		public List<Reward> RewardList = new List<Reward>();

		[ProtoMember(4)]
		public int CurSkinId { get; set; }

	}

//小时榜信息
	[ResponseType(nameof(M2C_GetHourRanksInfo))]
	[Message(OuterOpcode.C2M_GetHourRanksInfo)]
	[ProtoContract]
	public partial class C2M_GetHourRanksInfo: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_GetHourRanksInfo)]
	[ProtoContract]
	public partial class M2C_GetHourRanksInfo: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankInfoProto> RankInfoProtoList = new List<RankInfoProto>();

	}

//消消乐关卡事件
	[ResponseType(nameof(M2C_GetFunyGameEvent))]
	[Message(OuterOpcode.C2M_GetFunyGameEvent)]
	[ProtoContract]
	public partial class C2M_GetFunyGameEvent: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int funyGameId { get; set; }

	}

//
	[Message(OuterOpcode.M2C_GetFunyGameEvent)]
	[ProtoContract]
	public partial class M2C_GetFunyGameEvent: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int funyGameId { get; set; }

		[ProtoMember(2)]
		public List<FunyLevelEvent> levelEvent = new List<FunyLevelEvent>();

	}

//pvp榜信息
	[ResponseType(nameof(M2C_GetPvpRanksInfo))]
	[Message(OuterOpcode.C2M_GetPvpRanksInfo)]
	[ProtoContract]
	public partial class C2M_GetPvpRanksInfo: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int PvpLevel { get; set; }

	}

	[Message(OuterOpcode.M2C_GetPvpRanksInfo)]
	[ProtoContract]
	public partial class M2C_GetPvpRanksInfo: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankInfoProto> RankInfoProtoList = new List<RankInfoProto>();

	}

}
