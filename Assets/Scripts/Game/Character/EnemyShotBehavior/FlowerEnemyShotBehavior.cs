using UnityEngine;
using System.Collections;
using System;
using UniRx;

/// <summary>
/// 弾から弾を発射させるクラス。
/// </summary>
public class FlowerEnemyShotBehavior : EnemyShotBehavior
{
    public int Way { get; set; }
    public float Speed { get; set; }
    public float TimeSpan { get; set; }
    public float Angle { get; set; }

    public FlowerEnemyShotBehavior(EnemyShot owner) : base(owner)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        return Coroutine().ToObservable();
    }

    private IEnumerator Coroutine()
    {
        while (true)
        {
            var source = Owner.gameObject.transform.position;
            for (int i = 0; i < Way; i++)
            {
                var angle = Angle + i * (360 / Way);
                Owner.Api.Shot(source, angle, Speed * Def.UnitPerPixel);
            }
            yield return new WaitForSeconds(TimeSpan);
        }
    }
}
