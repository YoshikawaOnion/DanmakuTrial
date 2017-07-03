using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;

public class EnemyBehavior2 : EnemyBehavior
{
    private static readonly int Way = 8;

    private Coroutine coroutine1;
    private Coroutine coroutine2;
    private IDisposable actionSubscription;

    public EnemyBehavior2(EnemyApi api) : base(api)
    {
    }

    public IEnumerator Act2()
    {
        while (true)
        {
            var direction = GameManager.I.Player.transform.position
                                   - Api.Enemy.transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x)
                             * Mathf.Rad2Deg;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    var angleOffset = 0;
                    var speed = 240 + i * 15;
                    Api.Shot(90 - angle - angleOffset, speed * Def.UnitPerPixel);
                }
            }

            yield return new WaitForSeconds(0.8f);
        }
    }

    public IEnumerator Act1()
    {
        while (true)
        {
            for (int n = 0; n < 2; n++)
			{
				for (int i = 0; i < Way * 2 + 1; i++)
				{
					int span = 33 * (n == 0 ? -1 : 1);
					var angle = (i - Way) * span;
					for (int j = 0; j < 8; j++)
					{
						Api.Shot(angle, (200 + j * 40) * Def.UnitPerPixel);
					}
				}
                Api.PlayShootSound();
				yield return new WaitForSeconds(1.2f);
            }
        }
    }

    protected override IObservable<Unit> GetAction()
	{
		var c1 = Act1().ToObservable();
		var c2 = Act2().ToObservable();
		return c1.Merge(c2);
    }
}
