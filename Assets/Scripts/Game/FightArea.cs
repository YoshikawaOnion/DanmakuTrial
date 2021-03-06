﻿using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// 土俵の範囲による勝敗の判定を制御するクラス。
/// </summary>
public class FightArea : MonoBehaviour
{
    public IFightAreaEventAccepter EventAccepter { get; set; }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
		{
            EventAccepter.OnEnemyExitsFightAreaSubject.OnNext(Unit.Default);
            return;
        }

        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
		{
            EventAccepter.OnPlayerExitsFightAreaSubject.OnNext(Unit.Default);
            return;
        }
    }
}
