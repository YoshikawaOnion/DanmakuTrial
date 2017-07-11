using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

/// <summary>
/// 分身を出して戦う弾幕パターン。
/// </summary>
public class EnemyBehavior12 : EnemyBehavior
{
    private EnemyBehavior12Asset asset;

    public override IObservable<Unit> LoadAsset()
	{
		const string Name = "EnemyBehavior12";
		asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior12Asset>(Name);
		return DebugManager.I.LoadAssetFromServer<EnemyBehavior12AssetForJson, EnemyBehavior12Asset>(asset, Name);
    }

    protected override IObservable<Unit> GetAction()
    {
        return PrepareCoroutine().ToObservable()
                                 .SelectMany(t => ShotCoroutine().ToObservable());
    }

    private IEnumerator PrepareCoroutine()
    {
        int bodyCount = asset.BodyCount;
        float initialPositionAngleRange = asset.InitialPositionAngleRange;

        float initialPositionAngleSpan = initialPositionAngleRange / (bodyCount - 1);

        var locations = Enumerable.Range(0, bodyCount)
                                  .Select(i =>
        {
            var angle = i * initialPositionAngleSpan - initialPositionAngleRange / 2;
            var pos = Vector2Extensions.FromAngleLength(angle, asset.InitialPositionDisplacement);
            return pos + new Vector2(0, -10);
        })
                                  .ToArray();
		
        int locationIndex = (int)(UnityEngine.Random.value * bodyCount) % bodyCount;
		for (int i = 0; i < bodyCount; i++)
        {
            if (i != locationIndex)
            {
				var behavior = new ShootingEnemyShotBehavior()
                {
                    Way = asset.CopiesShotWay,
                    AngleSpan = asset.CopiesShotAngleSpan,
                    ShotSpeed = asset.CopiesShotSpeed,
                    ShotTimeSpan = asset.CopiesShotTimeSpan
				};
				var mob = Api.ShotMob(Api.Enemy.transform.position, 180, 0, behavior);
				behavior.InitializeComponent();
                behavior.Move(locations[i], 20);
			}
        }
        Api.MoveIt(locations[locationIndex], 20);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ShotCoroutine()
    {
        int shotLength = asset.BodyShotLength;
        float shotSpeed = asset.BodyShotSpeed;
        float shotTimeSpan = asset.BodyShotTimeSpan;
        while (true)
        {
            var angle = Api.GetAngleToPlayer(Api.Enemy.transform.position);
            for (int i = 0; i < shotLength; i++)
            {
                Api.Shot(angle, shotSpeed * Def.UnitPerPixel);
                yield return new WaitForSeconds(0.08f);
            }
            SoundManager.I.PlaySe(SeKind.EnemyShot);
            yield return new WaitForSeconds(shotTimeSpan);
        }
    }
}
