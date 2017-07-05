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

    /// <summary>
    /// CurrentBehavior を次の弾幕パターンへ移行します。
    /// </summary>
    /// <returns>弾幕パターンを全て攻略していれば <c>true</c>、まだ攻略すべき弾幕が残っていれば <c>false</c>。</returns>
    public bool MoveNextBehavior()
    {
        var result = Behaviors.MoveNext();
        CurrentBehavior = Behaviors.Current;
        return result;
    }

    /// <summary>
    /// この <see cref="EnemyStateContext"/> に紐づけられている Enemy のステートを遷移させます。
    /// </summary>
    /// <param name="stateName">遷移先のステート名。</param>
    public void ChangeState(string stateName)
    {
        var stateMachine = Enemy.GetComponent<StateMachine>();
        stateMachine.ChangeSubState(stateName, this);
    }
}
