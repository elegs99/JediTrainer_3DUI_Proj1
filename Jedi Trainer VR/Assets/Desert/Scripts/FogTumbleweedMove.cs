using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTumbleweedMove : MonoBehaviour {

    const float INDENT_DOWN = 1.46f;

    public Transform fog;   // Fog transform
	
	// Update is called once per frame
	void Update () {
        fog.position = new Vector3(transform.position.x, transform.position.y - INDENT_DOWN, transform.position.z);   // Move fog along with tumbleweed
    }
}
