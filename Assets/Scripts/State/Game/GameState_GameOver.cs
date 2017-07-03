using UnityEngine;
using System.Collections;
using UniRx;

public class GameState_GameOver : StateMachine
{
    protected override void EvStateEnter()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        GameUiManager.GameOverOption option = GameUiManager.GameOverOption.Retry;
        yield return GameUiManager.I.InputGameOverMenu((obj) => option = obj);

        if (option == GameUiManager.GameOverOption.Retry)
		{
			GameManager.I.ClearGameObjects();
            GameManager.I.ChangeState(GameManager.InitStateName);
        }
        else if (option == GameUiManager.GameOverOption.Title)
        {
            GameManager.I.ClearGameObjects();
			AppManager.I.ChangeState(AppManager.TitleStateName);   
        }
    }
}
