using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;

/// <summary>
/// プレイヤーが敵と戦っているステート。
/// </summary>
public class GameState_Play : StateMachine
{
    private GameStateContext context;
    private CompositeDisposable disposable;

    protected void EvStateEnter(GameStateContext context)
    {
        this.context = context;
        disposable = new CompositeDisposable();
    }

    protected override void EvStateExit()
    {
        GameManager.I.Player.StopAction();
        disposable.Dispose();
    }

    private void Update()
    {
        GameManager.I.GameEvents.OnNextRound
                   .SelectMany(Observable.TimerFrame(40))
                   .Subscribe(t => context.ChangeState(GameManager.OpeningStateName))
                   .AddTo(disposable);

        GameManager.I.GameEvents.OnEnemyDefeated
                   .SelectMany(Observable.TimerFrame(40))
                   .Subscribe(t => context.ChangeState(GameManager.WinStateName))
                   .AddTo(disposable);

        GameManager.I.GameEvents.OnPlayerExitsFightArea
                   .SelectMany(Observable.TimerFrame(40))
                   .Subscribe(t => context.ChangeState(GameManager.GameOverStateName))
                   .AddTo(disposable);
    }
}
