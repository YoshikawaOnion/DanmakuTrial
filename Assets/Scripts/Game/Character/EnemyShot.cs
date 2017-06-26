using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyShot : MonoBehaviour {
    public IObservable<Unit> DestroyEvent;

    private Subject<Unit> destroySubject;

    public EnemyShot()
	{
		destroySubject = new Subject<Unit>();
		DestroyEvent = destroySubject;
    }

    private void OnDestroy()
    {
        destroySubject.OnNext(Unit.Default);
        destroySubject.OnCompleted();
    }
}
