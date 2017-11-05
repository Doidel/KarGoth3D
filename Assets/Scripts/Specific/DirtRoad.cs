using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirtRoad : MonoBehaviour {

    public Texture2D Center;
    public Texture2D[] Straights;
    public Texture2D[] Curves;
    public Texture2D[] Forks;

    private GridObject myGridObject;
    private Renderer childRenderer;
    private Transform childTransform;

    void Awake()
    {
        myGridObject = gameObject.GetComponent<GridObject>();
        //childTransform = gameObject.GetComponentInChildren<Transform>();
        foreach (Transform child in transform)
        {
            childTransform = child;
            break;
        }
    }

    void Start()
    {
        childRenderer = GetComponentInChildren<Renderer>();
        SetConnections(GridManager.Instance.Tiles[myGridObject.PositionX, myGridObject.PositionY], true);
    }

    /// <summary>
    /// Depending on the neighbouring road connections choose the display of this road
    /// </summary>
    /// <param name="neighbours"></param>
    public void SetConnections(GridTile self, bool propagateToNeighbours)
    {
        var neighbours = self.Neighbours;
        var neighbourRoads = new DirtRoad[4]; // north, west, east, south
        for (int i = 0; i < 4; i++)
            if (neighbours[i] != null && neighbours[i].Building != null)
                neighbourRoads[i] = neighbours[i].Building.GetComponent<DirtRoad>();

        int roadCount = neighbourRoads.Count(n => n != null);


        switch (roadCount)
        {
            case 0:
            case 4:
                // no walls
                break;
            case 1:
                if (neighbourRoads[0] != null || neighbourRoads[3] != null)
                {
                    childTransform.localRotation = Quaternion.Euler(0, 90, 0);
                    childRenderer.material.SetTexture("_MainTex", Straights[Random.Range(0, Straights.Length)]);
                }
                else
                {
                    childTransform.localRotation = Quaternion.identity;
                    childRenderer.material.SetTexture("_MainTex", Straights[Random.Range(0, Straights.Length)]);
                }
                break;
            case 2:
                // can be curve or straight
                if (neighbourRoads[0] != null && neighbourRoads[3] != null)
                {
                    childTransform.localRotation = Quaternion.Euler(0, 90, 0);
                    childRenderer.material.SetTexture("_MainTex", Straights[Random.Range(0, Straights.Length)]);
                }
                else if (neighbourRoads[1] != null && neighbourRoads[2] != null)
                {
                    childTransform.localRotation = Quaternion.identity;
                    childRenderer.material.SetTexture("_MainTex", Straights[Random.Range(0, Straights.Length)]);
                }
                else if (neighbourRoads[0] != null && neighbourRoads[1] != null) // north and west
                {
                    childTransform.localRotation = Quaternion.Euler(0, 90, 0);
                    childRenderer.material.SetTexture("_MainTex", Curves[Random.Range(0, Curves.Length)]);
                }
                else if (neighbourRoads[0] != null && neighbourRoads[2] != null) // north and east
                {
                    childTransform.localRotation = Quaternion.identity;
                    childRenderer.material.SetTexture("_MainTex", Curves[Random.Range(0, Curves.Length)]);
                }
                else if (neighbourRoads[3] != null && neighbourRoads[1] != null) // south and west
                {
                    childTransform.localRotation = Quaternion.Euler(0, 180, 0);
                    childRenderer.material.SetTexture("_MainTex", Curves[Random.Range(0, Curves.Length)]);
                }
                else if (neighbourRoads[3] != null && neighbourRoads[2] != null) // south and east
                {
                    childTransform.localRotation = Quaternion.Euler(0, 270, 0);
                    childRenderer.material.SetTexture("_MainTex", Curves[Random.Range(0, Curves.Length)]);
                }
                break;
            case 3:
                /*StoneWalls = new GameObject[] {
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
                }*/
                break;
        }

        if (propagateToNeighbours)
        {
            for (int n = 0; n < neighbourRoads.Length; n++)
            {
                if (neighbourRoads[n] != null)
                    neighbourRoads[n].SetConnections(neighbours[n], false);
            }
        }
    }
}
