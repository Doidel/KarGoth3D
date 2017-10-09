using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatField : MonoBehaviour {

    public GameObject WheatSprite;

	// Use this for initialization
	void Start () {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                var sprite = Instantiate(WheatSprite);
                //sprite.transform.pos
                sprite.transform.SetParent(transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
