using UnityEngine;
using System.Collections;

public class EnemyState_Win : StateMachine
{
    private EnemyStateContext context { get; set; }

    protected void EvStateEnter(EnemyStateContext context)
    {
        this.context = context;
    }
}
