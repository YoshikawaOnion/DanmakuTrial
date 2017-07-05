using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Init : StateMachine {
    protected void EvStateEnter(GameStateContext context)
    {
        GameManager.I.InitializeGame();
		GameManager.I.StartEnemyAction();
        GameManager.I.ChangeState(GameManager.OpeningStateName, context);
    }
}
