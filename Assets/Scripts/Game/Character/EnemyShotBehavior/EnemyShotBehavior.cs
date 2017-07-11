using UnityEngine;
using System.Collections;
using UniRx;
using System;

/// <summary>
/// 弾に特別な振る舞いをさせる制御クラスの基底クラスです。
/// </summary>
public abstract class EnemyShotBehavior
{
    protected EnemyShot Owner { get; set; }
    public bool IsActive { get; set; }

    private IDisposable actionSubscription;

    public virtual void Initialize(EnemyShot shot)
    {
        Owner = shot;
    }

    public virtual void Reset()
    {
        GameManager.I.PoolManager.SleepInstance(this);
    }

    /// <summary>
    /// 弾の振る舞いを取得します。
    /// </summary>
    /// <returns>弾の振る舞いを実行するコルーチンを制御するストリーム。完了時に値を発行します。</returns>
    protected abstract IObservable<Unit> GetAction();

    /// <summary>
    /// この振る舞いを実行します。
    /// </summary>
    public void Start()
    {
        actionSubscription = GetAction().Subscribe();
    }

    /// <summary>
    /// この振る舞いを終了します。
    /// </summary>
    public void Stop()
    {
        if (actionSubscription != null)
        {
            actionSubscription.Dispose();
        }
    }
}
