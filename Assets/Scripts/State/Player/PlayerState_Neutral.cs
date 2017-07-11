using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class PlayerState_Neutral : PlayerState_Fight
{
	protected override void OnHit()
	{
		Context.ChangeState(Player.StateNameDamaged);
	}
}
