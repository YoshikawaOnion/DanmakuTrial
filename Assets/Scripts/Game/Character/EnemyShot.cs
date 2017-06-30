using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyShot : MonoBehaviour {
    public IObservable<Unit> DestroyEvent;
	public EnemyShotBehavior behavior;

    internal Enemy Owner;

	private Subject<Unit> destroySubject;

    public EnemyShot()
	{
		destroySubject = new Subject<Unit>();
		DestroyEvent = destroySubject;
        behavior = new NullEnemyShotBehavior(this);
    }

    private void Start()
    {
        behavior.Start();
    }

    private void OnDestroy()
    {
        behavior.Stop();
        destroySubject.OnNext(Unit.Default);
        destroySubject.OnCompleted();
    }
}
