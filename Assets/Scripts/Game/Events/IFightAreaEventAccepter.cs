using UnityEngine;
using System.Collections;
using UniRx;

public interface IFightAreaEventAccepter
{
    Subject<Unit> OnEnemyExitsFightAreaSubject { get; }
    Subject<Unit> OnPlayerExitsFightAreaSubject { get; }
}
