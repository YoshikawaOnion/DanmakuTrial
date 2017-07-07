using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyBehavior1 : EnemyBehavior
{
    private EnemyBehavior1Asset asset;

    public EnemyBehavior1(EnemyApi api) : base(api)
    {
    }

    public IEnumerator Act()
	{
        asset = Resources.Load<EnemyBehavior1Asset>
                         ("ScriptableAsset/EnemyBehavior1Asset");
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
}
