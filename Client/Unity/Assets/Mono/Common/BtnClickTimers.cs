using UnityEngine;
using UnityEngine.UI;

public class BtnClickTimers : MonoBehaviour
{
    public bool isClick = false;//是否点击
    public float tempTime = 0;//计时器
    public float maxTime = 2;//计时器
    public Button Btn;//按钮

    void Awake()
    {
        if (Btn == null)
            this.Btn = this.gameObject.GetComponent<Button>();
        if (this.Btn == null)
        {
            Debug.LogError("Btn == null");
            return;
        }
        Btn.onClick.AddListener(OnClick);//注册按钮事件
    }

    void Update()
    {
        if (isClick)//如果被点击
        {
            tempTime += Time.deltaTime;
            if (tempTime > maxTime)
            {
                tempTime= 0;
                Btn.enabled = true;
                isClick = false;
            }
        }
    }

    private void OnClick()
    {
        isClick = true;
        Btn.enabled= false;
    }
}
