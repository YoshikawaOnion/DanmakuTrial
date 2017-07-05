using UnityEngine;
using System.Collections;
using UniRx;

public interface IEnemyStateEventAccepter
{
    Subject<Unit> OnNextRoundSubject { get; }
    Subject<Unit> OnEnemyDefeatedSubject { get; }
}
