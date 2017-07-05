using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーが勝利した演出をしているステート。
/// </summary>
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