#pragma warning disable CS0649
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour {
    public static readonly string StateNameOpening = "PlayerState_Opening";
    public static readonly string StateNameFighting = "PlayerState_Fight";
    public static readonly string StateNameWin = "PlayerState_Win";
    public static readonly string StateNameLose = "PlayerState_Lose";

    [Tooltip("最高速度[px/frame]")]
    [SerializeField]
    private float speed = 0.1f;
    public int ShotSpan = 20;
	[SerializeField]
	private GameObject shotObject;
	[SerializeField]
	private GameObject shotSource;

    [SerializeField]
    private PlayerDamageArea damageArea;

    public bool IsEnabled
    {
        get { return isEnabled; }
	}
    public bool IsDefeated { get; set; }

	public Rigidbody2D Rigidbody { get; private set; }
	private bool isEnabled;
    private StateMachine stateMachine;

	// Use this for initialization
	void Start ()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<StateMachine>();
        isEnabled = false;
        damageArea.Owner = this;
        IsDefeated = false;

        var context = new PlayerStateContext
        {
            Player = this,
            ShotObject = shotObject,
            ShotSource = shotSource,
            MoveSpeed = speed,
        };
        context.ChangeState(StateNameOpening);
	}

    /// <summary>
    /// プレイヤーに移動を強制します。
    /// </summary>
    /// <param name="goal">移動の目的地点。</param>
    /// <param name="durationFrame">移動にかかる時間。</param>
    public void ForceToMove(Vector3 goal, int durationFrame)
    {
        var initial = transform.position;
        if (Rigidbody != null)
		{
			Rigidbody.velocity = Vector3.zero;
        }
        // 2次曲線によるイージングで移動します。
        Observable.EveryUpdate()
                  .Take(durationFrame)
                  .Select(t => (float)t / durationFrame)
                  .Select(t => -(t - 1) * (t - 1) + 1)
                  .Select(t => initial * (1 - t) + goal * t)
                  .Subscribe(p => transform.position = p);
    }
}
