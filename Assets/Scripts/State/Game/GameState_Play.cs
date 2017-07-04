using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameState_Play : StateMachine {
    protected override void EvStateEnter()
    {
        GameManager.I.Enemy.StartAction();
        GameManager.I.Player.StartAction();
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
                      .Subscribe(t => GameManager.I.ChangeState(GameManager.WinStateName));
        }
        if (GameManager.I.Player.IsDefeated)
        {
            Observable.TimerFrame(40)
                      .Subscribe(t => GameManager.I.ChangeState(GameManager.GameOverStateName));
        }
    }
}
