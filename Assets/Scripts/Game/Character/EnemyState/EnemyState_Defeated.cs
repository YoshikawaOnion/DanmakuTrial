using UnityEngine;
using System.Collections;

public class EnemyState_Defeated : StateMachine
{
	private EnemyStateContext context;

	protected void EvStateEnter(EnemyStateContext context)
	{
		this.context = context;
        StartCoroutine(context.Enemy.Vanish());
	}
}
