using UnityEngine;
using System.Collections;

public class GameState_Opening : StateMachine
{
    private GameStateContext context;

    protected void EvStateEnter(GameStateContext context)
    {
        this.context = context;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
	{

        //yield return StartCoroutine(GameUIManager.I.AnimateGameStart());
        yield return null;
        GameManager.I.ChangeState(GameManager.PlayStateName, context);
    }
}
