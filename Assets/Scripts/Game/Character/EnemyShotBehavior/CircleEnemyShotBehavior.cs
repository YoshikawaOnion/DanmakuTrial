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
    public float AnglePivot { get; set; }
    public float Distance { get; set; }

    private int index;
    private int way;

    public CircleEnemyShotBehavior(int index, float angle, int way)
    {
        AnglePivot = angle;
        Distance = 0;
        this.index = index;
        this.way = way;
    }

    protected override IObservable<Unit> GetAction()
	{
        var center = Owner.Api.Enemy.transform.position;

        return Observable.EveryUpdate()
                         .Select(t => Unit.Default)
                         .Do(t =>
        {
            // 角度は1周分から少しずらして少しづつずれるように
            var angle = AnglePivot + index * (360 / way + 2);
			var x = Mathf.Cos(angle * Mathf.Deg2Rad) * Distance;
			var y = Mathf.Sin(angle * Mathf.Deg2Rad) * Distance;
            Owner.transform.position = center + new Vector3(x, y, 10);
        });
    }
}
