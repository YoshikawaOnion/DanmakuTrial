using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// やたらと背後に弾が出る弾幕パターン。
/// </summary>
public class EnemyBehavior9 : EnemyBehavior
{
    private EnemyBehavior9Asset asset;

    protected override IObservable<Unit> GetAction()
    {
        var c0 = JetShotCoroutine().ToObservable();
        var c1 = AimShotCoroutine().ToObservable();
        return c0.Merge(c1);
    }

    private IEnumerator JetShotCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < asset.BackwardShotWay; i++)
            {
                float angle = (i - asset.BackwardShotWay / 2) * 20;
                Api.Shot(angle, asset.BackwardShotSpeed * Def.UnitPerPixel);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator AimShotCoroutine()
    {
        while (true)
        {
            var angle = Api.GetAngleToPlayer(Api.Enemy.transform.position);
            Api.Shot(angle, asset.AimShotSpeed * Def.UnitPerPixel);
            yield return new WaitForSeconds(asset.AimShotTimeSpan);
        }
    }

    public override IObservable<Unit> LoadAsset()
    {
        const string Name = "EnemyBehavior9";
        asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior9Asset>(Name);
        return DebugManager.I.LoadAssetFromServer<EnemyBehavior9AssetForJson, EnemyBehavior9Asset>(asset, Name);
    }
}
