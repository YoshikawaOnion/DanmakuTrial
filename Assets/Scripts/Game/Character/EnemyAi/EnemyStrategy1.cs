using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyStrategy1 : EnemyStrategy
{
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
			yield return new WaitForSeconds(1);
			Owner.Move(initialPosition + new Vector3(0, -10, 0), 10);
			yield return new WaitForSeconds(0.2f);
            Shot(Owner.LeftHand.transform.localPosition);

			yield return new WaitForSeconds(1);
			Owner.Move(initialPosition + new Vector3(0, 10, 0), 11);

			yield return new WaitForSeconds(1);
			Owner.Move(initialPosition + new Vector3(0, -10, 0), 10);
			yield return new WaitForSeconds(0.2f);
            Shot(Owner.RightHand.transform.localPosition);

			yield return new WaitForSeconds(1);
			Owner.Move(initialPosition + new Vector3(0, 10, 0), 11);
		}
    }

    private void Shot(Vector3 localPosition)
    {
        Owner.Shot(localPosition * scale, 180, 100);
    }
}
