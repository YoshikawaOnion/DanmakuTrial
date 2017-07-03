using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Linq;

public class EnemyBehavior4 : EnemyBehavior
{
    public EnemyBehavior4(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        var c1 = Coroutine().ToObservable();
        var c2 = SubShotCoroutine().ToObservable();
        return c1.Merge(c2);
    }

    private IEnumerator SubShotCoroutine()
    {
        var span = 137.507764f;
        var angle = 0.0f;
        while (true)
        {
            angle += span;
            Api.Shot(angle, 180 * Def.UnitPerPixel);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator Coroutine()
	{
        var angle = 180;
        var speed = 420 * Def.UnitPerPixel;
        var offsets = Enumerable.Range(-1, 3).Select(x => x * 2);
        while (true)
		{
            // プレイヤーを狙って移動
            for (int i = 0; i < 30; i++)
            {
                var ownerPos = Api.Enemy.transform.position;
                var playerPos = Player.gameObject.transform.position;
                Api.Enemy.transform.position = ownerPos.XReplacedBy((ownerPos.x * 4 + playerPos.x) / 5);
                yield return new WaitForSeconds(Time.deltaTime);
			}

			// レーザービーム
			Api.PlayShootSound();
			for (int j = 0; j < 15; j++)
			{
				var offsetSize = 5 * Def.UnitPerPixel;
                foreach (var i in offsets)
                {
                    var offset = new Vector3(i * offsetSize, 0);
                    Api.ShotByOffset(offset, angle, speed);
                }
				yield return new WaitForSeconds(0.02f);
			}
            yield return new WaitForSeconds(0.5f);
        }
    }
}
