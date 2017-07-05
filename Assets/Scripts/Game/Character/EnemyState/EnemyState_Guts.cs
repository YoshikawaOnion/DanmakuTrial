using UnityEngine;
using System.Collections;
using UniRx;
using System;

/// <summary>
/// Enemy が踏ん張りながら弾幕を繰り出しているステート。
/// </summary>
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
