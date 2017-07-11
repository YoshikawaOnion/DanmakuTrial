using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class EnemyStrategyKomusubi : EnemyStrategy
{
    protected override IEnumerable<EnemyBehavior> CreateBehaviors()
    {
        yield return new EnemyBehavior1();
        yield return new EnemyBehavior9();
        yield return new EnemyBehavior3();
    }
}
