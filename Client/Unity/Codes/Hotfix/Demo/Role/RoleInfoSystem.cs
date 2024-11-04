using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public static class RoleInfoSystem
    {
        public static void FromMessage(this RoleInfo self, RoleInfoProto roleInfoProto)
        {
            self.UserId = roleInfoProto.UserId;
            self.Name = roleInfoProto.Name;
            self.State = roleInfoProto.State;
            self.AccountId = roleInfoProto.AccountId;
            self.CreateTime = roleInfoProto.CreateTime;
            self.LastLoginTime = roleInfoProto.LastLoginTime;
            self.AvatarId = roleInfoProto.AvatarId;
        }
        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            return new RoleInfoProto()
            {
                UserId = self.UserId,
                Name = self.Name,
                State = self.State,
                AccountId = self.AccountId,
                CreateTime = self.CreateTime,
                LastLoginTime = self.LastLoginTime,
                AvatarId = self.AvatarId,
            };
        }
    }
}
