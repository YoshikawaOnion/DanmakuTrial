using UnityEngine;
using System.Collections;
using System;

public abstract class EnemyStrategy
{
    public abstract IEnumerable GetBehaviors(EnemyApi api);
}
