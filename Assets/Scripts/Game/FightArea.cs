using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightArea : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
		{
            SoundManager.I.PlaySe(SeKind.EnemyDamaged);
			enemy.StartNextRound();
            return;
        }

        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
		{
            SoundManager.I.PlaySe(SeKind.PlayerDamaged);
			player.IsDefeated = true;
        }
    }
}
