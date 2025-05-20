using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static void DestroyChildren(this GameObject go)
    {
        Transform[] children = new Transform[go.transform.childCount];
        for (int i = 0; i < go.transform.childCount; i++)
        {
            children[i] = go.transform.GetChild(i);
        }

        foreach (Transform child in children)
        {
            Object.Destroy(child.gameObject);
        }
    }

    public static void DestroyChildren(this Transform go)
    {
        Transform[] children = new Transform[go.childCount];
        for (int i = 0; i < go.childCount; i++)
        {
            children[i] = go.GetChild(i);
        }
        
        foreach (Transform child in children)
        {
            Object.Destroy(child.gameObject);
        }
    }
}
