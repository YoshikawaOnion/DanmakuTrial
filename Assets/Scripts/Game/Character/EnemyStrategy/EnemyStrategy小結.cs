using UnityEngine;
using System.Collections;
using System;

public class EnemyStrategy小結 : EnemyStrategy
{
    public override IEnumerable GetBehaviors(EnemyApi api)
    {
		yield return new EnemyBehavior1(api);
		yield return new EnemyBehavior2(api);
		yield return new EnemyBehavior3(api);
    }
}
