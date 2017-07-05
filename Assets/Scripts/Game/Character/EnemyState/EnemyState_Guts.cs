using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class EnemyState_Guts : EnemyState_Fighting
{
    protected new void EvStateEnter(EnemyStateContext context)
    {
        base.EvStateEnter(context);

        GameManager.I.GameEvents.OnEnemyEntersSafeArea
             .Subscribe(u => context.ChangeState(Enemy.NeutralStateName))
             .AddTo(Disposable);
    }

    private void Update()
    {
        Context.Enemy.Rigidbody.AddForce(
            Context.Enemy.RecoverOnGuts * Def.UnitPerPixel);
    }
}
