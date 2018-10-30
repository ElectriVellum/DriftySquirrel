using System.Collections;
using UnityEngine;

public static class Coroutines
{
    public static IEnumerator WaitForRealSeconds(float seconds)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < (start + seconds))
        {
            yield return null;
        }
    }
}