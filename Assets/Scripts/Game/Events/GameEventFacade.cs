using UnityEngine;
using System.Collections;
using UniRx;

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

    public Subject<Unit> OnPlayerExitsSubject { get; private set; }
    public IObservable<Unit> OnPlayerExitsFightArea
    {
        get { return OnPlayerExitsSubject; }
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
}
