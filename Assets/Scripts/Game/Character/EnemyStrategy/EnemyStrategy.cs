using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;
using System.Linq;

public abstract class EnemyStrategy
{
    private IEnumerable<EnemyBehavior> behaviors;

    public IObservable<Unit> LoadAssets()
    {
        behaviors = CreateBehaviors();
        var loads = behaviors.Select(x => x.LoadAsset());
        return Observable.WhenAll(loads);
    }

    public IEnumerable<EnemyBehavior> GetBehaviors(EnemyApi api)
    {
        return behaviors;
    }

    protected abstract IEnumerable<EnemyBehavior> CreateBehaviors();
}
