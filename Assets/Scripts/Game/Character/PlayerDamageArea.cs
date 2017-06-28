using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerDamageArea : MonoBehaviour {
    public GameObject PlayerSprite;
    public Player Owner;

	private Script_SpriteStudio_Root sprite;
    private IDisposable damageSubscription;

    // Use this for initialization
    void Start () {
        sprite = PlayerSprite.GetComponent<Script_SpriteStudio_Root>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (Owner.IsEnabled && !Owner.isDefeated && collision.tag == "EnemyShot")
		{
			Destroy(collision.gameObject);
            Owner.isDefeated = true;

            /*/
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
            //*/
		}
	}

    private void SetAnimation(string name)
	{
		var index = sprite.IndexGetAnimation(name);
		sprite.AnimationPlay(index);
    }
}
