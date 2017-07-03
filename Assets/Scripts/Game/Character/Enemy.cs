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
    public Vector2 RecoverOnGuts = new Vector2(0, -7);

    public BulletRenderer BulletRenderer;
    public GameObject LeftHand;
    public GameObject RightHand;
    public AudioClip ShootSound;
    public AudioClip DefeatedSound;
    public AudioClip DamageSound;
    public Script_SpriteStudio_Root spriteStudioRoot;
    public bool IsDefeated { get; private set; }

    private bool isEnabled;
    internal bool isGutsMode;
    private bool isTimeToNextRound;
    private EnemyStrategy executingStrategy;

    private Vector3 initialPos;
    private IDisposable moveSubscription;
    private EnemyApi api;
    internal Rigidbody2D rigidbody;
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
        api = new EnemyApi(this)
        {
            BulletRenderer = BulletRenderer
        };
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

    private IEnumerator RunStrategy(EnemyStrategy strategy)
	{
		api.Move(initialPos, 20);
        yield return new WaitForSeconds(2);

        rigidbody.velocity = Vector3.zero;
        executingStrategy = strategy;
        isGutsMode = false;
        isTimeToNextRound = false;

        if (isEnabled)
		{
			executingStrategy.Start();
        }

		while (!isTimeToNextRound)
		{
			yield return new WaitForSeconds(Time.deltaTime);
		}

        foreach (var bullet in BulletRenderer.Bullets)
        {
            Destroy(bullet.gameObject);
        }
        executingStrategy.Stop();
        AudioSource.PlayOneShot(DefeatedSound);
    }

    private IEnumerator Act()
	{
        while (!isEnabled)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

		isTimeToNextRound = true;
		yield return RunStrategy(new EnemyStrategy7(api));
		yield return RunStrategy(new EnemyStrategy6(api));
		yield return RunStrategy(new EnemyStrategy5(api));
		yield return RunStrategy(new EnemyStrategy1(api));
		yield return RunStrategy(new EnemyStrategy4(api));
		yield return RunStrategy(new EnemyStrategy2(api));
		yield return RunStrategy(new EnemyStrategy3(api));

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
        if (executingStrategy != null)
		{
            // TODO: ポーズを採用したら続きから再開の処理が要りそう
			executingStrategy.Start();
        }
        isEnabled = true;
    }

    public void StopAction()
    {
        if (!isEnabled || IsDefeated)
        {
            return;
        }
        if (executingStrategy != null)
        {
            executingStrategy.Stop();
        }
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
