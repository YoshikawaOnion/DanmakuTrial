using UnityEngine;
using System.Collections;
using UniRx;

public class AppState_Title : StateMachine
{
    protected override void EvStateEnter()
    {
        TitleUiManager.I.LevelSelectedObservable
                      .Subscribe(x =>
        {
            GameManager.I.EnemyStrategy = x;
            ChangeState(AppManager.GameStateName);
        });
        TitleUiManager.I.gameObject.SetActive(true);
    }
}
