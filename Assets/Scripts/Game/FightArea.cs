using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightArea : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
			enemy.AudioSource.PlayOneShot(enemy.DamageSound);
			enemy.StartNextRound();
        }
        else if(!GameManager.I.Enemy.IsTimeToNextRound && collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();
			player.AudioSource.PlayOneShot(player.DamageSound);
			player.isDefeated = true;
        }
    }
}
