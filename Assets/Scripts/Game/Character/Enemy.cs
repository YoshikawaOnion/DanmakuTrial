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
    public static readonly string OpeningStateName = "EnemyState_Opening";
    public static readonly string FightStateName = "EnemyState_Fight";
    public static readonly string GutsStateName = "EnemyState_Guts";
    public static readonly string NextRoundStateName = "EnemyState_NextRound";
    public static readonly string DefeatedStateName = "EnemyState_Defeated";

	public GameObject LeftHand;
	public GameObject RightHand;
    public Vector2 PushOnShoot = new Vector2(0, 100);
    public Vector2 RecoverOnGuts = new Vector2(0, -7);

    [SerializeField]
    private Script_SpriteStudio_Root spriteStudioRoot;

    public bool IsDefeated { get; private set; }
	public EnemyStrategy Strategy { get; set; }
	public Rigidbody2D Rigidbody { get; private set; }
	public BulletRenderer BulletRenderer { get; set; }
	public StateMachine StateMachine { get; private set; }

    public IObservable<Unit> OnExitSafeAreaObservable
    {
        get { return onExitSafeArea; }
    }
    public IObservable<Unit> OnExitFightAreaObservable
    {
        get { return onExitFightArea; }
    }
	public IObservable<Unit> OnEnterSafeAreaObservable
	{
		get { return onEnterSafeArea; }
	}
	public IObservable<Collider2D> OnHitPlayerShotObservable
	{
		get { return onHitPlayerShot; }
	}
    private Subject<Unit> onExitSafeArea;
    private Subject<Unit> onEnterSafeArea;
    private Subject<Unit> onExitFightArea;
    private Subject<Collider2D> onHitPlayerShot;

	// Use this for initialization
	void Start ()
    {
        onExitSafeArea = new Subject<Unit>();
        onEnterSafeArea = new Subject<Unit>();
        onExitFightArea = new Subject<Unit>();
        onHitPlayerShot = new Subject<Collider2D>();

        Rigidbody = GetComponent<Rigidbody2D>();
        StateMachine = GetComponent<StateMachine>();
        IsDefeated = false;
	}

    private void OnDestroy()
    {
        Rigidbody = null;
        StateMachine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onHitPlayerShot.OnNext(collision);
    }


    public void StartAction()
	{
		var api = new EnemyApi(this)
		{
			BulletRenderer = BulletRenderer
		};
        var context = new EnemyStateContext
        {
            Api = api,
            BulletRenderer = BulletRenderer,
            Behaviors = Strategy.GetBehaviors(api).GetEnumerator(),
            Enemy = this,
            InitialPos = transform.position,
        };
        context.ChangeState(NextRoundStateName);
    }

    /// <summary>
    /// 安全圏を飛び出したことをこのオブジェクトに通知します。
    /// </summary>
    public void RaiseExitSafeArea()
    {
        onExitSafeArea.OnNext(Unit.Default);
    }

    /// <summary>
    /// 安全圏に戻ってきたことをこのオブジェクトに通知します。
    /// </summary>
    public void RaiseEnterSafeArea()
    {
        onEnterSafeArea.OnNext(Unit.Default);
    }

    /// <summary>
    /// 土俵を飛び出したことをこのオブジェクトに通知します。
    /// </summary>
    public void RaiseExitFightArea()
    {
        onExitFightArea.OnNext(Unit.Default);
    }

    public IEnumerator Vanish()
	{
		// 死亡処理
		spriteStudioRoot.enabled = false;
		IsDefeated = true;

		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);        
    }
}
