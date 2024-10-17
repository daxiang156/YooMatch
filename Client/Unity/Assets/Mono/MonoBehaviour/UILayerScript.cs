using UnityEngine;

namespace ET
{
    public enum UILayer
    {
        Hidden = 0,
        Low = 10,
        Low2 = 12,
        Low3 = 15,
        Mid = 20,
        Mid2 = 25,
        High = 30,
    }
    
    public class UILayerScript: MonoBehaviour
    {
        public UILayer UILayer;
    }
}