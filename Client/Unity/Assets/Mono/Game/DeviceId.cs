using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DeviceId
{
    private static DeviceId instance;
    public static DeviceId Instance
    {
        get
        {
            if (instance == null)
                instance = new DeviceId();
            return instance;
        }
    }
    private string androidId = "";
    public string GetAndroidId()
    {
        if (this.androidId != "")
            return this.androidId;
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                    AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");

                    AndroidJavaClass secureClass = new AndroidJavaClass("android.provider.Settings$Secure");

                    androidId = secureClass.CallStatic<string>("getString", contentResolver, "android_id");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to get Android ID: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("This is not an Android device.");
        }
        Debug.Log("androidId:" + androidId);
        return androidId;
    }
}
