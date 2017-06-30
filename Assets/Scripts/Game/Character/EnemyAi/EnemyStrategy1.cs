using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyStrategy1 : EnemyStrategy
{
    private static readonly float ActionDelay = 0.9f;
    private static readonly int BulletLength = 4;

    private Vector3 initialPosition = new Vector3(0, 200 * Def.UnitPerPixel, 0);
    private float scale;

    public EnemyStrategy1(Enemy owner) : base(owner)
    {
    }

    public IEnumerator Act()
	{
        while (true)
        {
            ShotChunk(Owner.LeftHand.transform.localPosition);
			yield return WaitRandomly(ActionDelay);

			ShotChunk(Owner.RightHand.transform.localPosition);
			yield return WaitRandomly(ActionDelay);
		}
    }

    protected override IObservable<Unit> GetAction()
    {
        return Act().ToObservable();
    }


    private void MoveByOffset(Vector3 pixelOffset, int durationFrame)
    {
		Owner.Move(initialPosition + pixelOffset * Def.UnitPerPixel, 10);
    }

    private WaitForSeconds WaitRandomly(float pivotWait)
    {
        var delta = UnityEngine.Random.value * 0.5f - 0.25f;
        return new WaitForSeconds(pivotWait + delta);
    }

    private void ShotChunk(Vector3 localPosition)
    {
        var sourcePos = localPosition.Mul(Owner.transform.lossyScale);
        for (int i = 0; i < BulletLength; i++)
        {
            for (int j = 0; j < BulletLength + 2; j++)
			{
                var offset = new Vector3(i - BulletLength / 2, j - BulletLength / 2, 0);
                var offsetRandom = UnityEngine.Random.insideUnitCircle.ToVector3();
                var position = sourcePos + (offset * 12 + offsetRandom) * Def.UnitPerPixel;
				Owner.Shot(position, 180, 240 * Def.UnitPerPixel);
            }
        }
        Owner.AudioSource.PlayOneShot(Owner.ShootSound);
    }
}
