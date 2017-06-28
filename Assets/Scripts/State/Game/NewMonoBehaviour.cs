using UnityEngine;
using System.Collections;

public class GameState_GameOver : StateMachine
{
    protected override void EvStateEnter()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        yield return StartCoroutine(GameUIManager.I.AnimationGameOver());
        GameManager.I.ClearGameObjects();
        AppManager.I.ChangeState(AppManager.TitleStateName);
    }
}
