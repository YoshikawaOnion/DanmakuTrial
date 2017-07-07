using UnityEngine;
using System.Collections;
using System;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyBehavior10 : EnemyBehavior
{
    private EnemyBehavior10Asset asset;

    public EnemyBehavior10(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        asset = Resources.Load<EnemyBehavior10Asset>
                         ("ScriptableAsset/EnemyBehavior10Asset");
        if (asset == null)
        {
            asset = new EnemyBehavior10Asset()
            {
                Way = 36,
                MaxSpeed = 280,
                AngleAdvance = 137.5f,
            };
        }
        return ShotCoroutine().ToObservable();
    }

    private IEnumerator ShotCoroutine()
    {
        float angleBase = 0;
        while (true)
        {
            for (int i = 0; i < 36; i++)
            {
                float angle = angleBase + i * 10;
                float theta = Mathf.Sin(i * 10 * Mathf.Deg2Rad * 2);
                float speed = theta * 280;
                if (Math.Abs(speed) < 20)
                {
                    continue;
                }
                Api.Shot(angle, speed * Def.UnitPerPixel);
            }
            angleBase += 137;
            if (angleBase > 360)
            {
                angleBase -= 360;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
