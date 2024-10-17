using System;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    public float timeDelay;
    private void Start()
    {
        Invoke("DestroySelf", timeDelay);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}