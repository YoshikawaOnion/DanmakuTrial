using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class PlayerState_Neutral : PlayerState_Fight
{
    protected void EvStateEnter(PlayerStateContext context)
    {
		base.EvStateEnter(context);
	}

    protected override void EvStateExit()
    {
        base.EvStateExit();
    }

	protected override void OnHit()
	{
		Context.ChangeState(Player.StateNameDamaged);
	}
}
