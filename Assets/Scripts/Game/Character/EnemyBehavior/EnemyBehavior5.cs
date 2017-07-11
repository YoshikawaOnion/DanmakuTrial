using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 弾自身が弾を発射する弾で攻撃する弾幕パターン。
/// </summary>
public class EnemyBehavior5 : EnemyBehavior
{
    private EnemyBehavior5Asset asset_;
    private EnemyBehavior5Asset asset
    {
        get
		{
			Debug.Log("Asset get with " + (asset_ != null ? asset_.ToString() : "Null"));
            return asset_;
        }
        set
        {
            asset_ = value;
            Debug.Log("Asset set with " + (asset_ != null ? asset_.ToString() : "Null"));
        }
    }

    protected override IObservable<Unit> GetAction()
    {
        var c1 = ShotCoroutine().ToObservable();
        var c2 = MoveObservable();
        return c1.Merge(c2);
    }

    private IEnumerator ShotCoroutine()
    {
        float subAngle = 0;
        while (true)
        {
            ShotFlower(asset.ShotAngle, subAngle);
            ShotFlower(-asset.ShotAngle, subAngle);
            Api.PlayShootSound();
            subAngle += asset.FlowerAngleRotation;
            yield return new WaitForSeconds(asset.MainShotTimeSpan);
        }
    }

    private void ShotFlower(float angle, float subAngle)
    {
        var behavior = GameManager.I.PoolManager.GetInstance<FlowerEnemyShotBehavior>(EnemyShotKind.Flower);
        behavior.Way = asset.FlowerShotWay;
        behavior.Speed = asset.FlowerShotSpeed;
        behavior.TimeSpan = asset.FlowerShotTimeSpan;
        behavior.Angle = subAngle;

        var shot = Api.Shot(angle, asset.FlowerShotSpeed * Def.UnitPerPixel, behavior);
        if (shot == null)
        {
            return;
        }
    }

    private IObservable<Unit> MoveObservable()
    {
        // 3秒間踏ん張る
        return Observable.EveryUpdate()
                         .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(3)))
                         .Do(t =>
        {
            Api.Enemy.Rigidbody.AddForce(new Vector2(0, asset.GutsForce * Def.UnitPerPixel));
        })
                         .Select(t => Unit.Default);
    }

    public override IObservable<Unit> LoadAsset()
    {
        const string Name = "EnemyBehavior5";
        asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior5Asset>(Name);
        return DebugManager.I.LoadAssetFromServer<EnemyBehavior5AssetForJson, EnemyBehavior5Asset>(asset, Name);
    }
}
