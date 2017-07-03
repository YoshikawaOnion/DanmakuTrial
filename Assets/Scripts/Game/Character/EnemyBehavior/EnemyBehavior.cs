using UnityEngine;
using System.Collections;
using System;
using UniRx;

public abstract class EnemyBehavior
{
    protected EnemyApi Api;
    protected Player Player
    {
        get { return GameManager.I.Player; }
    }

	private IDisposable actionSubscription;

    public EnemyBehavior(EnemyApi api)
    {
        Api = api;
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
