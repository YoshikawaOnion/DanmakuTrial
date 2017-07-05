using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵弾・自機弾を消滅させるかどうかを制御するクラス。
/// </summary>
public class ShootingRoom : MonoBehaviour {
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerShot" || collision.tag == "EnemyShot")
        {
            Destroy(collision.gameObject);
        }
    }
}
