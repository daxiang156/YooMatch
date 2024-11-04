using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIMBMoreWinComponent: Entity, IAwake
    {
        public GameObject titleLose;
        public GameObject titleWin;
        public List<GameObject> itemList = new List<GameObject>();
        public Button btnReturn;
        public Button btnAgain;
        public Text txtTotalCoin;
        public Text txtStarNum;
        public Image img_head;
        public Text txt_userName;
        public Image pvpLv;
    }
}