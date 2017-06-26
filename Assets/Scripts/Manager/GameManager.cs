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
    public Text FpsText;

    protected override void Init()
    {
        var camera = CameraObject.GetComponent<Camera>();
        var topRight = camera.ViewportToWorldPoint(Vector3.one);
        var bottomLeft = camera.ViewportToWorldPoint(Vector3.zero);
        var size = topRight - bottomLeft;

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

        Observable.IntervalFrame(10)
                  .Subscribe(t => FpsText.text = (1.0f / Time.deltaTime).ToString());
    }
}
