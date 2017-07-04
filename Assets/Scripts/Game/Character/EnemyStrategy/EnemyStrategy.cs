using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class EnemyStrategy
{
    public abstract IEnumerable<EnemyBehavior> GetBehaviors(EnemyApi api);
}
