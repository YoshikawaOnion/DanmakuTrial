using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class CircleEnemyShotBehavior : EnemyShotBehavior
{
    public CircleEnemyShotBehavior(EnemyShot owner) : base(owner)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        throw new NotImplementedException();
    }
}
