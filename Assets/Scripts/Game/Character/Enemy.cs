using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using Ist;

/// <summary>
/// 敵キャラクターを制御するクラス。
/// </summary>
public class Enemy : MonoBehaviour
{
    public static readonly string OpeningStateName = "EnemyState_Opening";
    public static readonly string NeutralStateName = "EnemyState_Neutral";
    public static readonly string GutsStateName = "EnemyState_Guts";
    public static readonly string NextRoundStateName = "EnemyState_NextRound";
	public static readonly string DefeatedStateName = "EnemyState_Defeated";
	public static readonly string WinStateName = "EnemyState_Win";

	public GameObject LeftHand;
	public GameObject RightHand;
    public Vector2 PushOnShoot = new Vector2(0, 100);
    public Vector2 RecoverOnGuts = new Vector2(0, -7);

    [SerializeField]
    private Script_SpriteStudio_Root spriteStudioRoot;
	[SerializeField]
	private GameObject smokeEffectPrefab;

	public EnemyStrategy Strategy { get; set; }
	public Rigidbody2D Rigidbody { get; private set; }
	public BulletManager BulletRenderer { get; set; }
    public Vector3 InitialPosition { get; private set; }
    public IEnemyEventAccepter EventAccpter { get; set; }

	// Use this for initialization
	void Start ()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        InitialPosition = transform.position;

        var smoke = Instantiate(smokeEffectPrefab);
        smoke.transform.SetParent(transform);
        smoke.transform.localPosition = Vector3.zero;
	}

    private void OnDestroy()
    {
        Rigidbody = null;
        Strategy = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventAccpter.OnHitPlayerShotSubject.OnNext(collision);
    }

    public IEnumerator Die()
	{
		spriteStudioRoot.enabled = false;

		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);        
    }

}
