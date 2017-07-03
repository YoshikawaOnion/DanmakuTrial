using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using Ist;

/// <summary>
/// 敵キャラクターを制御するクラス。
/// </summary>
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
    public Script_SpriteStudio_Root SpriteStudioRoot;
    public bool IsDefeated { get; private set; }

    internal EnemyStrategy Strategy;
    internal bool IsEnabled;
    internal bool IsGutsMode;
    internal bool IsTimeToNextRound;
    private EnemyBehavior executingBehavior;

    private Vector3 initialPos;
    private EnemyApi api;
    internal Rigidbody2D Rigidbody;
    internal AudioSource AudioSource;

	// Use this for initialization
	void Start ()
    {
        AudioSource = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
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
            Rigidbody.AddForce(RecoverOnGuts * Def.UnitPerPixel);
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
            Rigidbody.AddForce(PushOnShoot * Def.UnitPerPixel);
        }
    }

    /// <summary>
    /// EnemyBehaviorの実装クラスに従って行動を開始します。
    /// </summary>
    /// <returns>The strategy.</returns>
    /// <param name="strategy">Strategy.</param>
    private IEnumerator RunStrategy(EnemyBehavior strategy)
	{
        var player = GameManager.I.Player;

        api.Move(initialPos, 20);
        player.ForceToMove(-initialPos, 20);
        player.StopAction();
        yield return GameUiManager.I.AnimateGameStart();
		player.StartAction();
        yield return new WaitForSeconds(0.5f);

        Rigidbody.velocity = Vector3.zero;
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

		Rigidbody.velocity = Vector3.zero;
        foreach (var bullet in BulletRenderer.Bullets)
        {
            Destroy(bullet.gameObject);
        }
        executingBehavior.Stop();
        AudioSource.PlayOneShot(DefeatedSound);
    }

    /// <summary>
    /// 敵キャラクターの制御フローを開始します。
    /// </summary>
    /// <returns>The act.</returns>
    private IEnumerator Act()
	{
        while (!IsEnabled)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

		IsTimeToNextRound = true;
        foreach (EnemyBehavior item in Strategy.GetBehaviors(api))
        {
            yield return RunStrategy(item);
        }

        // 死亡処理ds
        SpriteStudioRoot.enabled = false;
		IsDefeated = true;

        yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
    }

    /// <summary>
    /// 敵キャラクターの動作を開始します。
    /// </summary>
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

    /// <summary>
    /// 敵キャラクターの動作を停止します。
    /// </summary>
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

    /// <summary>
    /// 踏ん張りモード(踏ん張って押し出しにくくなるモード)を切り替えます。
    /// </summary>
    /// <param name="isGuts">踏ん張りモードをオンにするかどうかを表す真偽値。</param>
    public void ChangeGutsMode(bool isGuts)
    {
        IsGutsMode = isGuts;
    }

    /// <summary>
    /// 現在行なっている弾幕パターンを終了して次のパターンへ移させます。
    /// </summary>
    public void StartNextRound()
    {
        IsTimeToNextRound = true;
    }
}
