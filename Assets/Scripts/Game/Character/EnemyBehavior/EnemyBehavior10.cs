using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 花状の弾幕パターン。
/// </summary>
public class EnemyBehavior10 : EnemyBehavior
{
    private EnemyBehavior10Asset asset;

    public override IObservable<Unit> LoadAsset()
	{
		const string Name = "EnemyBehavior10";
		asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior10Asset>(Name);
		return DebugManager.I.LoadAssetFromServer<EnemyBehavior10AssetForJson, EnemyBehavior10Asset>(asset, Name);
    }

    protected override IObservable<Unit> GetAction()
    {
        return ShotCoroutine().ToObservable();
    }

    private IEnumerator ShotCoroutine()
    {
        float angleBase = 0;
        while (true)
        {
            for (int i = 0; i < asset.Way; i++)
            {
                float angle = angleBase + i * 10;

                //花状弾幕を形成する計算式
                float theta = Mathf.Sin(i * 10 * Mathf.Deg2Rad * 2);

                float speed = theta * asset.MaxSpeed;

                //弾が残らないように一定以下の速さのものは撃たない
                if (Math.Abs(speed) < 20)
                {
                    continue;
                }

                Api.Shot(angle, speed * Def.UnitPerPixel);
            }
            angleBase += asset.AngleAdvance;
            if (angleBase > 360)
            {
                angleBase -= 360;
            }
            SoundManager.I.PlaySe(SeKind.EnemyShot);
            yield return new WaitForSeconds(1);
        }
    }
}
