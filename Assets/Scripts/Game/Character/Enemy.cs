using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class Enemy : MonoBehaviour {
    public int Hp = 10;
    public GameObject ShotObject;

    private float time = 0;
    private Vector3 initialPos;

	// Use this for initialization
	void Start () {
		initialPos = transform.position;
		Observable.Interval(TimeSpan.FromMilliseconds(500))
                  .Where(t => Hp > 0)
				  .Subscribe(t =>
		{
			var shot = Instantiate(ShotObject);
			shot.transform.position = transform.position;
			shot.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -100);
		});
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
