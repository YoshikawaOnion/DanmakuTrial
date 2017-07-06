﻿using System;
using System.Collections;
using System.Collections.Generic;
using Ist;
using UnityEngine;
using UniRx;
using System.Linq;

/// <summary>
/// BatchRendererを用いて敵弾を高速に描画するクラス。
/// </summary>
public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shotObject = null;
    [SerializeField]
    private int MaxBulletCount = 300;

    internal List<EnemyShot> Bullets;
    private BatchRenderer batchRenderer;

    void Start()
    {
        Bullets = new List<EnemyShot>();
        batchRenderer = GetComponent<BatchRenderer>();
    }

    /// <summary>
    /// 敵弾を発射し、BatchRendererで描画するよう登録します。
    /// </summary>
    /// <returns>発射した敵弾を表すオブジェクト。</returns>
    /// <param name="source">発射地点。</param>
    /// <param name="angle">発射する角度。</param>
    /// <param name="speed">発射する速度。</param>
    public EnemyShot Shoot(Vector3 source, float angle, float speed)
    {
        var x = Mathf.Sin(angle * Mathf.Deg2Rad);
        var y = Mathf.Cos(angle * Mathf.Deg2Rad);
        var direction = new Vector2(x, y);

        var shot = MF_AutoPool.Spawn(shotObject);
        if (shot != null)
        {
			var rigidBody = shot.GetComponent<Rigidbody2D>();
			shot.transform.position = source;
			shot.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
			rigidBody.velocity = direction * speed;

			var script = shot.GetComponent<EnemyShot>();
			Bullets.Add(script);
			script.DestroyEvent.Subscribe(u => Bullets.Remove(script));
			return script;
		}
        return null;
    }

    private void Update()
    {
        Bullets.RemoveAll(x => x == null);
        foreach (var b in Bullets)
        {
            batchRenderer.AddInstanceTS(b.transform.position, b.transform.lossyScale);
        }
    }
}