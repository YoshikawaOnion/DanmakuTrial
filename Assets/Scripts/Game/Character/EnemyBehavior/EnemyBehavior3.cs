using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyBehavior3 : EnemyBehavior
{
    private EnemyBehavior3Asset asset;

    public EnemyBehavior3(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
		asset = Resources.Load<EnemyBehavior3Asset>
						 ("ScriptableAsset/EnemyBehavior3Asset");
        return Act().ToObservable();
    }

    private IEnumerator Act()
	{
		var span = 360.0f / asset.Way;
        float time = 0;
        float angleCenter = 0;
        while (true)
		{
            var d = time * Mathf.PI * 2 * asset.Frequency;
            angleCenter = Mathf.Sin(d) * asset.Amplitude;
			for (int i = 0; i < asset.Way; i++)
			{
				if (Mathf.Abs(i - asset.Way / 2) > asset.HoleSize)
				{
					var angle = 180 + angleCenter - (i - asset.Way / 2) * span;
					Api.Shot(angle, asset.Speed * Def.UnitPerPixel);
				}
			}
            yield return new WaitForSeconds(0.15f);
            time += 0.15f;
        }
    }
}
