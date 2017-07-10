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
    [SerializeField]
    private BulletPoolManager poolManagerPrefab = null;

    internal List<EnemyShot> Bullets;
    internal List<Mob> Mobs;
    private BatchRenderer batchRenderer;
    private BulletPoolManager poolManager;

    void Start()
    {
        Bullets = new List<EnemyShot>();
        Mobs = new List<Mob>();
        batchRenderer = GetComponent<BatchRenderer>();
        poolManager = Instantiate(poolManagerPrefab);
    }

    private void OnDestroy()
    {
        Bullets = null;
        Mobs = null;
        batchRenderer = null;
        poolManager = null;
    }

    /// <summary>
    /// 敵弾を発射し、BatchRendererで描画するよう登録します。
    /// </summary>
    /// <returns>発射した敵弾を表すオブジェクト。</returns>
    /// <param name="source">発射地点。</param>
    /// <param name="angle">発射する角度。</param>
    /// <param name="speed">発射する速度。</param>
    public EnemyShot Shot(Vector3 source, float angle, float speed)
    {
        var shot = poolManagerPrefab.GetInstance(BulletPoolManager.Kind.Normal);
        if (shot != null)
		{
			return InitializeShot(shot, Bullets, source, angle, speed);
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
    public Mob ShotMob(Vector3 source, float angle, float speed)
    {
        var shot = poolManagerPrefab.GetInstance(BulletPoolManager.Kind.Mob);
        if (shot != null)
        {
            return InitializeShot(shot, Mobs, source, angle, speed);
        }
        return null;
    }

    private TShot InitializeShot<TShot>(GameObject shot, List<TShot> list, Vector3 source, float angle, float speed)
        where TShot : EnemyShot
    {
        shot.transform.position = source;
        shot.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;

        var velocity = Vector2Extensions.FromAngleLength(angle, speed);
        var rigidbody = shot.GetComponent<Rigidbody2D>();
        rigidbody.velocity = velocity;

        var script = shot.GetComponent<TShot>();
        script.InitializeBullet(poolManager);
        list.Add(script);
        script.DestroyEvent.Subscribe(u => list.Remove(script));
        return script;
    }

    private void Update()
    {
        Bullets.RemoveAll(x => x == null);
        Mobs.RemoveAll(x => x == null);
        foreach (var b in Bullets)
        {
            batchRenderer.AddInstanceTS(b.transform.position, b.transform.lossyScale);
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
