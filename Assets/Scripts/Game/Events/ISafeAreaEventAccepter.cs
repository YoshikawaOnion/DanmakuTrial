using UnityEngine;
using System.Collections;
using UniRx;

public interface ISafeAreaEventAccepter
{
    Subject<Unit> OnEnemyExitsSafeAreaSubject { get; }
    Subject<Unit> OnEnemyEntersSafeAreaSubject { get; }
}
