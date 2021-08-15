using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Report
{
    public static void Log(object Object)
    {
#if UNITY_EDITOR
        Debug.Log(Object.ToString());
#endif
    }

    public static void LogError(object Object)
    {
#if UNITY_EDITOR
        Debug.LogError(Object.ToString());
#endif
    }
}
