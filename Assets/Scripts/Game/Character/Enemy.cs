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
    public GameObject LeftHand;
    public GameObject RightHand;

    private float time = 0;
    private Vector3 initialPos;
    private float angle = 0;
    private Coroutine coroutine;

	// Use this for initialization
	void Start ()
    {
        initialPos = transform.position;
        StartCoroutine(Act());
	}

	// Update is called once per frame
	void Update()
	{
		if (Hp <= 0 && coroutine != null)
		{
            StopCoroutine(coroutine);
            coroutine = null;
            Debug.Log("Stop!");
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

    public void Shot(float angle, float speed)
	{
        var position = transform.position.ZReplacedBy(transform.position.z + 10);
        BulletRenderer.Shoot(position, angle, speed);
    }

	public void Shot(Vector3 offset, float angle, float speed)
	{
        var pos = transform.position + offset;
		BulletRenderer.Shoot(pos, angle, speed);
	}

    public void Move(Vector3 goal, int durationFrame)
    {
        var prevPos = transform.position;
		Observable.EveryUpdate()
		          .Take(durationFrame)
		          .Subscribe(t =>
		{
			var pos = (prevPos * (durationFrame - t) + goal * t) / durationFrame;
			transform.position = pos;
		});
    }

    private IEnumerator Act()
    {
        var strategy = new EnemyStrategy1(this);
		coroutine = StartCoroutine(strategy.Act());
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (coroutine == null)
            {
                break;
            }
        }
        Debug.Log("Destroy?");
		Destroy(gameObject);
    }
}
