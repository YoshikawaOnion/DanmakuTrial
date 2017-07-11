using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 左右の手から交互に塊を発射する弾幕パターン。
/// </summary>
public class EnemyBehavior1 : EnemyBehavior
{
    private EnemyBehavior1Asset asset;

    public IEnumerator Act()
	{
        while (true)
        {
            ShotChunk(Api.Enemy.LeftHand.transform.localPosition);
			yield return WaitRandomly(asset.ShotTimeSpan);

			ShotChunk(Api.Enemy.RightHand.transform.localPosition);
			yield return WaitRandomly(asset.ShotTimeSpan);
		}
    }

    protected override IObservable<Unit> GetAction()
    {
        return Act().ToObservable();
    }

    private WaitForSeconds WaitRandomly(float pivotWait)
    {
        var delta = UnityEngine.Random.value * 0.5f - 0.25f;
        return new WaitForSeconds(pivotWait + delta);
    }

    private void ShotChunk(Vector3 localPosition)
    {
        var sourcePos = localPosition.Mul(Api.Enemy.transform.lossyScale);
        for (int i = 0; i < asset.ColumnsInChunk; i++)
        {
            for (int j = 0; j < asset.RowsInChunk; j++)
			{
                var offset = new Vector3(
                    i - asset.ColumnsInChunk / 2,
                    j - asset.RowsInChunk / 2,
                    0) * asset.OffsetSize;
                var offsetRandom = UnityEngine.Random.insideUnitCircle
                                              .ToVector3()
											   * asset.RandomOffsetSize;
                var pixelOffset = offset + offsetRandom;
                var position = sourcePos + pixelOffset * Def.UnitPerPixel;
				Api.ShotByOffset(position, 180, asset.Speed * Def.UnitPerPixel);
            }
        }
        Api.PlayShootSound();
    }

    public override IObservable<Unit> LoadAsset()
    {
        asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior1Asset>("EnemyBehavior1");
        return DebugManager.I.LoadAssetFromServer<EnemyBehavior1AssetForJson, EnemyBehavior1Asset>(asset, "EnemyBehavior1");
    }
}
