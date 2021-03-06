﻿#pragma warning disable CS0649
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アプリケーション全体を制御するクラス。
/// </summary>
public class AppManager : Singleton<AppManager> {
    public static readonly string InitStateName = "AppState_Init";
    public static readonly string TitleStateName = "AppState_Title";
    public static readonly string GameStateName = "AppState_Game";
	
    public GameObject Canvas;
	
    [SerializeField]
    private GameManager gameManagerPrefab;
    [SerializeField]
    private SoundManager soundManagerPrefab;
    [SerializeField]
    private GameObject dohyoBackgroundPrefab;
    [SerializeField]
    private DebugManager debugManagerPrefab;
    private StateMachine stateMachine;

    protected override void Init()
    {
        Instantiate(gameManagerPrefab);
        Instantiate(soundManagerPrefab);
		Instantiate(debugManagerPrefab);

		stateMachine = GetComponent<StateMachine>();
		ChangeState(InitStateName);

        var bg = Instantiate(dohyoBackgroundPrefab);
        bg.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;

        GameManager.I.Dohyou = bg;
    }

    protected override void OnDestroy()
    {
        stateMachine = null;
    }

    public void ChangeState(string stateName)
    {
        stateMachine.ChangeSubState(stateName);
    }
}
