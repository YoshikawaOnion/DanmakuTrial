using UnityEngine;
using System.Collections;
using System;
using UniRx;

/// <summary>
/// 一定時間後に自機へ向きを変える弾を発射する弾幕パターン。
/// </summary>
public class EnemyBehavior7 : EnemyBehavior
{
    private EnemyBehavior7Asset asset;
    
    protected override IObservable<Unit> GetAction()
    {
        return ShotCoroutine().ToObservable();
    }

    private IEnumerator ShotCoroutine()
    {
        while (true)
        {
            Api.PlayShootSound();
            Shot();
			yield return new WaitForSeconds(0.1f);
			Shot();
			yield return new WaitForSeconds(asset.ShotTimeSpan);
        }
    }

    private void Shot()
    {
		var angle = UnityEngine.Random.value * 360;
		var behavior = new LockOnEnemyShotBehavior
        {
            Angle = angle
        };
        var shot = Api.Shot(angle, 1, behavior);
        if (shot == null)
        {
            return;
        }
    }

    public override IObservable<Unit> LoadAsset()
	{
		const string Name = "EnemyBehavior7";
		asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior7Asset>(Name);
		return DebugManager.I.LoadAssetFromServer<EnemyBehavior7AssetForJson, EnemyBehavior7Asset>(asset, Name);
    }
}
