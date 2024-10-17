using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBGridItem : MonoBehaviour
{
    public int itemId = 0;
    public int configId = 0;
    public int level = 0;

    // Update is called once per frame
    public void SetInfo(int itemId, int configId, int level)
    {
        this.itemId = itemId;
        this.configId = configId;
        this.level = level;
    }
}
