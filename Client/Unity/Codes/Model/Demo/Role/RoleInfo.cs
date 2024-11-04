using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public enum RoleInfoState
    {
        Normal = 0,
        Freeze,
    }

    public class RoleInfo : Entity,IAwake
    {
        public string Name;

        public int State;

        public long AccountId;

        public long UserId;

        public long LastLoginTime;

        public long CreateTime;

        public int AvatarId;
    }
}
