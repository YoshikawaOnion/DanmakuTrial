using UnityEngine;
using System.Collections;
using UniRx;
using System;

public abstract class EnemyShotBehavior
{
    protected EnemyShot Owner;

    private IDisposable actionSubscription;

    public EnemyShotBehavior(EnemyShot owner)
    {
        Owner = owner;
    }

    protected abstract IObservable<Unit> GetAction();

    public void Start()
    {
        actionSubscription = GetAction().Subscribe();
    }

    public void Stop()
    {
        if (actionSubscription != null)
        {
            actionSubscription.Dispose();
        }
    }
}
