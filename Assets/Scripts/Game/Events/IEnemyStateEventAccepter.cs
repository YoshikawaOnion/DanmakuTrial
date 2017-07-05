using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// EnemyState_** がゲーム全体にイベントを通知することができるインターフェース。
/// </summary>
public interface IEnemyStateEventAccepter
{
    Subject<Unit> OnNextRoundSubject { get; }
    Subject<Unit> OnEnemyDefeatedSubject { get; }
}
