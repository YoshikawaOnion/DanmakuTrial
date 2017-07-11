using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 特定の角度で時間に沿って射撃角を変えながら撃つ弾幕パターン。
/// </summary>
public class EnemyBehavior8 : EnemyBehavior
{
    private EnemyBehavior8Asset asset;

    public override IObservable<Unit> LoadAsset()
    {
        const string Name = "EnemyBehavior8";
        asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior8Asset>(Name);
        return DebugManager.I.LoadAssetFromServer<EnemyBehavior8AssetForJson, EnemyBehavior8Asset>(asset, Name);
    }

    protected override IObservable<Unit> GetAction()
    {
        return ShotCoroutine().ToObservable();
    }

    private IEnumerator ShotCoroutine()
    {
        float angle = 0;
        float angleSpan = asset.AngleSpan;
        while (true)
        {
            Api.Shot(angle, asset.ShotSpeed * Def.UnitPerPixel);
            angle += angleSpan;
            yield return new WaitForSeconds(asset.TimeSpan);
        }
    }
}
