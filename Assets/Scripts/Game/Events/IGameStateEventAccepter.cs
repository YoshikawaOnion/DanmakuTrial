using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// GameState_** クラスがゲーム全体にイベントを通知することができるインターフェース。
/// </summary>
public interface IGameStateEventAccepter
{
    Subject<Unit> OnRoundStartSubject { get; }
}
