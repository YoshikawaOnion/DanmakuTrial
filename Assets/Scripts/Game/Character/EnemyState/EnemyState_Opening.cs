using UnityEngine;
using System.Collections;
using UniRx;
using System;

/// <summary>
/// Enemy が動作を開始するのを待機しているステート。
/// 動作が開始するタイミングは <seealso cref="GameState_Opening"/> が決定します。
/// </summary>
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
