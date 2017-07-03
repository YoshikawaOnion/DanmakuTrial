using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyStrategy7 : EnemyStrategy
{
    public EnemyStrategy7(EnemyApi api) : base(api)
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
            Shot();
			yield return new WaitForSeconds(0.1f);
			Shot();
			yield return new WaitForSeconds(0.5f);
        }
    }

    private void Shot()
    {
        var angle = UnityEngine.Random.value * 360;
        var shot = Api.Shot(angle, 1);
        var behavior = new LockOnEnemyShotBehavior(shot, angle);
        shot.behavior = behavior;
    }
}
