using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyStrategyOozeki : EnemyStrategy
{
    public override IEnumerable<EnemyBehavior> GetBehaviors(EnemyApi api)
    {
        yield return new EnemyBehavior6(api);
        yield return new EnemyBehavior8(api);
        yield return new EnemyBehavior12(api);
        yield return new EnemyBehavior2(api);
        yield return new EnemyBehavior10(api);
    }
}
