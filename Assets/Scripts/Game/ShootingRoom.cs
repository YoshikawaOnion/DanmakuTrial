using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵弾・自機弾を消滅させるかどうかを制御するクラス。
/// </summary>
public class ShootingRoom : MonoBehaviour {
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == Def.PlayerShotTag)
        {
            Destroy(collision.gameObject);
        }

        var shot = collision.gameObject.GetComponent<EnemyShot>();
        if (shot != null)
        {
            //MF_AutoPool.Despawn(collision.gameObject);
            //shot.NotifyDespawn();
            Destroy(shot.gameObject);
        }
    }
}
