using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIPopQuitComponent: Entity, IAwake
    {
        public Button btnClose;
        public Button btnCancel;
        public Button btnConfirm;
        public Text text;
        public Text title;
        
        public GameObject netWork;
        
    }
}