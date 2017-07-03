using UnityEngine;
using System.Collections;
using System;
using UniRx;

/// <summary>
/// 何も特別な振る舞いはしない弾の制御クラス。これを用いることで弾は単に直線状に飛んでいきます。
/// </summary>
public class NullEnemyShotBehavior : EnemyShotBehavior
{
    public NullEnemyShotBehavior(EnemyShot owner) : base(owner)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        return Observable.Return(Unit.Default);
    }
}
