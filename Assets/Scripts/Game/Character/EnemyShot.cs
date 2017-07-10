using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵のショットを表すクラス。
/// </summary>
public class EnemyShot : MonoBehaviour {
    public IObservable<Unit> DestroyEvent;
	private EnemyShotBehavior Behavior;

    public EnemyApi Api { get; private set; }

	private Subject<Unit> destroySubject;
    private BulletPoolManager poolManager;

    public EnemyShot()
	{
		destroySubject = new Subject<Unit>();
		DestroyEvent = destroySubject;
        Behavior = new NullEnemyShotBehavior();
    }

	public void InitializeBullet(BulletPoolManager poolManager, EnemyShotBehavior behavior, EnemyApi api)
	{
		this.poolManager = poolManager;
        this.Behavior = behavior;
        this.Api = api;
        Behavior.Initialize(this);
        Behavior.Start();
	}

    public void ResetBullet()
    {
        Behavior.Stop();
		poolManager.SleepInstance(gameObject);
		destroySubject.OnNext(Unit.Default);
    }

    private void OnDestroy()
    {
        Behavior.Stop();
        destroySubject.OnNext(Unit.Default);
        destroySubject.OnCompleted();
        destroySubject = null;
        Behavior = null;
    }

    public void ShotFromIt(float angle, float speed)
    {
        Api.Shot(transform.position, angle, speed);
    }
}
