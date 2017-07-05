using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

public class EnemyState_Neutral : EnemyState_Fighting
{
    private EnemyStateContext context;

    protected void EvStateEnter(EnemyStateContext context)
    {
		this.context = context;
		base.EvStateEnter(context);

        context.Enemy.OnExitSafeArea
               .Subscribe(u =>
        {
            context.ChangeState(Enemy.GutsStateName);
        })
               .AddTo(Disposable);
    }
}
