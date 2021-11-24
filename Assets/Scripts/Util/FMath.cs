using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FMath 
{
    public static float Approach(float start, float end, float shift)
    {
        if (start < end)
            return Mathf.Min(start + shift, end);

        return Mathf.Max(start - shift, end);
    }
}
