using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float Speed = 0.01f;
    public GameObject ShotObject;
    public GameObject ShotSource;
    public int shotSpan = 20;

    private Rigidbody2D rigidBody;
    private Vector3 shotPosition;
    private int shotTime = 0;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
		shotPosition = ShotObject.transform.localPosition;
		Debug.Log(shotPosition);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();
        Shot();
    }

    private void Shot()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (shotTime >= shotSpan)
			{
				var obj = Instantiate(ShotObject);
                obj.transform.position = ShotSource.transform.position;
                shotTime = 0;
			}
		}
		++shotTime;
    }

    private void Move()
    {
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
