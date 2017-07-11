using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemyStrategySekiwaki : EnemyStrategy
{
    protected override IEnumerable<EnemyBehavior> CreateBehaviors()
	{
        yield return new EnemyBehavior5();
        yield return new EnemyBehavior4();
        yield return new EnemyBehavior7();
        yield return new EnemyBehavior11();
    }
}
