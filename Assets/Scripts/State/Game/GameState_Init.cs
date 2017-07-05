using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム画面に移動しており、敵との戦闘を開始しようとしてるステート。
/// </summary>
public class GameState_Init : StateMachine {
    protected void EvStateEnter(GameStateContext context)
    {
        GameManager.I.InitializeGame();
		GameManager.I.StartEnemyAction();
        GameManager.I.ChangeState(GameManager.OpeningStateName, context);
    }
}
