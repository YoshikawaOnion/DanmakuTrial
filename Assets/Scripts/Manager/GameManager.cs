using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private static readonly float WallThickness = 10;

    public GameObject CameraObject;
	public GameObject WallPrefab;
    public GameObject PlayerPrefub;
    public GameObject EnemyPrefub;
    public GameObject ShootingRoomPrefub;
    public Script_SpriteStudio_ManagerDraw SpriteStudioView;
    public GameObject BulletRendererPrefub;
    public Text FpsText;

    private BulletRenderer bulletRenderer;

    protected override void Init()
    {
        var camera = CameraObject.GetComponent<Camera>();
        var bottomLeft = camera.ViewportToWorldPoint(Vector3.zero);
        var topRight = camera.ViewportToWorldPoint(Vector3.one);
        var size = topRight - bottomLeft;

        SetBulletRendererUp();
        SetWallsUp(topRight, bottomLeft, size);
        SetShootingRoomUp(size);
        SetFpsUp();
        SetPlayerUp(bottomLeft, size);
        SetEnemyUp(topRight, size);
    }

    private void SetBulletRendererUp()
    {
        var renderer = Instantiate(BulletRendererPrefub);
        bulletRenderer = renderer.GetComponent<BulletRenderer>();
        bulletRenderer.View = SpriteStudioView;
    }

    private void SetEnemyUp(Vector3 topRight, Vector3 size)
    {
        var enemy = Instantiate(EnemyPrefub);
        enemy.transform.position = new Vector3(0, topRight.y - size.y / 3, 0);
        enemy.transform.parent = SpriteStudioView.transform;

        var component = enemy.GetComponent<Enemy>();
        component.BulletRenderer = bulletRenderer;
    }

    private void SetPlayerUp(Vector3 bottomLeft, Vector3 size)
    {
        var player = Instantiate(PlayerPrefub);
        player.transform.position = new Vector3(0, bottomLeft.y + size.y / 4, 0);
        player.transform.parent = SpriteStudioView.transform;
        player.GetComponent<Player>().View = SpriteStudioView;
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
    }
}
