using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Init : StateMachine {
    protected void EvStateEnter(EnemyStateContext context)
    {
        GameManager.I.InitializeGame();
        GameManager.I.ChangeState(GameManager.OpeningStateName);
    }
}
