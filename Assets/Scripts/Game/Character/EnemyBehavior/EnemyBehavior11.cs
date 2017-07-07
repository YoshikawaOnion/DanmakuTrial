using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 押しのけることのできるモブを含んだ壁を出す弾幕パターン。
/// </summary>
public class EnemyBehavior11 : EnemyBehavior
{
    private EnemyBehavior11Asset asset;

    public EnemyBehavior11(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        asset = Resources.Load<EnemyBehavior11Asset>
                         ("ScriptableAsset/EnemyBehavior11Asset");
        return ShotCoroutine().ToObservable();
    }

    private IEnumerator ShotCoroutine()
    {
        var width = GameManager.I.TopRight.x / asset.ShotSpan;
        while (true)
        {
            float mobPosition = (UnityEngine.Random.value * 2 - 1)
                * (width - asset.MobAreaMarginColumn);
            int mobIndex = (int)(mobPosition + width);

            WallShot(width, mobIndex);
            yield return new WaitForSeconds(0.05f);

            var mobPos = GetShotPos(width, mobIndex);
			Api.ShotMob(mobPos, 180, asset.ShotSpeed * Def.UnitPerPixel);

			WallShot(width, mobIndex);
            yield return new WaitForSeconds(asset.ShotTimeSpan);
        }
    }

    private void WallShot(float width, int mobIndex)
    {
        var w = width * 2;
        for (int i = 0; i < w; i++)
        {
            if (Mathf.Abs(i - mobIndex) < asset.HoleSizeColumn)
            {
                continue;
            }
            var position = GetShotPos(width, i);
            Api.Shot(position, 180, asset.ShotSpeed * Def.UnitPerPixel);
        }
        SoundManager.I.PlaySe(SeKind.EnemyShot);
    }

    private Vector3 GetShotPos(float width, int index)
    {
		var x = index - width;
		return new Vector3(x * asset.ShotSpan, GameManager.I.TopRight.y);
    }
}
