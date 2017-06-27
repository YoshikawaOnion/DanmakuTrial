using UnityEngine;
using System.Collections;

public class GameState_Opening : StateMachine
{
    protected override void EvStateEnter()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
	{
    	yield return StartCoroutine(GameUIManager.I.AnimationGameStart());
        GameManager.I.ChangeState(GameManager.PlayStateName);
    }
}
