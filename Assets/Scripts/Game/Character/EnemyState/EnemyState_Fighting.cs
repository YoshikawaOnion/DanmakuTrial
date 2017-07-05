using UnityEngine;
using System.Collections;
using UniRx;

public class EnemyState_Fighting : StateMachine
{
	private EnemyStateContext context;
    protected CompositeDisposable Disposable { get; private set; }

    protected void EvStateEnter(EnemyStateContext context)
    {
		this.context = context;
        Disposable = new CompositeDisposable();
        var enemy = context.Enemy;

		enemy.OnExitFightArea
			   .Subscribe(u =>
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

            Debug.Log("EnemyDefeated");
			SoundManager.I.PlaySe(SeKind.EnemyDamaged);
			context.ChangeState(Enemy.NextRoundStateName);
		})
			   .AddTo(Disposable);

		enemy.OnHitPlayerShot
			   .Subscribe(collision =>
		{
			if (collision.gameObject.tag == Def.PlayerShotTag)
			{
				Destroy(collision.gameObject);
				SoundManager.I.PlaySe(SeKind.Hit, 0.2f);
				enemy.Rigidbody.AddForce(
					enemy.PushOnShoot * Def.UnitPerPixel);
			}
		})
			   .AddTo(Disposable);

        GameManager.I.Player.OnExitFightingAreaObservable
                   .Subscribe(u =>
        {
            context.CurrentBehavior.Stop();
            context.ChangeState(Enemy.WinStateName);
        });
    }

    protected override void EvStateExit()
    {
        Disposable.Dispose();
    }
}
