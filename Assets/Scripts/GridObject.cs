using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridObject : MonoBehaviour {

    public int SizeX;
    public int SizeY;

    [HideInInspector]
    public int PositionX;
    [HideInInspector]
    public int PositionY;

    private void Awake()
    {

    }
}
