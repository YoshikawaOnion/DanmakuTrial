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
    public bool IsEnabled
    {
        get { return isEnabled_; }
        set
        {
            if (isEnabled_ != value)
            {
                if (value)
                {
                    StartCoroutine(coroutine);
                }
                else
                {
                    StopCoroutine(coroutine);
                }
            }
            isEnabled_ = value;
        }
    }

    private bool isEnabled_;
    private Vector3 initialPos;
    private IEnumerator coroutine;
    private IDisposable moveSubscription;

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
		}
	}

    private void OnDestroy()
    {
        if (moveSubscription != null)
        {
            moveSubscription.Dispose();
            moveSubscription = null;
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
        if (moveSubscription != null)
        {
            moveSubscription.Dispose();
        }

        var prevPos = transform.position;
		moveSubscription = Observable.EveryUpdate()
		          .Take(durationFrame)
		          .Subscribe(t =>
		{
			var pos = (prevPos * (durationFrame - t) + goal * t) / durationFrame;
			transform.position = pos;
		}, () => moveSubscription = null);
    }

    private IEnumerator Act()
	{
		var strategy = new EnemyStrategy1(this);
		coroutine = strategy.Act();
		StartCoroutine(coroutine);
        while (true)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (coroutine == null)
            {
                break;
            }
        }
		Destroy(gameObject);
    }
}
