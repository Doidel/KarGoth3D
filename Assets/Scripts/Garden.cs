using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Garden : MonoBehaviour {

    public int SizeX = 6;
    public int SizeY = 3;
    public float PosScale = 3f;
    public bool WithRotation = true;
    public Vector3 PosShift = new Vector3(0.5f, 0f, 1.3f);
    public GameObject Stage1;
    public GameObject Stage2;
    public GameObject Stage3;
    public GameObject Broke;

    // Use this for initialization
    void Start()
    {
        SpawnCrops();
    }

    private void SpawnCrops()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        var random = new Random();
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                var plant = Instantiate(Stage3);
                var r = WithRotation ? Random.Range(0, 360) : Random.Range(0, 2) * 180f;
                plant.transform.SetParent(transform);
                //plant.transform.Rotate(new Vector3(0, r, 0));
                plant.transform.localRotation = Quaternion.Euler(-90, r, 0);
                plant.transform.localPosition = new Vector3(x - (float)SizeX / 2, 0, y - (float)SizeY / 2) * PosScale + PosShift;
            }
        }
    }
}
