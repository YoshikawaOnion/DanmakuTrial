using UnityEngine;
using System.Collections;

public class EnemyState_NextRound : StateMachine
{
	private EnemyStateContext context;

	protected void EvStateEnter(EnemyStateContext context)
	{
		this.context = context;

		context.Enemy.Rigidbody.velocity = Vector3.zero;
		foreach (var bullet in context.BulletRenderer.Bullets)
		{
			Destroy(bullet.gameObject);
		}

        if (context.CurrentBehavior != null)
		{
			context.CurrentBehavior.Stop();
        }

		SoundManager.I.PlaySe(SeKind.EnemyDefeated);

        var result = context.MoveNextBehavior();
        if (result)
		{
            context.ChangeState(Enemy.OpeningStateName);
        }
        else
		{
            context.ChangeState(Enemy.DefeatedStateName);
        }
	}
}
