using UnityEngine;
using System.Collections;
using UniRx;

public interface IFightAreaEventAccepter
{
    Subject<Unit> OnEnemyExitsSafeAreaSubject { get; }
    Subject<Unit> OnPlayerExitsSubject { get; }
}
