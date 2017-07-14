using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerDamageArea : MonoBehaviour {
    public IPlayerDamageAreaEventAccepter EventAccepter { get; set; }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
        var shot = collision.GetComponent<EnemyShot>();
        if (shot != null && shot.IsVisible)
		{
			EventAccepter.OnHitEnemyShotSubject.OnNext(collision);
        }

        if (collision.tag == Def.EnemyTag
            || collision.tag == Def.MobTag)
		{
			EventAccepter.OnHitEnemyShotSubject.OnNext(collision);
        }
    }
}
