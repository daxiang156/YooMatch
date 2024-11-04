using UnityEngine;

namespace ET
{
    public class ETListBaseItem
    {
        public GameObject gameObject;
        public Transform transform;
        public Transform parent;
        public string name;

        public ETListBaseItem(GameObject gameObject, Transform parent)
        {
            this.gameObject = gameObject;
            gameObject.transform.SetParent(parent);
            gameObject.SetActive(true);
            transform = this.gameObject.transform;
        }
        public RectTransform GetRectTransform()
        {
            return this.gameObject.GetComponent<RectTransform>();
        }
    }
}