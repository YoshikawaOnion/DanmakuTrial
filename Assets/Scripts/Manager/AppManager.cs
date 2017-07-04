using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : Singleton<AppManager> {
    public static readonly string InitStateName = "AppState_Init";
    public static readonly string TitleStateName = "AppState_Title";
    public static readonly string GameStateName = "AppState_Game";
	
    public GameObject Canvas;
	
    [SerializeField]
    private GameManager gameManagerPrefub;
    [SerializeField]
    private SoundManager soundManagerPrefub;
    private StateMachine stateMachine;

    protected override void Init()
    {
        stateMachine = GetComponent<StateMachine>();
        Instantiate(gameManagerPrefub);
        ChangeState(InitStateName);
        Instantiate(soundManagerPrefub);
    }

    public void ChangeState(string stateName)
    {
        stateMachine.ChangeSubState(stateName);
    }
}
