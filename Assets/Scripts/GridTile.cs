using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour {

    private Renderer planeRenderer;
    private GridManager manager;


    public enum GridStatus
    {
        Free,
        Occupied,
        Blocked
    }
    public GridStatus Status { get; private set; }


	// Use this for initialization
	void Start () {
        Status = GridStatus.Free;
        planeRenderer = GetComponentInChildren<Renderer>();
        SetColor();
        Hide();
	}

    public void SetColor(int color = -1)
    {
        if (manager == null || planeRenderer == null) return;

        if (color == -1)
        {
            color = Status == GridStatus.Free ? 0 : 1;
        }
        planeRenderer.material.SetColor("_Color", manager.TileColors[color]);
    }

    public void SetManager(GridManager manager)
    {
        this.manager = manager;
    }

    internal void Show()
    {
        SetColor();
    }

    internal void Hide()
    {
        SetColor(2);
    }
}
