using UnityEngine;
using System.Collections;
using System;
using UniRx;

/// <summary>
/// 一定時間後に自機へ向きを変える弾を発射する弾幕パターン。
/// </summary>
public class EnemyBehavior7 : EnemyBehavior
{
    public EnemyBehavior7(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        return ShotCoroutine().ToObservable();
    }

    private IEnumerator ShotCoroutine()
    {
        while (true)
        {
            Api.PlayShootSound();
            Shot();
			yield return new WaitForSeconds(0.1f);
			Shot();
			yield return new WaitForSeconds(0.8f);
        }
    }

    private void Shot()
    {
        var angle = UnityEngine.Random.value * 360;
        var shot = Api.Shot(angle, 1);
        if (shot == null)
        {
            return;
        }
        var behavior = new LockOnEnemyShotBehavior(shot, angle);
        shot.Behavior = behavior;
    }
}
