using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Init : StateMachine {
    protected override void EvStateEnter()
    {
        GameManager.I.InitializeGame();
        GameManager.I.ChangeState(GameManager.OpeningStateName);
    }
}
