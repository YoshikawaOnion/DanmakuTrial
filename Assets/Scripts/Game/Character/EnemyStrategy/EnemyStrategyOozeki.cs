using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class EnemyStrategyOozeki : EnemyStrategy
{
    protected override IEnumerable<EnemyBehavior> CreateBehaviors()
	{
        yield return new EnemyBehavior6();
        yield return new EnemyBehavior8();
        yield return new EnemyBehavior12();
        yield return new EnemyBehavior2();
        yield return new EnemyBehavior10();
    }
}
