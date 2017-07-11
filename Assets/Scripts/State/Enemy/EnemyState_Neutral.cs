using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

/// <summary>
/// Enemy が弾幕を繰り出しているステート。
/// </summary>
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
