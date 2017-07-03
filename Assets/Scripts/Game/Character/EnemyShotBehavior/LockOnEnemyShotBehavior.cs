using UnityEngine;
using System.Collections;
using System;
using UniRx;

/// <summary>
/// ランダムな地点まで飛んでからプレイヤーへ向けて向きを変えるよう弾を制御するクラス。
/// </summary>
public class LockOnEnemyShotBehavior : EnemyShotBehavior
{
    private float angle;

    public LockOnEnemyShotBehavior(EnemyShot owner, float angle) : base(owner)
    {
        this.angle = angle;
    }

    protected override IObservable<Unit> GetAction()
    {
        return Coroutine().ToObservable();
    }

    private IEnumerator Coroutine()
	{
		var rigidbody = Owner.GetComponent<Rigidbody2D>();
		var initialPos = Owner.transform.position;
		var radius = (50 + UnityEngine.Random.value * 100) * Def.UnitPerPixel;
		var offset = Vector2Extensions.FromAngleLength(angle, radius);
        rigidbody.velocity = Vector3.zero;

		yield return Observable.EveryUpdate()
                               .Take(20)
                               .Select(t => (float)t / 20)
							   .Select(t => -(t - 1) * (t - 1) + 1)
							   .Select(t => initialPos + offset.ToVector3() * t)
                               .TakeWhile(t => Owner != null)
			                   .Do(p => Owner.transform.position = p)
			                   .ToYieldInstruction();
        
        yield return new WaitForSeconds(0.5f);

        rigidbody.velocity = Owner.Api.GetAngleToPlayer(Owner.transform.position).normalized * 200 * Def.UnitPerPixel;
    }
}
