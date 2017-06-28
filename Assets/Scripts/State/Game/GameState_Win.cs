using UnityEngine;
using System.Collections;

public class GameState_Win : StateMachine
{
    protected override void EvStateEnter()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
		yield return StartCoroutine(GameUIManager.I.AnimationWin());
        GameManager.I.ClearGameObjects();
        AppManager.I.ChangeState(AppManager.TitleStateName);
    }
}