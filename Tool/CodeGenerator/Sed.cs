using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEditor;

public class EnemyBehavior$name$ : EnemyBehavior
{
    private EnemyBehavior$name$Asset asset;

    public EnemyBehavior$name$(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        throw new NotImplementedException();
    }
}
