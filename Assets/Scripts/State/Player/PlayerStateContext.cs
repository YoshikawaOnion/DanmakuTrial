using UnityEngine;
using System.Collections;

public class PlayerStateContext : EventContext
{
    public Player Player { get; set; }
    public GameObject ShotObject { get; set; }
    public GameObject ShotSource { get; set; }
    public float MoveSpeed { get; set; }
    public GameObject Sprite { get; set; }
    public Vector2 PushOnShoot { get; set; }
    public Vector2 PushOnCollideEnemy { get; set; }
    public Vector2 PushOnHitMob { get; set; }
    public int ShotSpan { get; set; }
    public PlayerDamagedCameraAsset DamageAsset { get; set; }
       
    public void ChangeState(string stateName)
    {
        if (Player != null)
		{
			Player.GetComponent<StateMachine>()
				  .ChangeSubState(stateName, this);   
        }
    }
}
