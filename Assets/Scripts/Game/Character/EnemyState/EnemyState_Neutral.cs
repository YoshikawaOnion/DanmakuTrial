using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

public class EnemyState_Neutral : EnemyState_Fighting
{
    protected new void EvStateEnter(EnemyStateContext context)
    {
		base.EvStateEnter(context);

        GameManager.I.GameEvents.OnEnemyExitsSafeArea
               .Subscribe(u => context.ChangeState(Enemy.GutsStateName))
               .AddTo(Disposable);
    }
}
