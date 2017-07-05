using UnityEngine;
using System.Collections;
using UniRx;

public interface IGameEventObservables
{
	IObservable<Unit> OnNextRound { get; }
	IObservable<Collider2D> OnHitPlayerShot { get; }
	IObservable<Unit> OnEnemyExitsFightArea { get; }
	IObservable<Unit> OnPlayerExitsFightArea { get; }
	IObservable<Unit> OnRoundStart { get; }
    IObservable<Unit> OnEnemyExitsSafeArea { get; }
    IObservable<Unit> OnEnemyEntersSafeArea { get; }
    IObservable<Unit> OnEnemyDefeated { get; }
}
