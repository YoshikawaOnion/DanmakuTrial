using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;

/// <summary>
/// 付属する弾が回転するリング上を回転するように制御する挙動を提供します。
/// </summary>
public class RingEnemyShotBehavior : EnemyShotBehavior
{
    public int BulletNum { get; set; }
    public float RotateSpeed { get; set; }

    private EnemyShot[] Shots { get; set; }
    private CompositeDisposable Disposable { get; set; }

    protected override IObservable<Unit> GetAction()
    {
        return ControlCoroutine().ToObservable();
    }

    public override void Initialize(EnemyShot shot)
    {
        base.Initialize(shot);
        Debug.Log("Ring Initialize.");
    }

    public override void Reset()
    {
        for (int i = 0; i < BulletNum; i++)
		{
			if (Shots[i] != null)
			{
                Debug.DrawLine(Owner.transform.position, Shots[i].transform.position, new Color(0, 1, 1), 1);
				Shots[i].ResetBullet();
			}
        }
		Shots = null;
		base.Reset();
        Disposable.Dispose();
		Debug.Log("Ring Reset.");
    }

    private IEnumerator ControlCoroutine()
    {
        Disposable = new CompositeDisposable();

        var source = Owner.transform.position;
        Shots = new EnemyShot[BulletNum];
        var span = 360.0f / BulletNum;
        var radius = (BulletNum * 3) + 8;

        for (int i = 0; i < BulletNum; i++)
        {
            int j = i;
            var offset = Vector2Extensions.FromAngleLength(i * span, radius);
            var shot = Owner.Api.Shot(source + offset.ToVector3(), 0, 0);
            Shots[i] = shot;
            // TODO: ここで全要素にnullが代入されるラムダ式ができているのかも・・・
            shot.DestroyEvent.Subscribe(u => Shots[j] = null)
                .AddTo(Disposable);
        }

        float angle = 0;
        while (true)
        {
            var origin = Owner.transform.position;
            for (int i = 0; i < BulletNum; i++)
			{
                if (Shots[i] != null)
				{
					var offset = Vector2Extensions.FromAngleLength(angle + i * span, radius);
					Shots[i].transform.position = origin + offset.ToVector3();
                }
            }
            for (int i = 0; i < Shots.Length; i++)
            {
                Debug.DrawLine(origin, Shots[i].transform.position, new Color(0, 0, 1));
            }
            angle += RotateSpeed;
            yield return null;
        }
    }
}
