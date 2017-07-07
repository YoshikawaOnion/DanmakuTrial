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

    public EnemyBehavior8(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        return ShotCoroutine().ToObservable();
    }

    private IEnumerator ShotCoroutine()
    {
        asset = Resources.Load<EnemyBehavior8Asset>
                         ("ScriptableAsset/EnemyBehavior8Asset");
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
