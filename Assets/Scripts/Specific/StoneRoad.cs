using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoneRoad : MonoBehaviour {

    public GameObject StoneWallSmall;
    public GameObject StoneWallMedium;
    public GameObject StoneWallLong;

    /// <summary>
    /// Depending on the neighbouring road connections choose the display of this road
    /// </summary>
    /// <param name="neighbours"></param>
    public void SetConnections(GridTile self)
    {
        var neighbours = self.Neighbours;
        var neighbourRoads = new StoneRoad[4];
        for (int i = 0; i < 4; i++)
            if (neighbours[i] != null && neighbours[i].Building != null)
                neighbourRoads[i] = neighbours[i].Building.GetComponent<StoneRoad>();

        int roadCount = neighbourRoads.Count(n => n != null);

        switch(roadCount)
        {
            case 0:
            case 4:
                // no walls

                break;
        }
        var roads = neighbours
    }
}
