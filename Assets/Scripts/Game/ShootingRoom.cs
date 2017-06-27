﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRoom : MonoBehaviour {
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerShot" || collision.tag == "EnemyShot")
        {
            Destroy(collision.gameObject);
        }
    }
}
