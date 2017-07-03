﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppState_Game : StateMachine
{
	protected override void EvStateEnter()
	{
		TitleUiManager.I.gameObject.SetActive(false);
		GameManager.I.ChangeState(GameManager.InitStateName);
        GameManager.I.gameObject.SetActive(true);
	}
}
