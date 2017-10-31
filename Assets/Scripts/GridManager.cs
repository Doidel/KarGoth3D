using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public Color[] TileColors;
    public GridTile GridTilePrefab;

    public GridObject BrushSelected { get; private set; }

    public GridTile[,] Tiles { get; private set; }

	// Use this for initialization
	void Start () {
        Tiles = CreateGrid(0, 0, 50, 50);
    }

    public void SelectBrush(GridObject o)
    {
        BrushSelected = o;
    }

    private GridTile[,] CreateGrid(int offsetX, int offsetY, int sizeX, int sizeY)
    {
        GridTile[,] tiles = new GridTile[sizeX, sizeY];
        for (int x = offsetX; x < sizeX + offsetX; x++)
        {
            for (int y = offsetY; y < sizeY + offsetY; y++)
            {
                var gt = Instantiate(GridTilePrefab, new Vector3(x * 5, 0, y * 5), Quaternion.identity, transform);
                gt.SetManager(this);
                tiles[x - offsetX, y - offsetY] = gt;
            }
        }
        return tiles;
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
            BrushSelected = null;
        }

        int layerMask = 1 << 9;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            var gt = hit.transform.gameObject.GetComponentInParent<GridTile>();
            if (BrushSelected == null)
            {
                gt.Show();
                _previousHoveredGridTile = gt;
            }
            else
            {

            }
        }
    }
}
