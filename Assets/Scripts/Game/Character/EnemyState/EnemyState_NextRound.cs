using UnityEngine;
using System.Collections;
using UniRx;

public class EnemyState_NextRound : StateMachine
{
	private EnemyStateContext context;

	protected void EvStateEnter(EnemyStateContext context)
	{
		this.context = context;

        var isFirst = context.CurrentBehavior == null;
        var result = context.MoveNextBehavior();
        if (result)
		{
            if (!isFirst)
			{
				SoundManager.I.PlaySe(SeKind.EnemyDamaged);
            }
            context.ChangeState(Enemy.OpeningStateName);
            context.EventAccepter.OnNextRoundSubject.OnNext(Unit.Default);
        }
        else
		{
			SoundManager.I.PlaySe(SeKind.EnemyDefeated);
            context.ChangeState(Enemy.DefeatedStateName);
            context.EventAccepter.OnEnemyDefeatedSubject.OnNext(Unit.Default);
        }
	}
}
