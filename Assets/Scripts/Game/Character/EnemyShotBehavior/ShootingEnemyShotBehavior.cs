using UnityEngine;
using System.Collections;
using System;
using UniRx;

/// <summary>
/// 自身も弾を撃つ弾の振る舞いを提供します。
/// </summary>
public class ShootingEnemyShotBehavior : EnemyShotBehavior
{
    public int Way { get; set; }
    public float AngleSpan { get; set; }
    public float ShotSpeed { get; set; }
    public float ShotTimeSpan { get; set; }

    private IDisposable moveSubscription;
    private Rigidbody2D rigidBody;


    public void InitializeComponent()
	{
		rigidBody = Owner.GetComponent<Rigidbody2D>();
    }

    public override void Initialize(EnemyShot shot)
    {
		base.Initialize(shot);
		rigidBody = Owner.GetComponent<Rigidbody2D>();
        Way = 2;
        AngleSpan = 30;
        ShotSpeed = 200;
        ShotTimeSpan = 0.5f;
        moveSubscription = null;
    }

    public override void Reset()
    {
        if (moveSubscription != null)
        {
            moveSubscription.Dispose();
        }
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
