using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using Ist;

public class Enemy : MonoBehaviour {
    public int Hp = 10;
    public BulletRenderer BulletRenderer;

    private float time = 0;
    private Vector3 initialPos;
    private float angle = 0;

	// Use this for initialization
	void Start ()
    {
        initialPos = transform.position;
        SetSpiralShotUp();
        SetRadiationShotUp();
	}

	// Update is called once per frame
	void Update()
	{
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

    private void SetRadiationShotUp()
    {
        Observable.Interval(TimeSpan.FromMilliseconds(50))
                  .Select(t => Enumerable.Range(-6, 13).Select(x => x * 24))
                  .Subscribe(angles =>
                  {
                      foreach (var a in angles)
                      {
                          var a2 = a + Mathf.Sin(angle * Mathf.PI * 2 / 50) * 3;
                          Shot(a2, 150);
                      }
                  });
    }

    private void SetSpiralShotUp()
    {
        Observable.Interval(TimeSpan.FromMilliseconds(80))
                  .Where(t => Hp > 0)
                  .Subscribe(t =>
                  {
                      Shot(angle, 100);
                      Shot(angle, -100);
                      angle += 13;
                  });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerShot")
        {
            Destroy(collision.gameObject);
            Hp -= 1;
        }
    }

    private void Shot(float angle, float speed)
	{
        var position = transform.position.ZReplacedBy(transform.position.z + 10);
        BulletRenderer.Shoot(position, angle, speed);
    }
}
