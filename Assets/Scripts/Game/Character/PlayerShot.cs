﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerShot : MonoBehaviour {
    [Tooltip("速度[px/sec]")]
    public float Speed = 10;

    [SerializeField]
    private GameObject hitEffectPrefab;

	// Use this for initialization
	void Start () {
        var rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector3(0, Speed * Def.UnitPerPixel, 0);
	}

	public void ShowHitEffect()
	{
		var effect = Instantiate(hitEffectPrefab);
		effect.transform.position = transform.position;
		Observable.TimerFrame(40)
				  .Subscribe(t => Destroy(effect));
	}
}
