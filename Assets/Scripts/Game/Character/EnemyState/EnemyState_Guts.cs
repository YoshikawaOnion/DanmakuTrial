using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class EnemyState_Guts : StateMachine
{
	private EnemyStateContext context;
    private CompositeDisposable disposable;
    private Enemy enemy
    {
        get { return context.Enemy; }
    }

	protected void EvStateEnter(EnemyStateContext context)
	{
		this.context = context;
        disposable = new CompositeDisposable();

        enemy.OnEnterSafeAreaObservable
             .Subscribe(u =>
        {
            context.ChangeState(Enemy.FightStateName);
        })
             .AddTo(disposable);
        
		enemy.OnExitFightAreaObservable
			   .Subscribe(u =>
		{
            context.ChangeState(Enemy.NextRoundStateName);
		})
               .AddTo(disposable);
        
		enemy.OnHitPlayerShotObservable
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
               .AddTo(disposable);
	}

    private void Update()
    {
        enemy.Rigidbody.AddForce(enemy.RecoverOnGuts * Def.UnitPerPixel);
    }

    protected override void EvStateExit()
    {
        disposable.Dispose();
    }
}
