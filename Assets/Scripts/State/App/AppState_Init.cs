using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppState_Init : StateMachine
{
    protected override void EvStateEnter()
    {
        AppManager.I.ChangeState(AppManager.TitleStateName);
        GameUIManager.I.gameObject.SetActive(false);
    }
}
