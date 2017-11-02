using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour {

    private Renderer planeRenderer;
    private GridManager manager;

    public GridObject Building { get; private set; }

    public int PositionX;
    public int PositionY;

    // north, west, east, south
    public GridTile[] Neighbours { get; private set; }

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

    public void SetBuilding(GridObject o)
    {
        Building = o;
        Status = o == null ? GridStatus.Free : GridStatus.Occupied;
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

    public void UpdateNeighbours()
    {
        var t = GridManager.Instance.Tiles;
        var tileNorth = PositionY - 1 >= 0 ? t[PositionX, PositionY - 1] : null;
        var tileWest = PositionX - 1 >= 0 ? t[PositionX - 1, PositionY] : null;
        var tileEast = PositionX + 1 < t.GetLength(0) ? t[PositionX + 1, PositionY] : null;
        var tileSouth = PositionY + 1 < t.GetLength(1) ? t[PositionX, PositionY + 1] : null;
        Neighbours = new GridTile[] { tileNorth, tileWest, tileEast, tileSouth };
    }
}
