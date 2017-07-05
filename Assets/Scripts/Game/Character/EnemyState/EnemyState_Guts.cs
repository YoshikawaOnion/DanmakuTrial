using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class EnemyState_Guts : EnemyState_Fighting
{
	private EnemyStateContext context;
    private Enemy enemy
    {
        get { return context.Enemy; }
    }

	protected void EvStateEnter(EnemyStateContext context)
	{
		this.context = context;
        base.EvStateEnter(context);

        enemy.OnEnterSafeArea
             .Subscribe(u =>
        {
            context.ChangeState(Enemy.NeutralStateName);
        })
             .AddTo(Disposable);
	}

    private void Update()
    {
        enemy.Rigidbody.AddForce(enemy.RecoverOnGuts * Def.UnitPerPixel);
    }
}
