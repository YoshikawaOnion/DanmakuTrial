using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyBehavior1 : EnemyBehavior
{
    private static readonly float ActionDelay = 1.3f;
    private static readonly int BulletLength = 3;

    private Vector3 initialPosition = new Vector3(0, 200 * Def.UnitPerPixel, 0);
    private float scale;

    public EnemyBehavior1(EnemyApi api) : base(api)
    {
    }

    public IEnumerator Act()
	{
        while (true)
        {
            ShotChunk(Api.Enemy.LeftHand.transform.localPosition);
			yield return WaitRandomly(ActionDelay);

			ShotChunk(Api.Enemy.RightHand.transform.localPosition);
			yield return WaitRandomly(ActionDelay);
		}
    }

    protected override IObservable<Unit> GetAction()
    {
        return Act().ToObservable();
    }


    private void MoveByOffset(Vector3 pixelOffset, int durationFrame)
    {
		Api.Move(initialPosition + pixelOffset * Def.UnitPerPixel, 10);
    }

    private WaitForSeconds WaitRandomly(float pivotWait)
    {
        var delta = UnityEngine.Random.value * 0.5f - 0.25f;
        return new WaitForSeconds(pivotWait + delta);
    }

    private void ShotChunk(Vector3 localPosition)
    {
        var sourcePos = localPosition.Mul(Api.Enemy.transform.lossyScale);
        for (int i = 0; i < BulletLength; i++)
        {
            for (int j = 0; j < BulletLength + 2; j++)
			{
                var offset = new Vector3(i - BulletLength / 2, j - BulletLength / 2, 0);
                var offsetRandom = UnityEngine.Random.insideUnitCircle.ToVector3();
                var position = sourcePos + (offset * 16 + offsetRandom) * Def.UnitPerPixel;
				Api.ShotByOffset(position, 180, 240 * Def.UnitPerPixel);
            }
        }
        Api.PlayShootSound();
    }
}
