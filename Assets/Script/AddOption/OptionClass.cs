using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OptionClass
{
    public static void CopyArr<T>(ref T[] c1, T[] c2)
    {
        c1 = new T[c2.Length];
        c1 = c2;
    }
}
