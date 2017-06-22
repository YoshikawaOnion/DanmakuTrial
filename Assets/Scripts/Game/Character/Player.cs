using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float Speed = 0.01f;
    public GameObject LeftWall;
    public GameObject RightWall;

    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 velocity = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity.x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity.x += 1;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			velocity.y -= 1;
		}
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity.y += 1;
        }

        var length = velocity.magnitude;
        if (length != 0)
		{
			velocity /= length;
        }
        rigidBody.velocity = velocity * Speed;
    }
}
