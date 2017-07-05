using UnityEngine;
using System.Collections;

/// <summary>
/// GameState_** 系のクラスに現在の文脈を連絡するコンテキスト クラス。
/// </summary>
public class GameStateContext : EventContext
{
    public IGameStateEventAccepter EventAccepter { get; set; }

    /// <summary>
    /// GameManager のステートを遷移させます。
    /// </summary>
    /// <param name="stateName">遷移先のステート名。</param>
    public void ChangeState(string stateName)
    {
        GameManager.I.ChangeState(stateName, this);
    }
}
