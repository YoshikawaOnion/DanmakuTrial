using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEditor;

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
        asset = AssetDatabase.LoadAssetAtPath<EnemyBehavior8Asset>
                             ("Assets/Editor/EnemyBehavior8Asset.asset");
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
