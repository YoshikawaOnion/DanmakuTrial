using System;
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
    private List<EnemyShot> Bullets;
    private List<Mob> Mobs;
    private BatchRenderer batchRenderer;

    void Start()
    {
        Bullets = new List<EnemyShot>();
        Mobs = new List<Mob>();
        batchRenderer = GetComponent<BatchRenderer>();
    }

    private void OnDestroy()
    {
        Bullets = null;
        Mobs = null;
        batchRenderer = null;
    }

    /// <summary>
    /// 敵弾を発射し、BatchRendererで描画するよう登録します。
    /// </summary>
    /// <returns>発射した敵弾を表すオブジェクト。</returns>
    /// <param name="source">発射地点。</param>
    /// <param name="angle">発射する角度。</param>
    /// <param name="speed">発射する速度。</param>
    public EnemyShot Shot(Vector3 source, float angle, float speed, EnemyShotBehavior behavior, EnemyApi api)
    {
        var shot = GameManager.I.PoolManager.GetInstance(ObjectPoolManager.Kind.Normal);
        if (shot != null)
		{
			return InitializeShot(shot, behavior, Bullets, source, angle, speed, api);
		}
        return null;
    }

    /// <summary>
    /// モブを発射します。
    /// </summary>
    /// <returns>発射したモブを表すオブジェクト。</returns>
    /// <param name="source">発射地点。</param>
    /// <param name="angle">発射する角度。</param>
    /// <param name="speed">発射する速度。</param>
    public Mob ShotMob(Vector3 source, float angle, float speed, EnemyShotBehavior behavior, EnemyApi api)
    {
        var shot = GameManager.I.PoolManager.GetInstance(ObjectPoolManager.Kind.Mob);
        if (shot != null)
        {
            return InitializeShot(shot, behavior, Mobs, source, angle, speed, api);
        }
        return null;
    }

    private TShot InitializeShot<TShot>(GameObject shot, EnemyShotBehavior behavior, List<TShot> list, Vector3 source, float angle, float speed, EnemyApi api)
        where TShot : EnemyShot
    {
        shot.transform.position = source;
        shot.transform.SetEulerAnglesZ(0);
        shot.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;

        var velocity = Vector2Extensions.FromAngleLength(angle, speed);
        var rigidbody = shot.GetComponent<Rigidbody2D>();
        rigidbody.velocity = velocity;

        var script = shot.GetComponent<TShot>();
        script.InitializeBullet(behavior, api);
		script.DestroyEvent.Subscribe(u => list.Remove(script));
		list.Add(script);

		shot.GetComponent<Collider2D>().enabled = true;
        return script;
    }

    private void Update()
    {
        foreach (var b in Bullets)
        {
            if (b.IsVisible)
			{
				batchRenderer.AddInstanceTS(b.transform.position, b.transform.lossyScale);
            }
        }
    }

    public void Clear()
    {
        foreach (var item in Bullets.ToArray())
        {
            item.ResetBullet();
        }
        foreach (var item in Mobs.ToArray())
        {
            item.ResetBullet();
        }
    }
}
