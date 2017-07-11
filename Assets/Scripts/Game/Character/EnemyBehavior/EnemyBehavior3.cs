using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 大量の弾幕に空けられた道を進んでいく弾幕パターン。
/// </summary>
public class EnemyBehavior3 : EnemyBehavior
{
    private EnemyBehavior3Asset asset;

    public override IObservable<Unit> LoadAsset()
    {
        const string Name = "EnemyBehavior3";
        asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior3Asset>(Name);
        return DebugManager.I.LoadAssetFromServer<EnemyBehavior3AssetForJson, EnemyBehavior3Asset>(asset, Name);
    }

    protected override IObservable<Unit> GetAction()
    {
        return Act().ToObservable();
    }

    private IEnumerator Act()
	{
		var span = 360.0f / asset.Way;
        float time = 0;
        float angleCenter = 0;
        while (true)
		{
            var d = time * Mathf.PI * 2 * asset.Frequency;
            angleCenter = Mathf.Sin(d) * asset.Amplitude;
			for (int i = 0; i < asset.Way; i++)
			{
				if (Mathf.Abs(i - asset.Way / 2) > asset.HoleSize)
				{
					var angle = 180 + angleCenter - (i - asset.Way / 2) * span;
					Api.Shot(angle, asset.Speed * Def.UnitPerPixel);
				}
			}
            yield return new WaitForSeconds(0.15f);
            time += 0.15f;
        }
    }
}
