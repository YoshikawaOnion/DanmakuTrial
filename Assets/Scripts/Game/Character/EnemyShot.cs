﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵のショットを表すクラス。
/// </summary>
public class EnemyShot : MonoBehaviour {
    public IObservable<Unit> DestroyEvent;
	public EnemyShotBehavior Behavior;

    public EnemyApi Api { get; set; }

	private Subject<Unit> destroySubject;
    private BulletPoolManager poolManager;

    public EnemyShot()
	{
		destroySubject = new Subject<Unit>();
		DestroyEvent = destroySubject;
        Behavior = new NullEnemyShotBehavior(this);
    }

    public void NotifyDespawn()
	{
		Behavior.Stop();
		destroySubject.OnNext(Unit.Default);
		destroySubject.OnCompleted();
	}

	public void InitializeBullet(BulletPoolManager poolManager)
	{
		this.poolManager = poolManager;
	}

    public void ResetBullet()
    {
        Behavior.Stop();
		poolManager.SleepInstance(gameObject);
		destroySubject.OnNext(Unit.Default);
    }

    private void Start()
    {
        Behavior.Start();
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
