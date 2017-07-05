using UnityEngine;
using System.Collections;
using UniRx;

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

		api.Move(enemy.InitialPosition, 20);
		player.ForceToMove(-enemy.InitialPosition, 20);
		player.StopAction();
		yield return GameUiManager.I.AnimateGameStart();

		player.StartAction();
		yield return new WaitForSeconds(0.5f);

        context.ChangeState(GameManager.PlayStateName);
        context.EventAccepter.OnRoundStartSubject.OnNext(Unit.Default);
    }
}
