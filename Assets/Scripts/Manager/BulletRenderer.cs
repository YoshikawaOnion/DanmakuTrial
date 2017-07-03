using System;
using System.Collections;
using System.Collections.Generic;
using Ist;
using UnityEngine;
using UniRx;

/// <summary>
/// BatchRendererを用いて敵弾を高速に描画するクラス。
/// </summary>
public class BulletRenderer : MonoBehaviour {
    [SerializeField]
    private GameObject shotObject = null;

    internal List<Rigidbody2D> Bullets;
    private BatchRenderer batchRenderer;

    void Start()
	{
		Bullets = new List<Rigidbody2D>();
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

		var shot = Instantiate(shotObject);
		var rigidBody = shot.GetComponent<Rigidbody2D>();
		shot.transform.position = source;
        shot.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
		rigidBody.velocity = direction * speed;
		
        Bullets.Add(rigidBody);
        var script = shot.GetComponent<EnemyShot>();
        script.DestroyEvent.Subscribe(u => Bullets.Remove(rigidBody));

        return script;
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
