using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyStrategy1 : EnemyStrategy
{
    private static readonly float ActionDelay = 0.8f;
    private static readonly int BulletLength = 5;

    private Vector3 initialPosition = new Vector3(0, 200, 0);
    private float scale;

    public EnemyStrategy1(Enemy owner) : base(owner)
    {
    }

    public override IEnumerator Act()
	{
		scale = Owner.transform.localScale.magnitude;
		Owner.Move(initialPosition, 60);
		yield return new WaitForSeconds(0.6f);

        while (!isDefeated)
        {
            yield return WaitRandomly(ActionDelay);
			Owner.Move(initialPosition + new Vector3(0, -10, 0), 10);
			yield return new WaitForSeconds(0.2f);
            Shot(Owner.LeftHand.transform.localPosition);

			yield return WaitRandomly(ActionDelay);
			Owner.Move(initialPosition + new Vector3(0, 10, 0), 11);

			yield return WaitRandomly(ActionDelay);
			Owner.Move(initialPosition + new Vector3(0, -10, 0), 10);
			yield return new WaitForSeconds(0.2f);
            Shot(Owner.RightHand.transform.localPosition);

			yield return WaitRandomly(ActionDelay);
			Owner.Move(initialPosition + new Vector3(0, 10, 0), 11);
		}
    }

    private WaitForSeconds WaitRandomly(float pivotWait)
    {
        var delta = UnityEngine.Random.value * 0.5f - 0.25f;
        return new WaitForSeconds(pivotWait + delta);
    }

    private void Shot(Vector3 localPosition)
    {
        for (int i = 0; i < BulletLength; i++)
        {
            for (int j = 0; j < BulletLength; j++)
			{
                var offset = new Vector3(i - BulletLength / 2, j - BulletLength / 2, 0);
                var offsetRandom = UnityEngine.Random.insideUnitCircle;
                var position = localPosition * scale
                    + offset * scale * 3
                    + offsetRandom.ToVector3() * scale;
				Owner.Shot(position, 180, 100);
            }
        }
    }
}
