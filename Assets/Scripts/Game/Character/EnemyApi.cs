using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class EnemyApi
{
    internal BulletRenderer BulletRenderer { get; set; }
    internal Enemy Enemy { get; private set; }
    internal Transform Transform
    {
        get { return Enemy.transform; }
    }
    internal Rigidbody2D Rigidbody
    {
        get { return Enemy.rigidbody; }
    }

    private IDisposable moveSubscription { get; set; }
    private AudioSource audioSource { get; set; }

    public EnemyApi(Enemy enemy)
    {
        this.Enemy = enemy;
        audioSource = enemy.GetComponent<AudioSource>();
	}

	public EnemyShot Shot(float angle, float speed)
	{
		return ShotByOffset(Vector3.zero, angle, speed);
	}

	public EnemyShot ShotByOffset(Vector3 offset, float angle, float speed)
	{
		var pos = Enemy.transform.position + offset.ZReplacedBy(10);
		return Shot(pos, angle, speed);
	}

	public EnemyShot Shot(Vector3 position, float angle, float speed)
	{
		position += new Vector3(0, 0, 10);
		var shot = BulletRenderer.Shoot(position, angle, speed);
		shot.Api = this;
		return shot;
	}

	public void Move(Vector3 goal, int durationFrame)
	{
		if (moveSubscription != null)
		{
			moveSubscription.Dispose();
		}

		var prevPos = Enemy.transform.position;
		moveSubscription = Observable.EveryUpdate()
				  .Take(durationFrame)
				  .Subscribe(t =>
		{
			var pos = (prevPos * (durationFrame - t) + goal * t) / durationFrame;
			Enemy.transform.position = pos;
		}, () => moveSubscription = null);
	}

    public void PlayShootSound()
    {
        audioSource.PlayOneShot(Enemy.ShootSound);
    }

    public void PlayDefeatedSound()
    {
        audioSource.PlayOneShot(Enemy.DefeatedSound);
    }

    public Vector3 GetAngleToPlayer(Vector3 source)
    {
        return GameManager.I.Player.transform.position - source;
    }
}
