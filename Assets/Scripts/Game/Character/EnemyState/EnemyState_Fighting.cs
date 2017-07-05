using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// Enemy が弾幕を繰り出しているステートの基底クラス。
/// </summary>
public class EnemyState_Fighting : StateMachine
{
    protected EnemyStateContext Context { get; private set; }
    protected CompositeDisposable Disposable { get; private set; }

    protected void EvStateEnter(EnemyStateContext context)
    {
        this.Context = context;
        Disposable = new CompositeDisposable();
        var enemy = context.Enemy;

        GameManager.I.GameEvents.OnEnemyExitsFightArea
               .Subscribe(u => OnDamaged(context))
               .AddTo(Disposable);

        GameManager.I.GameEvents.OnHitPlayerShot
               .Subscribe(collision => OnHitPlayerShot(collision, enemy))
               .AddTo(Disposable);

        GameManager.I.GameEvents.OnPlayerExitsFightArea
                   .Subscribe(u => OnPlayerDamaged(context))
                   .AddTo(Disposable);
    }

    private static void OnPlayerDamaged(EnemyStateContext context)
    {
        context.CurrentBehavior.Stop();
        context.ChangeState(Enemy.WinStateName);
    }

    /// <summary>
    /// 敵がプレイヤーの弾に衝突した時に呼ばれるイベント。
    /// </summary>
    /// <param name="collision">Collision.</param>
    /// <param name="enemy">Enemy.</param>
    private static void OnHitPlayerShot(Collider2D collision, Enemy enemy)
    {
        if (collision.gameObject.tag == Def.PlayerShotTag)
        {
            Destroy(collision.gameObject);
            SoundManager.I.PlaySe(SeKind.Hit, 0.2f);
            enemy.Rigidbody.AddForce(enemy.PushOnShoot * Def.UnitPerPixel);
        }
    }

    /// <summary>
    /// 敵が弾幕パターンを攻略された時に呼ばれるイベント。
    /// </summary>
    /// <param name="context">Context.</param>
    private static void OnDamaged(EnemyStateContext context)
    {
        context.Enemy.Rigidbody.velocity = Vector3.zero;
        foreach (var bullet in context.BulletRenderer.Bullets)
        {
            Destroy(bullet.gameObject);
        }

        if (context.CurrentBehavior != null)
        {
            context.CurrentBehavior.Stop();
        }

        SoundManager.I.PlaySe(SeKind.EnemyDamaged);
        context.ChangeState(Enemy.NextRoundStateName);
    }

    protected override void EvStateExit()
    {
        if (Disposable != null)
		{
			Disposable.Dispose();
			Disposable = null;
        }
    }

    private void OnDestroy()
	{
		if (Disposable != null)
		{
			Disposable.Dispose();
			Disposable = null;
		}
    }
}
