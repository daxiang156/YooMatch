using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIMBItemComponent : Entity, IAwake
    {
//        public int configId;
        public int row;
        public int num;
        public int level;
        public int itemId;
        public Transform obj;
        public GameObject grey;
        public Image btnItem;
        public Image bgImg;
        public bool canKill = false;
        public int downnum = 0;
        public int itemUnitId;
        public bool isDown = false;
        public bool isCanRotate = false;
        public int itemEffectNum = 0;//特效效果，雷，冰。。

        public Dictionary<int, UIMBItemComponent> overGrid = new Dictionary<int, UIMBItemComponent>();
    }
}