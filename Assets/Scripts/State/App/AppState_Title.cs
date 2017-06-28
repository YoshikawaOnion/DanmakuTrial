using UnityEngine;
using System.Collections;

public class AppState_Title : StateMachine
{
    protected override void EvStateEnter()
    {
        TitleUiManager.I.gameObject.SetActive(true);
    }
}
