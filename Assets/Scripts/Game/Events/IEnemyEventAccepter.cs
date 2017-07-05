using UnityEngine;
using System.Collections;
using UniRx;

public interface IEnemyEventAccepter
{
    Subject<Collider2D> OnHitPlayerShotSubject { get; } 
}
