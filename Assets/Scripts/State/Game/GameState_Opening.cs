using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// 敵の弾幕パターンを開始する演出を行なっているステート。
/// </summary>
public class GameState_Opening : StateMachine
{
    private GameStateContext context;

    protected void EvStateEnter(GameStateContext context)
    {
        this.context = context;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
	{
        yield return new WaitForSeconds(0.1f);

		var player = GameManager.I.Player;
		var enemy = GameManager.I.Enemy;
		var api = new EnemyApi(GameManager.I.Enemy);

		api.MoveIt(enemy.InitialPosition, 20);
		player.ForceToMove(-enemy.InitialPosition, 20);
		yield return GameUiManager.I.AnimateGameStart();

        context.ChangeState(GameManager.PlayStateName);
        context.EventAccepter.OnRoundStartSubject.OnNext(Unit.Default);
    }
}
