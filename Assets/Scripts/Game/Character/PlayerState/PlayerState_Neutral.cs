using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class PlayerState_Neutral : PlayerState_Fight
{
    private IDisposable disposable;

    protected void EvStateEnter(PlayerStateContext context)
    {
		base.EvStateEnter(context);

		disposable = GameManager.I.GameEvents.OnHitEnemyShot
				   .Subscribe(collider => OnHit(collider))
				   .AddTo(Disposable);
	}

    protected override void EvStateExit()
    {
        base.EvStateExit();
        disposable.Dispose();
    }

	private void OnHit(Collider2D collider)
	{
		if (collider.tag == EnemyShotTag)
		{
			Destroy(collider.gameObject);
			Context.Player.Rigidbody.AddForce(
				Context.PushOnShoot * Def.UnitPerPixel);
			Context.ChangeState(Player.StateNameDamaged);
		}
		if (collider.tag == EnemyTag)
		{
			Context.Player.Rigidbody.AddForce(
				Context.PushOnCollide * Def.UnitPerPixel);
			Context.ChangeState(Player.StateNameDamaged);
		}
	}
}
