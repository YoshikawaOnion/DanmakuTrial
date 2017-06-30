using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class NullEnemyShotBehavior : EnemyShotBehavior
{
    public NullEnemyShotBehavior(EnemyShot owner) : base(owner)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        return Observable.Return(Unit.Default);
    }
}
