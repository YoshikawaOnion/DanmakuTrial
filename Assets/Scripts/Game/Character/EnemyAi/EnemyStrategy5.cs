using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyStrategy5 : EnemyStrategy
{
    public EnemyStrategy5(Enemy owner) : base(owner)
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
            ShotFlower(90, subAngle);
            ShotFlower(-90, subAngle);
            Owner.PlayShotSound();
            subAngle += 13;
            yield return new WaitForSeconds(2.5f);
        }
    }

    private void ShotFlower(float angle, float subAngle)
    {
        var shot = Owner.Shot(angle, 150 * Def.UnitPerPixel);
		var behavior = new FlowerEnemyShotBehavior(shot);
        behavior.Way = 12;
        behavior.Speed = 240;
        behavior.TimeSpan = 0.4f;
        behavior.Angle = subAngle;
        shot.behavior = behavior;
    }

    private IObservable<Unit> MoveObservable()
    {
        return Observable.EveryUpdate()
                         .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(3)))
                         .Do(t =>
        {
            Owner.rigidbody.AddForce(new Vector2(0, -9 * Def.UnitPerPixel));
        })
                         .Select(t => Unit.Default);
    }
}
