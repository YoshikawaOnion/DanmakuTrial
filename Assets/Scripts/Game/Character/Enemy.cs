using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using Ist;

public class Enemy : MonoBehaviour
{
    public Vector2 PushOnShoot = new Vector2(0, 100);
    public Vector2 RecoverOnGuts = new Vector2(0, -12);

    public int Hp = 10;
    public BulletRenderer BulletRenderer;
    public GameObject LeftHand;
    public GameObject RightHand;
    public AudioClip ShootSound;
    public AudioClip DefeatedSound;
    public AudioClip DamageSound;
    public Script_SpriteStudio_Root spriteStudioRoot;
    public bool IsDefeated { get; private set; }

    private bool isEnabled;
    public bool isGutsMode;
    private bool isTimeToNextRound;
    private Vector3 initialPos;
    private IEnumerator coroutine;
    private IDisposable moveSubscription;
    private Rigidbody2D rigidbody;
    internal AudioSource AudioSource;

	// Use this for initialization
	void Start ()
    {
        AudioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        initialPos = transform.position;
		StartCoroutine(Act());
        isEnabled = false;
        IsDefeated = false;
        isGutsMode = false;
        isTimeToNextRound = false;
	}

	// Update is called once per frame
	void Update()
	{
        if (isGutsMode)
        {
            rigidbody.AddForce(RecoverOnGuts * Def.UnitPerPixel);
        }
    }

    private void OnDestroy()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnabled || isTimeToNextRound)
        {
            return;
        }
        if (collision.gameObject.tag == "PlayerShot")
        {
            Destroy(collision.gameObject);
            AudioSource.PlayOneShot(DamageSound, 0.2f);
            rigidbody.AddForce(PushOnShoot * Def.UnitPerPixel);
        }
    }

    public void Shot(float angle, float speed)
	{
        var position = transform.position.ZReplacedBy(transform.position.z + 10);
        BulletRenderer.Shoot(position, angle, speed);
    }

	public void Shot(Vector3 offset, float angle, float speed)
	{
        var pos = transform.position + offset.ZReplacedBy(10);
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

    private IEnumerator RunStrategy(EnemyStrategy strategy)
	{
		Move(initialPos, 20);
        yield return new WaitForSeconds(2);

		coroutine = strategy.Act();
		Hp = 110;
        isGutsMode = false;
        isTimeToNextRound = false;

        if (isEnabled)
		{
			StartCoroutine(coroutine);
        }

		while (!isTimeToNextRound)
		{
			yield return new WaitForSeconds(Time.deltaTime);
		}

        foreach (var bullet in BulletRenderer.Bullets)
        {
            Destroy(bullet.gameObject);
        }
        strategy.OnDestroy();

        AudioSource.PlayOneShot(DefeatedSound);
		StopCoroutine(coroutine);
    }

    private IEnumerator Act()
	{
		yield return RunStrategy(new EnemyStrategy1(this));
		yield return RunStrategy(new EnemyStrategy2(this));

		// 死亡処理
        if (moveSubscription != null)
		{
			moveSubscription.Dispose();
		}

        spriteStudioRoot.enabled = false;
		IsDefeated = true;

        yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
    }

    public void StartAction()
    {
        if (isEnabled || IsDefeated)
        {
            return;
        }
        StartCoroutine(coroutine);
        isEnabled = true;
    }

    public void StopAction()
    {
        if (!isEnabled || IsDefeated)
        {
            return;
        }
        StopCoroutine(coroutine);
        isEnabled = false;
    }

    public void ChangeGutsMode(bool isGuts)
    {
        isGutsMode = isGuts;
    }

    public void StartNextRound()
    {
        isTimeToNextRound = true;
    }
}
