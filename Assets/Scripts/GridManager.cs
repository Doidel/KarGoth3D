﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public Color[] TileColors;
    public GridTile GridTilePrefab;

    public GridObject BrushSelected { get; private set; }
    public GridTile[,] Tiles { get; private set; }
    public static GridManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        CreateGrid(0, 0, 50, 50);
    }

    public void SelectBrush(GridObject o)
    {
        BrushSelected = Instantiate(o);
    }

    private void CreateGrid(int offsetX, int offsetY, int sizeX, int sizeY)
    {
        GridTile[,] tiles = new GridTile[sizeX, sizeY];
        for (int x = offsetX; x < sizeX + offsetX; x++)
        {
            for (int y = offsetY; y < sizeY + offsetY; y++)
            {
                var gt = Instantiate(GridTilePrefab, new Vector3(x * 5, 0, y * 5), Quaternion.identity, transform);
                gt.PositionX = x;
                gt.PositionY = y;
                gt.SetManager(this);
                tiles[x - offsetX, y - offsetY] = gt;
            }
        }

        Tiles = tiles;

        // update neighbour associations
        // TODO: update tiles' neighbours already existing before CreateGrid
        for (int x = offsetX; x < sizeX + offsetX; x++)
            for (int y = offsetY; y < sizeY + offsetY; y++)
                tiles[x - offsetX, y - offsetY].UpdateNeighbours();
    }

    // Update is called once per frame
    private GridTile _previousHoveredGridTile;
	void Update () {
		if (_previousHoveredGridTile != null)
        {
            _previousHoveredGridTile.Hide();
            _previousHoveredGridTile = null;
        }

        if (BrushSelected != null && Input.GetMouseButtonDown(1))
        {
            DestroyImmediate(BrushSelected);
            BrushSelected = null;
        }

        int layerMask = 1 << 9;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            var gt = hit.transform.gameObject.GetComponentInParent<GridTile>();
            
            gt.Show();
            _previousHoveredGridTile = gt;

            if (BrushSelected != null)
            {
                BrushSelected.transform.localPosition = new Vector3(gt.transform.localPosition.x, 0, gt.transform.localPosition.z);
                if (Input.GetMouseButtonDown(0) && gt.Status == GridTile.GridStatus.Free)
                {
                    // get all tiles that should be built upon
                    GridTile[,] brushTiles;
                    if (GetAllBrushTargetTiles(gt, out brushTiles))
                    {
                        Debug.Log("building road");
                        var newGridObject = Instantiate(BrushSelected);
                        newGridObject.transform.parent = gt.transform;
                        newGridObject.transform.localPosition = Vector3.zero;
                        newGridObject.transform.localRotation = Quaternion.identity;
                        newGridObject.PositionX = gt.PositionX;
                        newGridObject.PositionY = gt.PositionY;
                        gt.SetBuilding(newGridObject);
                    }
                }
            }
        }
    }

    private bool GetAllBrushTargetTiles(GridTile gt, out GridTile[,] tiles)
    {
        tiles = null;
        int offsetX = BrushSelected.SizeX / 2;
        int offsetY = BrushSelected.SizeY / 2;
        var gts = new GridTile[BrushSelected.SizeX, BrushSelected.SizeY];
        for (int x = 0; x < BrushSelected.SizeX; x++)
        {
            for (int y = 0; y < BrushSelected.SizeY; y++)
            {
                var tilePosX = gt.PositionX - offsetX + x;
                if (tilePosX < 0 || tilePosX >= Tiles.GetLength(0)) return false;
                var tilePosY = gt.PositionY - offsetY + y;
                if (tilePosY < 0 || tilePosY >= Tiles.GetLength(1)) return false;
                gts[x, y] = Tiles[tilePosX, tilePosY];
            }
        }
        tiles = gts;
        return true;
    }
}
