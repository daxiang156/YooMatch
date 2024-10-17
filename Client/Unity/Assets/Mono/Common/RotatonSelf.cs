using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class RotatonSelf : MonoBehaviour
    {
        public GameObject rotateObj;

        public float rotateSpeed = -0.5f;
        // Start is called before the first frame update
        void Start()
        {
            if (this.rotateObj == null)
            {
                this.rotateObj = this.gameObject;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (this.rotateObj != null)
            {
                this.rotateObj.transform.Rotate(0, 0, rotateSpeed);
            }
        }
    }
}
