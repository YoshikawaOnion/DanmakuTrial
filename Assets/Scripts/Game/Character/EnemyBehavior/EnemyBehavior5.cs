using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyBehavior5 : EnemyBehavior
{
    private EnemyBehavior5Asset asset;

    public EnemyBehavior5(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        var c1 = ShotCoroutine().ToObservable();
        var c2 = MoveObservable();
        return c1.Merge(c2);
    }

    private IEnumerator ShotCoroutine()
    {
        float subAngle = 0;
        while (true)
        {
            ShotFlower(asset.ShotAngle, subAngle);
            ShotFlower(-asset.ShotAngle, subAngle);
            Api.PlayShootSound();
            subAngle += asset.FlowerAngleRotation;
            yield return new WaitForSeconds(asset.MainShotTimeSpan);
        }
    }

    private void ShotFlower(float angle, float subAngle)
    {
        var shot = Api.Shot(angle, asset.FlowerShotSpeed * Def.UnitPerPixel);
		var behavior = new FlowerEnemyShotBehavior(shot);
        behavior.Way = asset.FlowerShotWay;
        behavior.Speed = asset.FlowerShotSpeed;
        behavior.TimeSpan = asset.FlowerShotTimeSpan;
        behavior.Angle = subAngle;
        shot.behavior = behavior;
    }

    private IObservable<Unit> MoveObservable()
    {
        // 3秒間踏ん張る
        return Observable.EveryUpdate()
                         .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(3)))
                         .Do(t =>
        {
            Api.Enemy.Rigidbody.AddForce(new Vector2(0, asset.GutsForce * Def.UnitPerPixel));
        })
                         .Select(t => Unit.Default);
    }
}
