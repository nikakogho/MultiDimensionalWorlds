using UnityEngine;
using System.Collections.Generic;

public static class StaticFunctions
{
    public static Vector3 RandomLerp(Vector3 a, Vector3 b)
    {
        return new Vector3(Random.Range(a.x, b.x), Random.Range(a.y, b.y), Random.Range(a.z, b.z));
    }

    public static List<T> ToList<T>(this T[] array)
    {
        List<T> list = new List<T>();

        foreach (T element in array) list.Add(element);

        return list;
    }
}
