using UnityEngine;
using System.Collections;

public class TitleUiManager : Singleton<TitleUiManager>
{
    protected override void Init()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AppManager.I.ChangeState(AppManager.GameStateName);
        }
    }
}
