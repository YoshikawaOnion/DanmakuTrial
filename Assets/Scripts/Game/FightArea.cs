﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightArea : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
		{
			enemy.AudioSource.PlayOneShot(enemy.DamageSound);
			enemy.StartNextRound();
            return;
        }

        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
		{
			player.AudioSource.PlayOneShot(player.DamageSound);
			player.isDefeated = true;
        }
    }
}
