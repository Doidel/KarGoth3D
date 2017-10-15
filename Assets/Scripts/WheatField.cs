using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class WheatField : MonoBehaviour {

    public GameObject WheatSprite;

	// Use this for initialization
	void Start ()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        var random = new Random();
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                var sprite = Instantiate(WheatSprite);
                var r = Random.Range(0, 180);
                sprite.transform.SetParent(transform);
                sprite.transform.localRotation = Quaternion.Euler(0, r, 0);
                sprite.transform.localPosition = new Vector3(x - 5, 0, y - 5);
            }
        }
	}
}
