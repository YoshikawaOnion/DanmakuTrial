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
    private float AnglePivot { get; set; }
    private float Distance { get; set; }

    public int Index { get; set; }
    public int Way { get; set; }

    public override void Initialize(EnemyShot shot)
    {
        base.Initialize(shot);
        Distance = 0;
    }

    public void SetLocation(float anglePivot, float velocity)
    {
        AnglePivot = anglePivot;
        Distance += velocity;
    }

    protected override IObservable<Unit> GetAction()
	{
        var center = Owner.Api.Enemy.transform.position;

        return Observable.EveryUpdate()
                         .Select(t => Unit.Default)
                         .Do(t =>
        {
            // 角度は1周分から少しずらして少しづつずれるように
            var angle = AnglePivot + Index * (360 / Way + 2);
			var x = Mathf.Cos(angle * Mathf.Deg2Rad) * Distance;
			var y = Mathf.Sin(angle * Mathf.Deg2Rad) * Distance;
            Owner.transform.position = center + new Vector3(x, y, 10);
        });
    }
}
