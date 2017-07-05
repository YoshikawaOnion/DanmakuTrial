using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// Enemy がゲーム全体にイベントを通知することができるインターフェース。
/// </summary>
public interface IEnemyEventAccepter
{
    Subject<Collider2D> OnHitPlayerShotSubject { get; } 
}
