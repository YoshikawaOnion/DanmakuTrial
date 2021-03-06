using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 敵を中心に台風のように弾が回転する弾幕パターン。
/// </summary>
public class EnemyBehavior6 : EnemyBehavior
{
    private List<CircleEnemyShotBehavior> shots;
    private float anglePivot;
    private EnemyBehavior6Asset asset;

    protected override IObservable<Unit> GetAction()
	{
		shots = new List<CircleEnemyShotBehavior>();
        var c0 = PrepareCoroutine().ToObservable();

        // 時間に沿って弾を回転させながら広がる
        var c1 = Observable.EveryUpdate()
                  .Select(t => Unit.Default)
                  .Do(t =>
        {
            anglePivot -= asset.AngleSpeed;
            foreach (var s in shots)
            {
                s.SetLocation(anglePivot, asset.ShotSpeed);
            }
        });
        var c2 = ShotCoroutine().ToObservable();

        return c0.SelectMany(c1.Merge(c2));
    }

    private IEnumerator PrepareCoroutine()
    {
        var goal = asset.InitialPosition.transform.position;
        Api.MoveIt(goal, 30);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ShotCoroutine()
	{
        anglePivot = 45;
        while (true)
        {
            ShotCircle();
			yield return new WaitForSeconds(1.2f);
        }
    }

    private void ShotCircle()
	{
		Api.PlayShootSound();
        int way = asset.Way * asset.ChunkWay;
		for (int i = 0; i < way; i++)
		{
            var behavior = GameManager.I.PoolManager.GetInstance
                                  <CircleEnemyShotBehavior>(EnemyShotKind.Circle);
            behavior.Index = i;
            behavior.Way = asset.Way;
			var shot = Api.Shot(0, 0, behavior);
            if (shot == null)
            {
                return;
            }
			shots.Add(behavior);
			shot.DestroyEvent.Subscribe(x => 
            {
                shots.Remove(behavior);
            });
		}
    }

    public override IObservable<Unit> LoadAsset()
    {
        const string Name = "EnemyBehavior6";
        asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior6Asset>(Name);
        return DebugManager.I.LoadAssetFromServer<EnemyBehavior6AssetForJson, EnemyBehavior6Asset>(asset, Name);
    }
}
