﻿using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour {
    public float Speed = 0.1f;
    public float MouseSpeed = 0.1f;
    public GameObject ShotObject;
    public GameObject ShotSource;
	public int shotSpan = 20;

    private Rigidbody2D rigidBody;
    private Vector3 shotPosition;
    private int shotTime = 0;
    private Script_SpriteStudio_Root sprite;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        shotPosition = ShotObject.transform.localPosition;
        SetSpriteUp();
        SetMouseControlUp();
	}

	// Update is called once per frame
	void Update()
	{
		Move();
		Shot();
	}

    private void SetSpriteUp()
    {
        var spriteControl = GetComponent<Script_SpriteStudio_ControlPrefab>();
        var prefab = spriteControl.PrefabUnderControl as GameObject;
        sprite = prefab.GetComponent<Script_SpriteStudio_Root>();
    }

    private void SetMouseControlUp()
    {
        var drag = Observable.EveryUpdate()
                  .SkipWhile(t => !Input.GetMouseButtonDown(0))
                  .Select(t => Input.mousePosition)
                  .TakeWhile(t => !Input.GetMouseButtonUp(0));
        drag.Zip(drag.Skip(1), (arg1, arg2) => arg2 - arg1)
            .Repeat()
            .Subscribe(delta =>
		{
            var v = delta * MouseSpeed;
            if (v.magnitude > Speed)
            {
                v = v / v.magnitude * Speed;
            }
            rigidBody.velocity = v;
        });
    }

    private void Shot()
    {
        if (shotTime >= shotSpan)
		{
			var obj = Instantiate(ShotObject);
            obj.transform.position = ShotSource.transform.position;
            obj.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
            shotTime = 0;
		}
		++shotTime;
    }

    private void Move()
    {
        Vector3 velocity = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
			velocity.x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity.x += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity.y -= 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity.y += 1;
        }

        var length = velocity.magnitude;
        if (length != 0)
        {
            velocity /= length;
        }
        rigidBody.velocity = velocity * Speed;
    }
}
