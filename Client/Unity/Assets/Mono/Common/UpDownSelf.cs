using UnityEngine;

namespace ET
{
    public class UpDownSelf : MonoBehaviour
    {
        public GameObject updownObj;
        public Vector3 speed;
        public float maxValue;

        private Vector3 originPos;
        // Start is called before the first frame update
        void Start()
        {
            if (this.updownObj == null)
            {
                this.updownObj = this.gameObject;
            }
            originPos = this.updownObj.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if (this.updownObj != null)
            {
                this.updownObj.transform.localPosition += this.speed;
                Vector3 disance = this.updownObj.transform.localPosition - this.originPos;
                if (Mathf.Abs(disance.x) > this.maxValue || Mathf.Abs(disance.y) > this.maxValue)
                {
                    this.speed *= -1;
                }
            }
        }
    }
}