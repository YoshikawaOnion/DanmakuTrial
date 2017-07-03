using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームをプレイする画面のマネージャー クラス。
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public static readonly string InitStateName = "GameState_Init";
    public static readonly string OpeningStateName = "GameState_Opening";
    public static readonly string PlayStateName = "GameState_Play";
    public static readonly string WinStateName = "GameState_Win";
    public static readonly string GameOverStateName = "GameState_GameOver";

    private static readonly float WallThickness = 10;

    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject shootingRoomPrefab;
    [SerializeField]
    private GameObject bulletRendererPrefab;

	internal Enemy Enemy;
	internal Player Player;

    public AudioClip Bgm;
    public EnemyStrategy EnemyStrategy;

    private BulletRenderer bulletRenderer;
    private StateMachine stateMachine;
	private List<GameObject> objectsToDestroy;
    private AudioSource audioSource;

    protected override void Init()
    {
        stateMachine = GetComponent<StateMachine>();
        audioSource = GetComponent<AudioSource>();
        objectsToDestroy = new List<GameObject>();
    }

    /// <summary>
    /// GameManagerのステートを遷移します。
    /// </summary>
    /// <param name="stateName">遷移先のステート名.</param>
    public void ChangeState(string stateName)
    {
        stateMachine.ChangeSubState(stateName);
    }

    /// <summary>
    /// ゲーム内のオブジェクトをそれぞれ初期化します。
    /// </summary>
    public void InitializeGame()
    {
        var bottomLeft = new Vector3(-45, -80);
        var topRight = new Vector3(45, 80);
        var size = topRight - bottomLeft;

        SetBulletRendererUp();
        SetWallsUp(topRight, bottomLeft, size);
        SetShootingRoomUp(size);
        SetPlayerUp(bottomLeft, size);
        SetEnemyUp(topRight, size);

        audioSource.clip = Bgm;
        audioSource.Play();
	}

    /// <summary>
    /// ゲーム内のオブジェクトを削除します。
    /// </summary>
    public void ClearGameObjects()
    {
        foreach (var item in objectsToDestroy)
        {
            Destroy(item);
        }
        audioSource.Stop();
    }

    /// <summary>
    /// ゲームの進行を開始します。
    /// </summary>
    public void StartGameAction()
    {
        Enemy.StartAction();
        Player.StartAction();
    }

    /// <summary>
    /// ゲームの進行を停止します。
    /// </summary>
    public void StopGameAction()
    {
        Enemy.StopAction();
        Player.StopAction();
    }


    /// <summary>
    /// BatchRendererを用いて敵弾を描画する準備をします。
    /// </summary>
    private void SetBulletRendererUp()
    {
        var renderer = Instantiate(bulletRendererPrefab);
        bulletRenderer = renderer.GetComponent<BulletRenderer>();
        objectsToDestroy.Add(renderer);
    }

    /// <summary>
    /// 敵キャラクターを初期化します。
    /// </summary>
    /// <param name="topRight">画面の右上端の座標。</param>
    /// <param name="size">画面の左下端の座標。</param>
    private void SetEnemyUp(Vector3 topRight, Vector3 size)
    {
        var e = Instantiate(enemyPrefab);
        e.transform.position = new Vector3(0, topRight.y - size.y / 2.6f, 0);
        e.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;

        Enemy = e.GetComponent<Enemy>();
        Enemy.BulletRenderer = bulletRenderer;
        Enemy.Strategy = EnemyStrategy;
        objectsToDestroy.Add(Enemy.gameObject);
    }

    /// <summary>
    /// プレイヤーを初期化します。
    /// </summary>
    /// <param name="bottomLeft">画面の右上端の座標。</param>
    /// <param name="size">画面の左上端の座標。</param>
    private void SetPlayerUp(Vector3 bottomLeft, Vector3 size)
    {
        var p = Instantiate(playerPrefab);
        p.transform.position = new Vector3(0, bottomLeft.y + size.y / 2.6f, 0);
        p.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
        Player = p.GetComponent<Player>();
        objectsToDestroy.Add(Player.gameObject);
    }

    /// <summary>
    /// 敵やプレイヤーのショットが存在できる範囲を初期化します。
    /// </summary>
    /// <param name="size">Size.</param>
    private void SetShootingRoomUp(Vector3 size)
    {
        var shootingRoom = Instantiate(shootingRoomPrefab);
        var collider = shootingRoom.GetComponent<BoxCollider2D>();
        collider.transform.localScale = size + new Vector3(10, 10);
        objectsToDestroy.Add(shootingRoom);
    }

    /// <summary>
    /// プレイヤーが移動できる範囲を初期化します。
    /// </summary>
    /// <param name="topRight">Top right.</param>
    /// <param name="bottomLeft">Bottom left.</param>
    /// <param name="size">Size.</param>
    private void SetWallsUp(Vector3 topRight, Vector3 bottomLeft, Vector3 size)
    {
        var rightWall = Instantiate(wallPrefab);
        rightWall.gameObject.transform.localScale = new Vector3(WallThickness, size.y, 1);
        rightWall.gameObject.transform.position = topRight + new Vector3(WallThickness, -size.y, 0) / 2;

        var leftWall = Instantiate(wallPrefab);
        leftWall.gameObject.transform.localScale = new Vector3(WallThickness, size.y, 1);
        leftWall.gameObject.transform.position = bottomLeft + new Vector3(-WallThickness, size.y, 0) / 2;

        var topWall = Instantiate(wallPrefab);
        topWall.gameObject.transform.localScale = new Vector3(size.x, WallThickness, 1);
        topWall.gameObject.transform.position = topRight + new Vector3(-size.x, WallThickness, 0) / 2;

        var bottomWall = Instantiate(wallPrefab);
        bottomWall.gameObject.transform.localScale = new Vector3(size.x, WallThickness, 1);
        bottomWall.gameObject.transform.position = bottomLeft + new Vector3(size.x, -WallThickness, 0) / 2;

		objectsToDestroy.Add(rightWall);
		objectsToDestroy.Add(leftWall);
		objectsToDestroy.Add(topWall);
		objectsToDestroy.Add(bottomWall);
    }
}
