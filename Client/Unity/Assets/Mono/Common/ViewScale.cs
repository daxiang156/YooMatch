using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class ViewScale : MonoBehaviour
    {
        public float wide = 1251f;
        // Start is called before the first frame update
        void Start()
        {
            var grid = transform.Find("Content").GetComponent<GridLayoutGroup>();
            var rect = GetComponent<RectTransform>().rect;
            Debug.Log(rect.width + " 比值：" + rect.width / wide);
            if(rect.width / wide  < 1)
                grid.cellSize = new Vector2(rect.width / wide * grid.cellSize.x, grid.cellSize.y);        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
