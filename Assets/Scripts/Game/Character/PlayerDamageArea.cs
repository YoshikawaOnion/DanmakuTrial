using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerDamageArea : MonoBehaviour {
    public IPlayerDamageAreaEventAccepter EventAccepter { get; set; }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
        EventAccepter.OnHitEnemyShotSubject.OnNext(collision);
    }
}
