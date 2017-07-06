using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵のショットを表すクラス。
/// </summary>
public class EnemyShot : MonoBehaviour {
    public IObservable<Unit> DestroyEvent;
	public EnemyShotBehavior behavior;

    public EnemyApi Api { get; set; }

	private Subject<Unit> destroySubject;

    public EnemyShot()
	{
		destroySubject = new Subject<Unit>();
		DestroyEvent = destroySubject;
        behavior = new NullEnemyShotBehavior(this);
    }

    public void NotifyDespawn()
	{
		behavior.Stop();
		destroySubject.OnNext(Unit.Default);
		destroySubject.OnCompleted();
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
