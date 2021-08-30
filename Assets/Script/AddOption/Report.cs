using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Report
{
    public static void Log(this object Object)
    {
#if UNITY_EDITOR
        Debug.Log(Object.ToString());
#endif
    }

    public static void LogError(this object Object)
    {
#if UNITY_EDITOR
        Debug.LogError(Object.ToString());
#endif
    }
}
