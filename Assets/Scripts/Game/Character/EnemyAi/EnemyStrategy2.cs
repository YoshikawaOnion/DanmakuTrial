using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;

public class EnemyStrategy2 : EnemyStrategy
{
    private static readonly int Way = 8;

    private Coroutine coroutine1;
    private Coroutine coroutine2;

    public EnemyStrategy2(Enemy owner) : base(owner)
    {
    }

    public IEnumerator Act2()
    {
        while (true)
        {
            var direction = GameManager.I.Player.transform.position
                                   - Owner.transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x)
                             * Mathf.Rad2Deg;
			Owner.Shot(90 - angle, 240 * Def.UnitPerPixel);
			Owner.Shot(90 - angle - 2, 240 * Def.UnitPerPixel);
			Owner.Shot(90 - angle + 2, 240 * Def.UnitPerPixel);
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
                    int span = 36 - n * 80;
					if (Mathf.Abs(i - Way) > 0)
					{
						var angle = 180 - (i - Way) * span;
						Owner.Shot(angle, 200 * Def.UnitPerPixel);
						Owner.Shot(angle, 240 * Def.UnitPerPixel);
						Owner.Shot(angle, 280 * Def.UnitPerPixel);
						Owner.Shot(angle, 320 * Def.UnitPerPixel);
						Owner.Shot(angle, 360 * Def.UnitPerPixel);
						Owner.Shot(angle, 400 * Def.UnitPerPixel);
						Owner.Shot(angle, 480 * Def.UnitPerPixel);
					}
				}
                Owner.AudioSource.PlayOneShot(Owner.ShootSound);
				yield return new WaitForSeconds(2);
            }
        }
    }

    public override IEnumerator Act()
    {
        coroutine1 = Owner.StartCoroutine(Act1());
        coroutine2 = Owner.StartCoroutine(Act2());
        yield return null;
    }

    public override void OnDestroy()
    {
        Owner.StopCoroutine(coroutine1);
        Owner.StopCoroutine(coroutine2);
    }
}
