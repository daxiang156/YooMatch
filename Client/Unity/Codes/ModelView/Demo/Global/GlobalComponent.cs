using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class GlobalComponent: Entity, IAwake, IDestroy
    {
        public static GlobalComponent Instance;
        
        public Transform Global;
        public Transform Unit;
        public Transform UI;

        public Transform TPSControls;
        public Transform eventSystem;

        public Transform uiLoading;

        public Transform mainCamera;
        public Transform UICamera;

        public bool isFirstInCity = true;
        public int changeSceneId = 0;

        public Scene scene;
        public Dictionary<int, MBLv1> mbDic = null;
        public bool isShowConnectingLoading = false;
        public int noticeTime = 0;
    }
}