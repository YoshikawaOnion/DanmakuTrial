using UnityEngine;
using System.Collections;
using System;
using UniRx;

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
                               .Take(50)
                               .Select(t => (float)t / 50)
							   .Select(t => -(t - 1) * (t - 1) + 1)
							   .Select(t => initialPos + offset.ToVector3() * t)
			                   .Do(p => Owner.transform.position = p)
			                   .ToYieldInstruction();
        
        yield return new WaitForSeconds(0.5f);

        rigidbody.velocity = Owner.Api.GetAngleToPlayer(Owner.transform.position).normalized * 100 * Def.UnitPerPixel;
    }
}