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
		damageSubscription = Observable.TimerFrame(60)
			.Subscribe(t =>
		{
			SetAnimation("Player");
			damageSubscription = null;
		});
		SetAnimation("Damage");        
    }

    private void SetAnimation(string name)
	{
		var index = sprite.IndexGetAnimation(name);
		sprite.AnimationPlay(index);
    }
}
