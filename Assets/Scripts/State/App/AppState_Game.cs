using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppState_Game : StateMachine
{
	protected override void EvStateEnter()
	{
        GameManager.I.ChangeState(GameManager.InitStateName);
	}
}
