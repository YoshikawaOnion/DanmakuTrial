using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameState_Play : StateMachine {
    private GameStateContext context;

    protected void EvStateEnter(GameStateContext context)
    {
        this.context = context;
        GameManager.I.StartEnemyAction();
    }

    protected override void EvStateExit()
    {
        GameManager.I.Player.StopAction();
    }

    private void Update()
    {
        if (GameManager.I.Enemy.IsDefeated)
        {
            Observable.TimerFrame(40)
                      .Subscribe(t => context.ChangeState(GameManager.WinStateName));
        }
        if (GameManager.I.Player.IsDefeated)
        {
            Observable.TimerFrame(40)
                      .Subscribe(t => context.ChangeState(GameManager.GameOverStateName));
        }
    }
}
