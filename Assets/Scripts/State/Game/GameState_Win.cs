using UnityEngine;
using System.Collections;

public class GameState_Win : StateMachine
{
    protected void EvStateEnter(GameStateContext context)
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
		yield return StartCoroutine(GameUiManager.I.AnimateWin());
        GameManager.I.ClearGameObjects();
        yield return null;
        AppManager.I.ChangeState(AppManager.TitleStateName);
    }
}