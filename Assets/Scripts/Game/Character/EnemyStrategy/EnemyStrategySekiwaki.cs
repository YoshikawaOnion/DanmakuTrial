using UnityEngine;
using System.Collections;
using System;

public class EnemyStrategySekiwaki : EnemyStrategy
{
    public override IEnumerable GetBehaviors(EnemyApi api)
	{
		yield return new EnemyBehavior4(api);
		yield return new EnemyBehavior5(api);
		yield return new EnemyBehavior6(api);
		yield return new EnemyBehavior7(api);
    }
}
