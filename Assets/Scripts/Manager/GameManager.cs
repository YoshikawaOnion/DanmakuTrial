﻿using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public static readonly string InitStateName = "GameState_Init";
    public static readonly string OpeningStateName = "GameState_Opening";
    public static readonly string PlayStateName = "GameState_Play";
    public static readonly string WinStateName = "GameState_Win";
    public static readonly string GameOverStateName = "GameState_GameOver";

    private static readonly float WallThickness = 10;

	public GameObject WallPrefab;
    public GameObject PlayerPrefub;
    public GameObject EnemyPrefub;
    public GameObject ShootingRoomPrefub;
    public GameObject BulletRendererPrefub;
	public Text FpsText;
	public Enemy Enemy;
	public Player Player;

    private BulletRenderer bulletRenderer;
    private StateMachine stateMachine;
    private List<GameObject> objectsToDestroy;

    protected override void Init()
    {
        stateMachine = GetComponent<StateMachine>();
        objectsToDestroy = new List<GameObject>();
    }

    public void ChangeState(string stateName)
    {
        stateMachine.ChangeSubState(stateName);
    }

    public void InitializeGame()
    {
        var camera = SpriteStudioManager.I.MainCamera;
        var bottomLeft = camera.ViewportToWorldPoint(Vector3.zero);
        var topRight = camera.ViewportToWorldPoint(Vector3.one);
        var size = topRight - bottomLeft;

        SetBulletRendererUp();
        SetWallsUp(topRight, bottomLeft, size);
        SetShootingRoomUp(size);
        //SetFpsUp();
        SetPlayerUp(bottomLeft, size);
        SetEnemyUp(topRight, size);
	}

    public void ClearGameObjects()
    {
        foreach (var item in objectsToDestroy)
        {
            Destroy(item);
        }
    }

    public void StartGameAction()
    {
        Enemy.StartAction();
        Player.StartAction();
    }

    public void StopGameAction()
    {
        Enemy.StopAction();
        Player.StopAction();
    }


    private void SetBulletRendererUp()
    {
        var renderer = Instantiate(BulletRendererPrefub);
        bulletRenderer = renderer.GetComponent<BulletRenderer>();
        objectsToDestroy.Add(renderer);
    }

    private void SetEnemyUp(Vector3 topRight, Vector3 size)
    {
        var e = Instantiate(EnemyPrefub);
        e.transform.position = new Vector3(0, topRight.y - size.y / 3, 0);
        e.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;

        Enemy = e.GetComponent<Enemy>();
        Enemy.BulletRenderer = bulletRenderer;
        objectsToDestroy.Add(Enemy.gameObject);
    }

    private void SetPlayerUp(Vector3 bottomLeft, Vector3 size)
    {
        var p = Instantiate(PlayerPrefub);
        p.transform.position = new Vector3(0, bottomLeft.y + size.y / 4, 0);
        p.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
        Player = p.GetComponent<Player>();
        objectsToDestroy.Add(Player.gameObject);
    }

    private void SetFpsUp()
    {
        Observable.IntervalFrame(10)
                  .Subscribe(t => FpsText.text = (1.0f / Time.deltaTime).ToString());
    }

    private void SetShootingRoomUp(Vector3 size)
    {
        var shootingRoom = Instantiate(ShootingRoomPrefub);
        var collider = shootingRoom.GetComponent<BoxCollider2D>();
        collider.transform.localScale = size;
        objectsToDestroy.Add(shootingRoom);
    }

    private void SetWallsUp(Vector3 topRight, Vector3 bottomLeft, Vector3 size)
    {
        var rightWall = Instantiate(WallPrefab);
        rightWall.gameObject.transform.localScale = new Vector3(WallThickness, size.y, 1);
        rightWall.gameObject.transform.position = topRight + new Vector3(WallThickness, -size.y, 0) / 2;

        var leftWall = Instantiate(WallPrefab);
        leftWall.gameObject.transform.localScale = new Vector3(WallThickness, size.y, 1);
        leftWall.gameObject.transform.position = bottomLeft + new Vector3(-WallThickness, size.y, 0) / 2;

        var topWall = Instantiate(WallPrefab);
        topWall.gameObject.transform.localScale = new Vector3(size.x, WallThickness, 1);
        topWall.gameObject.transform.position = topRight + new Vector3(-size.x, WallThickness, 0) / 2;

        var bottomWall = Instantiate(WallPrefab);
        bottomWall.gameObject.transform.localScale = new Vector3(size.x, WallThickness, 1);
        bottomWall.gameObject.transform.position = bottomLeft + new Vector3(size.x, -WallThickness, 0) / 2;

		objectsToDestroy.Add(rightWall);
		objectsToDestroy.Add(leftWall);
		objectsToDestroy.Add(topWall);
		objectsToDestroy.Add(bottomWall);
    }
}
