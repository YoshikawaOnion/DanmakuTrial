using UnityEngine;
using System.Collections;

public class EnemyState_Opening : StateMachine
{
	private EnemyStateContext context;

	protected void EvStateEnter(EnemyStateContext context)
	{
		this.context = context;
        StartCoroutine(Run());
	}

    private IEnumerator Run()
    {
		var player = GameManager.I.Player;
		var enemy = GameManager.I.Enemy;
		var api = new EnemyApi(GameManager.I.Enemy);

		api.Move(enemy.InitialPosition, 20);
		player.ForceToMove(-enemy.InitialPosition, 20);
		player.StopAction();
		yield return GameUiManager.I.AnimateGameStart();

		player.StartAction();
		yield return new WaitForSeconds(0.5f);

		context.CurrentBehavior.Start();
		context.ChangeState(Enemy.NeutralStateName);
    }
}
