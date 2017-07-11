using UnityEngine;
using System.Collections;
using UniRx;

public class AppState_Title : StateMachine
{
    protected override void EvStateEnter()
    {
        TitleUiManager.I.LevelSelectedObservable
                      .SelectMany(x =>
		{
			GameManager.I.EnemyStrategy = x;
            return x.LoadAssets();
        })
                      .Subscribe(x =>
        {
            ChangeState(AppManager.GameStateName);
        });
        TitleUiManager.I.gameObject.SetActive(true);
    }
}
