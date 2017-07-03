using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyBehavior3 : EnemyBehavior
{
    private static readonly int Way = 72;
    private static readonly int HoleSize = 4;
    private static readonly float Frequency = 1.0f / 8;
    private static readonly float Amplitude = 30;

    public EnemyBehavior3(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        return Act().ToObservable();
    }

    private IEnumerator Act()
	{
		var span = 360.0f / Way;
        float time = 0;
        float angleCenter = 0;
        while (true)
		{
            angleCenter = Mathf.Sin(time * Mathf.PI * 2 * Frequency) * Amplitude;
			for (int i = 0; i < Way; i++)
			{
				if (Mathf.Abs(i - Way / 2) > HoleSize)
				{
					var angle = 180 + angleCenter - (i - Way / 2) * span;
					Api.Shot(angle, 120 * Def.UnitPerPixel);
				}
			}
            yield return new WaitForSeconds(0.15f);
            time += 0.15f;
        }
    }
}
