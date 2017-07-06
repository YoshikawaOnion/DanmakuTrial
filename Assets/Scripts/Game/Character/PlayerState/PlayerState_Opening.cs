using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class PlayerState_Opening : StateMachine
{
    private IDisposable disposable { get; set; }

    protected void EvStateEnter(PlayerStateContext context)
    {
        disposable = GameManager.I.GameEvents.OnRoundStart
                                .Subscribe(u => 
        {
            context.ChangeState(Player.StateNameFighting);
        });
    }

    protected override void EvStateExit()
    {
        disposable.Dispose();
    }
}
