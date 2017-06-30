using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;

public class EnemyStrategy6 : EnemyStrategy
{
    private List<CircleEnemyShotBehavior> shots;
    private float anglePivot;

    public EnemyStrategy6(EnemyApi api) : base(api)
    {
        shots = new List<CircleEnemyShotBehavior>();
    }

    protected override IObservable<Unit> GetAction()
    {
        var c0 = PrepareCoroutine().ToObservable();
        var c1 = Observable.EveryUpdate()
                  .Select(t => Unit.Default)
                  .Do(t =>
        {
            anglePivot -= 1;
            foreach (var s in shots)
            {
                s.AnglePivot = anglePivot;
                s.Distance += 0.5f;
            }
        });
        var c2 = ShotCoroutine().ToObservable();

        return c0.SelectMany(c1.Merge(c2));
    }

    private IEnumerator PrepareCoroutine()
    {
        var goal = Api.Enemy.transform.position + new Vector3(-35, 0, 0);
        Api.Move(goal, 30);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ShotCoroutine()
	{
        anglePivot = 45;
        while (true)
        {
            ShotCircle();
			yield return new WaitForSeconds(1.2f);
        }
    }

    private void ShotCircle()
	{
		for (int i = 0; i < CircleEnemyShotBehavior.Way * 4; i++)
		{
			var shot = Api.Shot(0, 0);
			var behavior = new CircleEnemyShotBehavior(shot, i, anglePivot);
			shot.behavior = behavior;
			shots.Add(behavior);
			shot.DestroyEvent.Subscribe(x => shots.Remove(behavior));
		}
    }
}
