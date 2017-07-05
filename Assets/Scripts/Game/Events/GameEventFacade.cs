using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// ゲームプレイ中に発生するイベントの登録口・購読口を提供するクラス。
/// </summary>
public class GameEventFacade : IEnemyStateEventAccepter, IEnemyEventAccepter,
    IFightAreaEventAccepter, ISafeAreaEventAccepter,
    IGameStateEventAccepter, IGameEventObservables
{
    public Subject<Unit> OnNextRoundSubject { get; private set; }
    public IObservable<Unit> OnNextRound
    {
        get { return OnNextRoundSubject; }
    }

    public Subject<Collider2D> OnHitPlayerShotSubject { get; private set; }
    public IObservable<Collider2D> OnHitPlayerShot
    {
        get { return OnHitPlayerShotSubject; }
    }

    public Subject<Unit> OnEnemyExitsFightAreaSubject { get; private set; }
    public IObservable<Unit> OnEnemyExitsFightArea
    {
        get { return OnEnemyExitsFightAreaSubject; }
    }

    public Subject<Unit> OnPlayerExitsFightAreaSubject { get; private set; }
    public IObservable<Unit> OnPlayerExitsFightArea
    {
        get { return OnPlayerExitsFightAreaSubject; }
    }

    public Subject<Unit> OnRoundStartSubject { get; private set; }
    public IObservable<Unit> OnRoundStart
    {
        get { return OnRoundStartSubject; }
    }

    public Subject<Unit> OnEnemyExitsSafeAreaSubject { get; private set; }
    public IObservable<Unit> OnEnemyExitsSafeArea
    {
        get { return OnEnemyExitsSafeAreaSubject; }
    }

    public Subject<Unit> OnEnemyEntersSafeAreaSubject { get; private set; }
    public IObservable<Unit> OnEnemyEntersSafeArea
    {
        get { return OnEnemyEntersSafeAreaSubject; }
    }

    public Subject<Unit> OnEnemyDefeatedSubject { get; private set; }
    public IObservable<Unit> OnEnemyDefeated
    {
        get { return OnEnemyDefeatedSubject; }
    }

    public GameEventFacade()
    {
        OnNextRoundSubject = new Subject<Unit>();
        OnHitPlayerShotSubject = new Subject<Collider2D>();
        OnEnemyExitsFightAreaSubject = new Subject<Unit>();
        OnPlayerExitsFightAreaSubject = new Subject<Unit>();
        OnRoundStartSubject = new Subject<Unit>();
        OnEnemyExitsSafeAreaSubject = new Subject<Unit>();
        OnEnemyEntersSafeAreaSubject = new Subject<Unit>();
        OnEnemyDefeatedSubject = new Subject<Unit>();
    }
}
