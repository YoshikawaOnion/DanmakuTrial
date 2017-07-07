using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyBehavior4 : EnemyBehavior
{
    private EnemyBehavior4Asset asset;

    public EnemyBehavior4(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
		asset = Resources.Load<EnemyBehavior4Asset>
						 ("ScriptableAsset/EnemyBehavior4Asset");
        var c1 = Coroutine().ToObservable();
        var c2 = SubShotCoroutine().ToObservable();
        return c1.Merge(c2);
    }

    private IEnumerator SubShotCoroutine()
    {
        var span = asset.FlowerShotSpan;
        var angle = 0.0f;
        while (true)
        {
            angle += span;
            Api.Shot(angle, asset.FlowerShotSpeed * Def.UnitPerPixel);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator Coroutine()
	{
        var angle = 180;
        var speed = asset.BeamSpeed * Def.UnitPerPixel;
        var offsets = Enumerable.Range(
            -(asset.BeamColumns/2),
            asset.BeamColumns)
								.Select(x => x * 2);
		var offsetSize = asset.BeamColumnOffset * Def.UnitPerPixel;
        while (true)
		{
            // プレイヤーを狙って移動
            for (int i = 0; i < asset.WaitTimeFrame; i++)
            {
                var ownerPos = Api.Enemy.transform.position;
                var playerPos = Player.gameObject.transform.position;
                Api.Enemy.transform.position = ownerPos.XReplacedBy((ownerPos.x * 4 + playerPos.x) / 5);
                yield return new WaitForSeconds(Time.deltaTime);
			}

			// レーザービーム
			Api.PlayShootSound();
			for (int j = 0; j < asset.BeamRows; j++)
			{
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
