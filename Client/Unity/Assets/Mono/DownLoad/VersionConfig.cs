using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class FileVersionInfo
    {
        public string File;
        public string MD5;
        public long Size;
    }
    public class VersionConfig
    {
        public int Version;

        public long TotalSize;
		
        public Dictionary<string, FileVersionInfo> FileInfoDict = new Dictionary<string, FileVersionInfo>();

        public void EndInit()
        {
            foreach (FileVersionInfo fileVersionInfo in this.FileInfoDict.Values)
            {
                this.TotalSize += fileVersionInfo.Size;
            }
        }
    }

    public static class NoUpDate
    {
        public static Dictionary<string, string> NoUpDateDic = new Dictionary<string, string>()
        {
            ["a01.unity3d"] = "a01.unity3d",
            ["a02.unity3d"] = "a02.unity3d",
            ["a03.unity3d"] = "a03.unity3d",
            ["a04.unity3d"] = "a04.unity3d",
            ["a05.unity3d"] = "a05.unity3d",
            ["a06.unity3d"] = "a06.unity3d",
            ["a07.unity3d"] = "a07.unity3d",
            ["a08.unity3d"] = "a08.unity3d",
            ["a09.unity3d"] = "a09.unity3d",
            ["a10.unity3d"] = "a10.unity3d",
            ["a11.unity3d"] = "a11.unity3d",
            ["a12.unity3d"] = "a12.unity3d",
            ["a13.unity3d"] = "a13.unity3d",
            ["g00.unity3d"] = "g00.unity3d",
            ["g01h01_skin.unity3d"] = "g01h01_skin.unity3d",
            ["g02.unity3d"] = "g02.unity3d",
            ["g03.unity3d"] = "g03.unity3d",
            ["g04.unity3d"] = "g04.unity3d",
            ["g05.unity3d"] = "g05.unity3d",
            ["l01.unity3d"] = "l01.unity3d",
            ["l01h01_skin.unity3d"] = "l01h01_skin.unity3d",
            ["l02.unity3d"] = "l02.unity3d",
            ["l03.unity3d"] = "l03.unity3d",
            ["l04.unity3d"] = "l04.unity3d",
        };
    }
}