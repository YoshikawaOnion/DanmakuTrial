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
        GameUIManager.GameOverOption option = GameUIManager.GameOverOption.Retry;
        yield return GameUIManager.I.InputGameOverMenu((obj) => option = obj);

        if (option == GameUIManager.GameOverOption.Retry)
		{
			GameManager.I.ClearGameObjects();
            GameManager.I.ChangeState(GameManager.InitStateName);
        }
        else if (option == GameUIManager.GameOverOption.Title)
        {
            GameManager.I.ClearGameObjects();
			AppManager.I.ChangeState(AppManager.TitleStateName);   
        }
    }
}
