using UnityEngine;
using System.Collections;
using System;
using UniRx;

/// <summary>
/// 敵キャラクターを制御する機能を提供するクラス。
/// </summary>
public class EnemyApi
{
    public BulletManager BulletRenderer { get; set; }
    public ObjectPoolManager PoolManager { get; set; }
    public Enemy Enemy { get; private set; }
    public Transform Transform
    {
        get { return Enemy.transform; }
    }
    public Rigidbody2D Rigidbody
    {
        get { return Enemy.Rigidbody; }
    }

    private IDisposable moveSubscription { get; set; }

    public EnemyApi(Enemy enemy)
    {
        this.Enemy = enemy;
	}

    /// <summary>
    /// 敵キャラクターから弾を1つ発射します。
    /// </summary>
    /// <returns>発射した弾を表す EnemyShot オブジェクト。</returns>
    /// <param name="angle">発射する方向。</param>
    /// <param name="speed">発射するスピード。</param>
	public EnemyShot Shot(float angle, float speed, EnemyShotBehavior behavior = null)
	{
		return ShotByOffset(Vector3.zero, angle, speed, behavior);
	}

	/// <summary>
	/// 敵キャラクターから一定距離離れた位置から弾を1つ発射します。
	/// </summary>
	/// <returns>発射した弾を表す EnemyShot オブジェクト。</returns>
	/// <param name="offset">敵キャラクターから見た相対的な発射位置。</param>
	/// <param name="angle">発射する方向。</param>
	/// <param name="speed">発射するスピード。</param>
	public EnemyShot ShotByOffset(Vector3 offset, float angle, float speed, EnemyShotBehavior behavior = null)
	{
		var pos = Enemy.transform.position + offset.ZReplacedBy(10);
		return Shot(pos, angle, speed, behavior);
	}

	/// <summary>
	/// 指定した位置から弾を1つ発射します。
	/// </summary>
	/// <returns>発射した弾を表す EnemyShot オブジェクト。</returns>
	/// <param name="position">発射する位置のワールド座標。</param>
	/// <param name="angle">発射する方向。</param>
	/// <param name="speed">発射するスピード。</param>
	public EnemyShot Shot(Vector3 position, float angle, float speed, EnemyShotBehavior behavior = null)
	{
        behavior = behavior ?? new NullEnemyShotBehavior();
		position += new Vector3(0, 0, 10);
		var shot = BulletRenderer.Shot(position, angle, speed, behavior, this);
        return shot;
	}

    public Mob ShotMob(Vector3 position, float angle, float speed, EnemyShotBehavior behavior = null)
	{
		behavior = behavior ?? new NullEnemyShotBehavior();
        position += new Vector3(0, 0, 10);
        var shot = BulletRenderer.ShotMob(position, angle, speed, behavior, this);
        return shot;
    }

    /// <summary>
    /// 敵キャラクターを滑らかに移動させます。
    /// </summary>
    /// <param name="goal">移動の目的地点。</param>
    /// <param name="durationFrame">移動にかける時間(フレーム)。</param>
	public void MoveIt(Vector3 goal, int durationFrame)
	{
		if (moveSubscription != null)
		{
			moveSubscription.Dispose();
		}

		moveSubscription = Enemy.Move(goal, durationFrame);
	}

    /// <summary>
    /// 弾の発射音を再生します。
    /// </summary>
    public void PlayShootSound()
    {
        SoundManager.I.PlaySe(SeKind.EnemyShot);
    }

    /// <summary>
    /// 指定した地点からプレイヤーへのベクトルを取得します。
    /// </summary>
    /// <returns>指定した地点からプレイヤーへのベクトル。</returns>
    /// <param name="source">計算の基準位置。</param>
    public Vector3 GetDistanceToPlayer(Vector3 source)
    {
        return GameManager.I.Player.transform.position - source;
    }

	/// <summary>
	/// 指定した地点からプレイヤーへの角度を取得します。
	/// </summary>
	/// <returns>指定した地点からプレイヤーへの角度</returns>
    /// <param name="source">計算の基準位置。</param>
	public float GetAngleToPlayer(Vector3 source)
    {
        var v = GetDistanceToPlayer(source);
        return -(Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg) + 90;
    }
}
