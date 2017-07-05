using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// 敵の踏ん張りを制御するための範囲を表すクラス。
/// </summary>
public class SafeArea : MonoBehaviour
{
    public ISafeAreaEventAccepter EventAccepter { get; set; }

    private void OnTriggerExit2D(Collider2D collision)
	{
		var enemy = collision.gameObject.GetComponent<Enemy>();
		if (enemy != null)
		{
            EventAccepter.OnEnemyExitsSafeAreaSubject.OnNext(Unit.Default);
		}
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		var enemy = collision.gameObject.GetComponent<Enemy>();
		if (enemy != null)
		{
            EventAccepter.OnEnemyEntersSafeAreaSubject.OnNext(Unit.Default);
		}
    }
}
