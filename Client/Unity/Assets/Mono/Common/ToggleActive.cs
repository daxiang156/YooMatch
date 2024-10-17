using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class ToggleActive : MonoBehaviour
    {
        private Toggle toggle;
        public GameObject[] activeObjs;

        public GameObject[] unActiveObjs; 
        
        // Start is called before the first frame update
        void Start()
        {
            this.toggle = GetComponent<Toggle>();
            if (this.toggle == null)
            {
                Debug.LogError("Toggle Active has no Toggle:" + this.gameObject.name);
                return;
            }

            for (int i = 0; i < this.activeObjs.Length; i++)
            {
                this.activeObjs[i].SetActive(this.toggle.isOn);
            }
            for (int i = 0; i < this.unActiveObjs.Length; i++)
            {
                this.unActiveObjs[i].SetActive(!this.toggle.isOn);
            }
            this.toggle.onValueChanged.AddListener(this.TglChange);
        }

        void TglChange(bool isOn)
        {
            //SoundManager.Instance.PlayActionSound("Music/commonBtn");
            for (int i = 0; i < this.activeObjs.Length; i++)
            {
                this.activeObjs[i].SetActive(this.toggle.isOn);
            }
            for (int i = 0; i < this.unActiveObjs.Length; i++)
            {
                this.unActiveObjs[i].SetActive(!this.toggle.isOn);
            }
        }
    }
}
