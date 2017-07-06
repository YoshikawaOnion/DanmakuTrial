using UnityEngine;
using System.Collections;
using UniRx;

public class PlayerState_Neutral : PlayerState_Fight
{
    protected void EvStateEnter(PlayerStateContext context)
    {
		base.EvStateEnter(context);

		GameManager.I.GameEvents.OnHitEnemyShot
				   .Subscribe(collider => OnHit(collider))
				   .AddTo(Disposable);
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
