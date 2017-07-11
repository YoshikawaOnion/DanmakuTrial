using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// 敵が敗北したか、それとも次の弾幕を繰り出すかどうかを判定しているステート。
/// Enemy の初期状態です。
/// </summary>
public class EnemyState_NextRound : StateMachine
{
	private EnemyStateContext context;

	protected void EvStateEnter(EnemyStateContext context)
	{
		this.context = context;

        // 次の弾幕があるなら Opening, もうなければ Defeated へ遷移します。
        var isFirst = context.CurrentBehavior == null;
        var result = context.MoveNextBehavior();
        if (result)
		{
            if (!isFirst)
			{
				SoundManager.I.PlaySe(SeKind.EnemyDamaged);
            }
            context.CurrentBehavior.InitializeAsync(context.Api);
            context.ChangeState(Enemy.OpeningStateName);
            context.EventAccepter.OnNextRoundSubject.OnNext(Unit.Default);
        }
        else
		{
			SoundManager.I.PlaySe(SeKind.EnemyDamaged);
            context.ChangeState(Enemy.DefeatedStateName);
            context.EventAccepter.OnEnemyDefeatedSubject.OnNext(Unit.Default);
        }
	}
}
