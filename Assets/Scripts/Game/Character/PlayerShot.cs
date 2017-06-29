using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour {
    [Tooltip("速度[px/sec]")]
    public float Speed = 10;

	// Use this for initialization
	void Start () {
        var rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector3(0, Speed * Def.UnitPerPixel, 0);
	}
}
