using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;
using UnityEditor;

public class EnemyBehavior6 : EnemyBehavior
{
    private List<CircleEnemyShotBehavior> shots;
    private float anglePivot;
    private EnemyBehavior6Asset asset;

    public EnemyBehavior6(EnemyApi api) : base(api)
    {
        shots = new List<CircleEnemyShotBehavior>();
    }

    protected override IObservable<Unit> GetAction()
    {
        asset = AssetDatabase.LoadAssetAtPath<EnemyBehavior6Asset>
                             ("Assets/Editor/EnemyBehavior6Asset.asset");
        var c0 = PrepareCoroutine().ToObservable();

        // 時間に沿って弾を回転させながら広がる
        var c1 = Observable.EveryUpdate()
                  .Select(t => Unit.Default)
                  .Do(t =>
        {
            anglePivot -= asset.AngleSpeed;
            foreach (var s in shots)
            {
                s.AnglePivot = anglePivot;
                s.Distance += asset.ShotSpeed;
            }
        });
        var c2 = ShotCoroutine().ToObservable();

        return c0.SelectMany(c1.Merge(c2));
    }

    private IEnumerator PrepareCoroutine()
    {
        var goal = asset.InitialPosition.transform.position;
        Api.Move(goal, 30);
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
			var shot = Api.Shot(0, 0);
            if (shot == null)
            {
                return;
            }
            var behavior = new CircleEnemyShotBehavior(shot, i, anglePivot, asset.Way);
			shot.behavior = behavior;
			shots.Add(behavior);
			shot.DestroyEvent.Subscribe(x => shots.Remove(behavior));
		}
    }
}
