using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyStrategy6 : EnemyStrategy
{
    public EnemyStrategy6(Enemy owner) : base(owner)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        throw new NotImplementedException();
    }

    private IEnumerator ShotCoroutine()
    {
        yield return null;
    }
}
