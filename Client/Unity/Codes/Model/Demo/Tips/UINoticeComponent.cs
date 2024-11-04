using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UINoticeComponent  : Entity, IAwake,IDestroy
    {
        public Text text;
        public RectTransform ImageTip;
    }
}