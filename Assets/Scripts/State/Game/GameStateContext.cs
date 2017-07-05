using UnityEngine;
using System.Collections;

public class GameStateContext : EventContext
{
    public IGameStateEventAccepter EventAccepter { get; set; }

    public void ChangeState(string stateName)
    {
        GameManager.I.ChangeState(stateName, this);
    }
}
