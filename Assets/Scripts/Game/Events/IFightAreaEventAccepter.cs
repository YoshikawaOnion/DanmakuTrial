using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// FightArea がゲーム全体にイベントを通知することができるインターフェース。
/// </summary>
public interface IFightAreaEventAccepter
{
    Subject<Unit> OnEnemyExitsFightAreaSubject { get; }
    Subject<Unit> OnPlayerExitsFightAreaSubject { get; }
}
