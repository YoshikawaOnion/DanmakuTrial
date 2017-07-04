using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

public class EnemyState_Fight : StateMachine
{
    private EnemyStateContext context;
    private CompositeDisposable disposable;

    protected void EvStateEnter(EnemyStateContext context)
    {
        this.context = context;
        disposable = new CompositeDisposable();

        context.Enemy.OnExitSafeAreaObservable
               .Subscribe(u =>
        {
            context.ChangeState(Enemy.GutsStateName);
        })
               .AddTo(disposable);
        
        context.Enemy.OnExitFightAreaObservable
               .Subscribe(u =>
        {
            context.ChangeState(Enemy.NextRoundStateName);
        })
               .AddTo(disposable);
        
        context.Enemy.OnHitPlayerShotObservable
               .Subscribe(collision =>
        {
            if (collision.gameObject.tag == Def.PlayerShotTag)
			{
				Destroy(collision.gameObject);
				SoundManager.I.PlaySe(SeKind.Hit, 0.2f);
				context.Enemy.Rigidbody.AddForce(
                    context.Enemy.PushOnShoot * Def.UnitPerPixel);
			}
        })
               .AddTo(disposable);
    }

    protected override void EvStateExit()
    {
        disposable.Dispose();
    }
}
