using UnityEngine;
using System.Collections;

/// <summary>
/// 敵がプレイヤーに勝利した（プレイヤーが敗北した）ステート。
/// </summary>
public class EnemyState_Win : StateMachine
{
    private EnemyStateContext context { get; set; }

    protected void EvStateEnter(EnemyStateContext context)
    {
        this.context = context;
    }
}
