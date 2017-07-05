using UnityEngine;
using System.Collections;
using UniRx;

public interface IGameStateEventAccepter
{
    Subject<Unit> OnRoundStartSubject { get; }
}
