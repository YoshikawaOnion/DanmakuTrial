using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyStrategyKomusubi : EnemyStrategy
{
    public override IEnumerable<EnemyBehavior> GetBehaviors(EnemyApi api)
    {
        yield return new EnemyBehavior8(api);
		yield return new EnemyBehavior1(api);
        yield return new EnemyBehavior9(api);
		yield return new EnemyBehavior3(api);
    }
}
