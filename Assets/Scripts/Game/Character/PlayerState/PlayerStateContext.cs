using UnityEngine;
using System.Collections;

public class PlayerStateContext : EventContext
{
    public Player Player { get; set; }
    public GameObject ShotObject { get; set; }
    public GameObject ShotSource { get; set; }
    public float MoveSpeed { get; set; }

    public void ChangeState(string stateName)
    {
        Player.GetComponent<StateMachine>()
              .ChangeSubState(stateName, this);
    }
}
