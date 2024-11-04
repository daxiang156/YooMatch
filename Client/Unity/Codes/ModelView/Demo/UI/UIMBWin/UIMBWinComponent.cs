using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIMBWinComponent: Entity, IAwake, IDestroy
    {
        public Button btnReturn;
        public Button btnContinue;
        public Button btnTryAgan;
        public GameObject win;
        public GameObject lose;
        public GameObject item1;
        public GameObject item2;
        public Slider Slider1;
        public Slider Slider2;
        public Text textGrade;

        public Image Mask1;
        public Image Mask2;
        public Image Mask3;
        
        public GameObject spineLoading;
        
        public List<GameObject> itemList = new List<GameObject>();
        public List<Text> itemTextList = new List<Text>();
        
        
        public Button btnAd;
        public Button btnSkip;
        public GameObject ShowAd;
        
        public Text tips;
    }
}