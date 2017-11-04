using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoneRoad : MonoBehaviour {

    public GameObject StoneWallSmallPrefab;
    public GameObject StoneWallMediumPrefab;
    public GameObject StoneWallLongPrefab;

    public const float WallDistanceFromCenter = 2f;

    public GameObject[] StoneWalls { get; private set; }

    private GridObject myGridObject;

    void Awake()
    {
        myGridObject = gameObject.GetComponent<GridObject>();
    }

    void Start()
    {
        SetConnections(GridManager.Instance.Tiles[myGridObject.PositionX, myGridObject.PositionY], true);
    }

    /// <summary>
    /// Depending on the neighbouring road connections choose the display of this road
    /// </summary>
    /// <param name="neighbours"></param>
    public void SetConnections(GridTile self, bool propagateToNeighbours)
    {
        var neighbours = self.Neighbours;
        var neighbourRoads = new StoneRoad[4]; // north, west, east, south
        for (int i = 0; i < 4; i++)
            if (neighbours[i] != null && neighbours[i].Building != null)
                neighbourRoads[i] = neighbours[i].Building.GetComponent<StoneRoad>();

        int roadCount = neighbourRoads.Count(n => n != null);

        // remove already existing walls
        if (StoneWalls != null)
        {
            foreach (var wall in StoneWalls)
            {
                DestroyImmediate(wall);
            }
            StoneWalls = null;
        }

        float centerOffset = 2.5f;

        switch(roadCount)
        {
            case 0:
            case 4:
                // no walls
                break;
            case 1:
                StoneWalls = new GameObject[] {
                    Instantiate(StoneWallLongPrefab, gameObject.transform),
                    Instantiate(StoneWallLongPrefab, gameObject.transform)
                };
                if (neighbourRoads[0] != null || neighbourRoads[3] != null)
                {
                    StoneWalls[0].transform.localPosition = new Vector3(-WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[0].transform.localRotation = Quaternion.Euler(0, 90, 0);
                    StoneWalls[1].transform.localPosition = new Vector3(WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[1].transform.localRotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    StoneWalls[0].transform.localPosition = new Vector3(centerOffset, 0, -WallDistanceFromCenter + centerOffset);
                    StoneWalls[1].transform.localPosition = new Vector3(centerOffset, 0, WallDistanceFromCenter + centerOffset);
                }
                break;
            case 2:
                // can be curve or straight
                StoneWalls = new GameObject[] {
                    Instantiate(StoneWallLongPrefab, gameObject.transform),
                    Instantiate(StoneWallLongPrefab, gameObject.transform)
                };
                if (neighbourRoads[0] != null && neighbourRoads[3] != null)
                {
                    StoneWalls[0].transform.localPosition = new Vector3(-WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[0].transform.localRotation = Quaternion.Euler(0, 90, 0);
                    StoneWalls[1].transform.localPosition = new Vector3(WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[1].transform.localRotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourRoads[1] != null && neighbourRoads[2] != null)
                {
                    StoneWalls[0].transform.localPosition = new Vector3(centerOffset, 0, -WallDistanceFromCenter + centerOffset);
                    StoneWalls[1].transform.localPosition = new Vector3(centerOffset, 0, WallDistanceFromCenter + centerOffset);
                }
                else if (neighbourRoads[0] != null && neighbourRoads[1] != null) // north and west
                {
                    StoneWalls[0].transform.localPosition = new Vector3(WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[0].transform.localRotation = Quaternion.Euler(0, 90, 0);
                    StoneWalls[1].transform.localPosition = new Vector3(centerOffset, 0, WallDistanceFromCenter + centerOffset);
                }
                else if (neighbourRoads[0] != null && neighbourRoads[2] != null) // north and east
                {
                    StoneWalls[0].transform.localPosition = new Vector3(-WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[0].transform.localRotation = Quaternion.Euler(0, 90, 0);
                    StoneWalls[1].transform.localPosition = new Vector3(centerOffset, 0, WallDistanceFromCenter + centerOffset);
                }
                else if (neighbourRoads[3] != null && neighbourRoads[1] != null) // south and west
                {
                    StoneWalls[0].transform.localPosition = new Vector3(WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[0].transform.localRotation = Quaternion.Euler(0, 90, 0);
                    StoneWalls[1].transform.localPosition = new Vector3(centerOffset, 0, -WallDistanceFromCenter + centerOffset);
                }
                else if (neighbourRoads[3] != null && neighbourRoads[2] != null) // south and east
                {
                    StoneWalls[0].transform.localPosition = new Vector3(-WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[0].transform.localRotation = Quaternion.Euler(0, 90, 0);
                    StoneWalls[1].transform.localPosition = new Vector3(centerOffset, 0, -WallDistanceFromCenter + centerOffset);
                }
                break;
            case 3:
                StoneWalls = new GameObject[] {
                    Instantiate(StoneWallLongPrefab, gameObject.transform)
                };
                if (neighbourRoads[0] == null)
                {
                    StoneWalls[0].transform.localPosition = new Vector3(centerOffset, 0, -WallDistanceFromCenter + centerOffset);
                }
                else if (neighbourRoads[3] == null)
                {
                    StoneWalls[0].transform.localPosition = new Vector3(centerOffset, 0, WallDistanceFromCenter + centerOffset);
                }
                else if (neighbourRoads[1] == null)
                {
                    StoneWalls[0].transform.localPosition = new Vector3(-WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[0].transform.localRotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourRoads[2] == null)
                {
                    StoneWalls[0].transform.localPosition = new Vector3(WallDistanceFromCenter + centerOffset, 0, centerOffset);
                    StoneWalls[0].transform.localRotation = Quaternion.Euler(0, 90, 0);
                }
                break;
        }

        if (propagateToNeighbours) {
            for (int n = 0; n < neighbourRoads.Length; n++)
            {
                if (neighbourRoads[n] != null)
                    neighbourRoads[n].SetConnections(neighbours[n], false);
            }
        }
    }
}
