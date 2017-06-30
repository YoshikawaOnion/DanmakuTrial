using UnityEngine;
using System.Collections;
using System;
using UniRx;

public abstract class EnemyStrategy
{
    protected Enemy Owner;
    protected Player Player
    {
        get { return GameManager.I.Player; }
    }

	private IDisposable actionSubscription;

    public EnemyStrategy(Enemy owner)
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
