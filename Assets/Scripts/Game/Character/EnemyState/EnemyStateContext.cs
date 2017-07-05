using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStateContext : EventContext
{
    public IEnumerator<EnemyBehavior> Behaviors { get; set; }
    public EnemyBehavior CurrentBehavior { get; set; }
    public Enemy Enemy { get; set; }
    public EnemyApi Api { get; set; }
    public Vector3 InitialPos { get; set; }
    public BulletRenderer BulletRenderer { get; set; }
    public IEnemyStateEventAccepter EventAccepter { get; set; }

    public bool MoveNextBehavior()
    {
        var result = Behaviors.MoveNext();
        CurrentBehavior = Behaviors.Current;
        return result;
    }

    public void ChangeState(string stateName)
    {
        var stateMachine = Enemy.GetComponent<StateMachine>();
        stateMachine.ChangeSubState(stateName, this);
    }
}
