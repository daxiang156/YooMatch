using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class DelayFunc : MonoBehaviour
{
    private object[] paras;

    private Action callback;
    // Start is called before the first frame update
    public void SetDelayFunc(Action callback, float timeDelay,object[] paras)
    {
        this.callback = callback;
        this.paras = paras;
        Invoke("FuncDelay", timeDelay);
    }

    // Update is called once per frame
    void FuncDelay()
    {
        this.callback();
        Destroy(this);
    }
}
