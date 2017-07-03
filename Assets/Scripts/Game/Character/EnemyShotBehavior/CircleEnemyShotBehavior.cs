using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;

/// <summary>
/// 敵を中心に回転しながら離れていくように弾を制御するクラス。
/// </summary>
public class CircleEnemyShotBehavior : EnemyShotBehavior
{
    public static readonly int Way = 4;

    internal float AnglePivot { get; set; }
    internal float Distance { get; set; }

    private int index;

    public CircleEnemyShotBehavior(EnemyShot owner, int index, float angle) : base(owner)
    {
        AnglePivot = angle;
        Distance = 0;
        this.index = index;
    }

    protected override IObservable<Unit> GetAction()
	{
		var center = Owner.Api.Enemy.transform.position;

        return Observable.EveryUpdate()
                         .Select(t => Unit.Default)
                         .Do(t =>
        {
            var angle = AnglePivot + index * (360 / Way + 2);
			var x = Mathf.Cos(angle * Mathf.Deg2Rad) * Distance;
			var y = Mathf.Sin(angle * Mathf.Deg2Rad) * Distance;
            Owner.transform.position = center + new Vector3(x, y, 10);
        });
    }
}
