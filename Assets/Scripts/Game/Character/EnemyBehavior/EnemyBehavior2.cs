using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 花火攻撃と自機狙い攻撃を同時に行う弾幕パターン。
/// </summary>
public class EnemyBehavior2 : EnemyBehavior
{
    private EnemyBehavior2Asset asset;

    private IEnumerator PointShotCoroutine()
    {
        while (true)
        {
            var direction = GameManager.I.Player.transform.position
                                   - Api.Enemy.transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x)
                             * Mathf.Rad2Deg;

            for (int i = 0; i < asset.AimShotRows; i++)
            {
                for (int j = 0; j < asset.AimShotColumns; j++)
                {
                    var angleOffset = 0;
                    var speed = 240 + i * 15;
                    Api.Shot(90 - angle - angleOffset, speed * Def.UnitPerPixel);
                }
            }

            yield return new WaitForSeconds(0.8f);
        }
    }

    private IEnumerator FlowerShotCoroutine()
    {
        while (true)
        {
            for (int n = 0; n < 2; n++)
			{
				for (int i = 0; i < asset.Way * 2 + 1; i++)
                {
                    FlowerShot(n, i);
                }
                Api.PlayShootSound();
				yield return new WaitForSeconds(asset.ShotTimeSpan);
            }
        }
    }

    private void FlowerShot(int n, int i)
    {
        var span = asset.ShotSpan * (n == 0 ? -1 : 1);
        var angle = (i - asset.Way) * span;
        for (int j = 0; j < asset.FlowerShotRows; j++)
        {
            var speed = asset.FlowerSlowestSpeed + j * 40;
            Api.Shot(angle, speed * Def.UnitPerPixel);
        }
    }

    protected override IObservable<Unit> GetAction()
	{
		var c1 = FlowerShotCoroutine().ToObservable();
		var c2 = PointShotCoroutine().ToObservable();
		return c1.Merge(c2);
    }

    public override IObservable<Unit> LoadAsset()
    {
        asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior2Asset>("EnemyBehavior2");
        return DebugManager.I.LoadAssetFromServer<EnemyBehavior2AssetForJson, EnemyBehavior2Asset>(asset, "EnemyBehavior2");
    }
}
