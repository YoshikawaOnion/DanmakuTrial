using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEditor;

public class EnemyBehavior13 : EnemyBehavior
{
    private EnemyBehavior13Asset asset;

    protected override IObservable<Unit> GetAction()
    {
        return ShotCoroutine().ToObservable();
    }

    private IEnumerator ShotCoroutine()
    {
        while(true)
        {
            var x = (UnityEngine.Random.value * 1.5f - 0.75f) * Def.WorldXMax;
            var y = Def.WorldYMax * 3 / 4;
            var num = UnityEngine.Random.value * 8 + 4;
            var behavior = GameManager.I.PoolManager.GetInstance<RingEnemyShotBehavior>
                                      (EnemyShotKind.Ring);
            behavior.BulletNum = (int)num;
            behavior.RotateSpeed = 0.3f;
            var shot = Api.Shot(new Vector2(0, 0), Def.DownAngle, 180 * Def.UnitPerPixel, behavior);
            //shot.IsVisible = false;
            yield return new WaitForSeconds(1.2f);
        }
    }

    public override IObservable<Unit> LoadAsset()
    {
        const string Name = "EnemyBehavior13";
        asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior13Asset>(Name);
        return DebugManager.I.LoadAssetFromServer<EnemyBehavior13AssetForJson, EnemyBehavior13Asset>(asset, Name);
    }
}
