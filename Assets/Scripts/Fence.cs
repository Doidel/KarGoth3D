using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fence : MonoBehaviour {

    public int SizeX = 5;
    public int SizeY = 3;
    public float DistX = -1.66f * 2;
    public float DistY = -1.66f * 2;
    public int[] Open = new int[0];
    public GameObject FenceObject;

	// Use this for initialization
	void Start () {
        SpawnFence();

    }

    private void SpawnFence()
    {
        var l = Open.ToList();
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        
        for (int x = 0; x < SizeX; x++)
        {
            if (!l.Contains(x))
            {
                var fence = Instantiate(FenceObject);
                fence.transform.SetParent(transform);
                fence.transform.localPosition = new Vector3(x * DistX, 0, 0);
            }
            if (!l.Contains(SizeX + SizeY + SizeX - x - 1))
            {
                var fence = Instantiate(FenceObject);
                fence.transform.SetParent(transform);
                fence.transform.localPosition = new Vector3(x * DistX, 0, SizeY * DistY);
            }
        }
        for (int y = 0; y < SizeY; y++)
        {
            if (!l.Contains(SizeX * 2 + SizeY + SizeY - y - 1))
            {
                var fence = Instantiate(FenceObject);
                fence.transform.SetParent(transform);
                fence.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
                fence.transform.localPosition = new Vector3(-DistX / 2, 0, y * DistY + DistY / 2);
            }
            if (!l.Contains(y + SizeX))
            {
                var fence = Instantiate(FenceObject);
                fence.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
                fence.transform.SetParent(transform);
                fence.transform.localPosition = new Vector3(SizeX * DistX - DistX / 2, 0, y * DistY + DistY / 2);
            }
        }
    }
}
