using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEditor;

public class EnemyBehavior8 : EnemyBehavior
{
    private EnemyBehavior8Asset asset;

    public EnemyBehavior8(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        throw new NotImplementedException();
    }
}
