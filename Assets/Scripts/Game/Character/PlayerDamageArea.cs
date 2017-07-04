using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerDamageArea : MonoBehaviour {
    public Player Owner;

    [SerializeField]
    private GameObject playerSprite = null;
    [Tooltip("被弾時の押し出し[px*kg/sec^2]")]
    [SerializeField]
    private Vector2 pushOnShoot = new Vector2(0, -7000);
    [Tooltip("敵と接触時の押し出し[px*kg/sec^2]")]
    [SerializeField]
    private Vector2 pushOnCollide = new Vector2(0, -10000);

	private Script_SpriteStudio_Root sprite;
    private IDisposable damageSubscription;

    // Use this for initialization
    void Start () {
        sprite = playerSprite.GetComponent<Script_SpriteStudio_Root>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (!Owner.IsEnabled || Owner.IsDefeated)
        {
            return;
        }
        if (collision.tag == "EnemyShot")
		{
			Destroy(collision.gameObject);
			Owner.Rigidbody.AddForce(pushOnShoot * Def.UnitPerPixel);
			AnimateDamage();
		}
        if (collision.tag == "Enemy")
		{
			Owner.Rigidbody.AddForce(pushOnCollide * Def.UnitPerPixel);
            AnimateDamage();
        }
    }

    private void AnimateDamage()
	{
		if (damageSubscription != null)
		{
			damageSubscription.Dispose();
		}

        var composite = new CompositeDisposable();

        Observable.EveryUpdate()
                  .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(0.3)))
                  .Select(t => new
        {
            X = Mathf.Sin(t * 8) * 2 * Def.UnitPerPixel,
            Y = Mathf.Cos(t * 8) * 2 * Def.UnitPerPixel,
        })
                  .Subscribe(p =>
        {
            SpriteStudioManager.I.MainCamera.transform.position = new Vector3(p.X, p.Y, 0);
        })
                  .AddTo(composite);

		Observable.TimerFrame(60)
			      .Subscribe(t =>
		{
			SetAnimation("Player");
			damageSubscription = null;
		})
                  .AddTo(composite);
		SetAnimation("Damage");

        damageSubscription = composite;
    }

    private void SetAnimation(string name)
	{
		var index = sprite.IndexGetAnimation(name);
		sprite.AnimationPlay(index);
    }
}
