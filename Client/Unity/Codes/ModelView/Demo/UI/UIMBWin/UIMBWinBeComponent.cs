using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Spine.Unity;

namespace ET
{
    public class UIMBWinBeComponent: Entity, IAwake
    {
        public Button btnContinue;
        public Text textRank;
        public Text textNub;
        public GameObject top;
        public Image winBg;
    }
}