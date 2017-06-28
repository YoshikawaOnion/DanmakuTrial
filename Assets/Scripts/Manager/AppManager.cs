using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : Singleton<AppManager> {
    public static readonly string InitStateName = "AppState_Init";
    public static readonly string TitleStateName = "AppState_Title";
    public static readonly string GameStateName = "AppState_Game";

    public GameManager GameManagerPrefub;
    public SpriteRenderer Background;
    private StateMachine stateMachine;

    protected override void Init()
    {
        stateMachine = GetComponent<StateMachine>();
        Instantiate(GameManagerPrefub);
        ChangeState(InitStateName);

        var cameraScale = SpriteStudioManager.I.MainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        var imageSize = Background.sprite.bounds.size;
        var bgScale = new Vector2(
            cameraScale.x / imageSize.x * 2,
            cameraScale.y / imageSize.y * 2);
        var bgScale2 = Mathf.Max(bgScale.x, bgScale.y);
        Background.transform.localScale = new Vector3(bgScale2, bgScale2, 1);
    }

    public void ChangeState(string stateName)
    {
        stateMachine.ChangeSubState(stateName);
    }
}
