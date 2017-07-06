using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// PlayerDamageArea がゲーム全体にイベントを通知することができるインターフェース。
/// </summary>
public interface IPlayerDamageAreaEventAccepter
{
    Subject<Collider2D> OnHitEnemyShotSubject { get; }
}
