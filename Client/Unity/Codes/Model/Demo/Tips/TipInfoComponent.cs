using System;

namespace ET
{
    public class TipInfoComponent  : Entity, IAwake,IDestroy
    {
        public static TipInfoComponent Instance; 
        public string text;
        public string titleStr;
        public Action sureCallback;
        public Action cancelCallback;
        public bool isNet = false;
        public string itemIcon = "";
        public int itemId = 0;
        public int itemNum = 0;
        public int usdeNum = 1;
        public int goodId = 0;
    }
}