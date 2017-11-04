using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWall : MonoBehaviour {

    public GameObject[] Vines;
    public const float GreeneryChance = 0.25f;

    void Awake()
    {
        if (Random.Range(0f, 1f) > GreeneryChance)
            foreach (var vine in Vines)
                DestroyImmediate(vine);
    }
}
