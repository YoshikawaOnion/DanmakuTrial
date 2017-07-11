using UnityEngine;
using System.Collections;

/// <summary>
/// 敵がプレイヤーに弾幕を全て攻略して倒されたステート。
/// </summary>
public class EnemyState_Defeated : StateMachine
{
	protected void EvStateEnter(EnemyStateContext context)
	{
        StartCoroutine(context.Enemy.Die());
	}
}
