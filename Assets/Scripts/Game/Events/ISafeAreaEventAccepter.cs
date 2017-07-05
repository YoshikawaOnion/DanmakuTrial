using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// SafeArea がゲーム全体にイベントを通知することができるインターフェース。
/// </summary>
public interface ISafeAreaEventAccepter
{
    Subject<Unit> OnEnemyExitsSafeAreaSubject { get; }
    Subject<Unit> OnEnemyEntersSafeAreaSubject { get; }
}
