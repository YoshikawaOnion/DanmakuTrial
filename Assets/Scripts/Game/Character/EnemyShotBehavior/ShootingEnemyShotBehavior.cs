using UnityEngine;
using System.Collections;
using System;
using UniRx;

/// <summary>
/// 自身も弾を撃つ弾の振る舞いを提供します。
/// </summary>
public class ShootingEnemyShotBehavior : EnemyShotBehavior
{
    public int Way = 2;
    public float AngleSpan = 30;
    public float ShotSpeed = 200;
    public float ShotTimeSpan = 0.5f;

    private IDisposable moveSubscription;
    private Rigidbody2D rigidBody;

    public ShootingEnemyShotBehavior(EnemyShot owner) : base(owner)
    {
    }

    public void InitializeComponent()
	{
		rigidBody = Owner.GetComponent<Rigidbody2D>();
    }

    protected override IObservable<Unit> GetAction()
    {
        return ShotCoroutine().ToObservable();
    }

    public void Move(Vector3 goal, int durationFrame)
    {
        if (moveSubscription != null)
        {
            moveSubscription.Dispose();
        }
        moveSubscription = Owner.Move(goal, durationFrame);
        Observable.EveryFixedUpdate()
                  .TakeUntil(Observable.TimerFrame(durationFrame))
                  .Subscribe(t =>
        {
            rigidBody.velocity = Vector3.zero;
        });
    }

    private IEnumerator ShotCoroutine()
    {
        while (Owner != null)
        {
            for (int i = 0; i < Way; i++)
            {
                float angleVariable = (i - (Way - 1.0f) / 2);
                float angle = 180 + angleVariable * AngleSpan;
                Owner.ShotFromIt(angle, ShotSpeed * Def.UnitPerPixel);
            }
            yield return new WaitForSeconds(ShotTimeSpan);
        }
    }
}
