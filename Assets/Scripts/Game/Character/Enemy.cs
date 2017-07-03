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

    internal EnemyStrategy strategy;
    internal bool IsEnabled;
    internal bool IsGutsMode;
    internal bool IsTimeToNextRound;
    private EnemyBehavior executingBehavior;

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
        IsEnabled = false;
        IsDefeated = false;
        IsGutsMode = false;
        IsTimeToNextRound = false;
        api = new EnemyApi(this)
        {
            BulletRenderer = BulletRenderer
        };
	}

	// Update is called once per frame
	void Update()
	{
        if (IsGutsMode)
        {
            rigidbody.AddForce(RecoverOnGuts * Def.UnitPerPixel);
        }
    }

    private void OnDestroy()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsEnabled || IsTimeToNextRound)
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

    private IEnumerator RunStrategy(EnemyBehavior strategy)
	{
        var player = GameManager.I.Player;

        api.Move(initialPos, 20);
        player.ForceToMove(-initialPos, 20);
        player.StopAction();
        yield return GameUIManager.I.AnimateGameStart();
		player.StartAction();
        yield return new WaitForSeconds(0.5f);

        rigidbody.velocity = Vector3.zero;
        executingBehavior = strategy;
        IsGutsMode = false;
        IsTimeToNextRound = false;

        if (IsEnabled)
		{
			executingBehavior.Start();
        }

		while (!IsTimeToNextRound)
		{
			yield return new WaitForSeconds(Time.deltaTime);
		}

		rigidbody.velocity = Vector3.zero;
        foreach (var bullet in BulletRenderer.Bullets)
        {
            Destroy(bullet.gameObject);
        }
        executingBehavior.Stop();
        AudioSource.PlayOneShot(DefeatedSound);
    }

    private IEnumerator Act()
	{
        while (!IsEnabled)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

		IsTimeToNextRound = true;
        foreach (EnemyBehavior item in strategy.GetBehaviors(api))
        {
            yield return RunStrategy(item);
        }

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
        if (IsEnabled || IsDefeated)
        {
            return;
        }
        if (executingBehavior != null)
		{
            // TODO: ポーズを採用したら続きから再開の処理が要りそう
			executingBehavior.Start();
        }
        IsEnabled = true;
    }

    public void StopAction()
    {
        if (!IsEnabled || IsDefeated)
        {
            return;
        }
        if (executingBehavior != null)
        {
            executingBehavior.Stop();
        }
        IsEnabled = false;
    }

    public void ChangeGutsMode(bool isGuts)
    {
        IsGutsMode = isGuts;
    }

    public void StartNextRound()
    {
        IsTimeToNextRound = true;
    }
}
