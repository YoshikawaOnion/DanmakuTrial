using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class EnemyState_Opening : StateMachine
{
	private EnemyStateContext context;
    private IDisposable disposable;

	protected void EvStateEnter(EnemyStateContext context)
	{
		this.context = context;

        disposable = GameManager.I.GameEvents.OnRoundStart
                                .Subscribe(u =>
        {
			context.CurrentBehavior.Start();
			context.ChangeState(Enemy.NeutralStateName);
        });
	}

    protected override void EvStateExit()
    {
        disposable.Dispose();
    }
}
