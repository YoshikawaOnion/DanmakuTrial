using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int Hp = 10;

    private float time = 0;
    private Vector3 initialPos;

	// Use this for initialization
	void Start () {
        initialPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        var y = Mathf.Sin(time * Mathf.PI * 2 / 2);
        y = Mathf.Clamp(y, -0.8f, 0.8f);
        y *= 0.5f;
        transform.SetPositionY(initialPos.y + y);

        if (Hp == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShot")
        {
            Destroy(collision.gameObject);
            Hp -= 1;
        }
    }
}
