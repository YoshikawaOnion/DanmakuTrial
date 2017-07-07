#pragma warning disable CS0649
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Player : MonoBehaviour {
    public static readonly string StateNameOpening = "PlayerState_Opening";
    public static readonly string StateNameNeutral = "PlayerState_Neutral";
    public static readonly string StateNameWin = "PlayerState_Win";
    public static readonly string StateNameLose = "PlayerState_Lose";
    public static readonly string StateNameDamaged = "PlayerState_Damaged";

    [Tooltip("最高速度[px/frame]")]
    [SerializeField]
    private float speed = 1;
	[Tooltip("ショットの時間間隔[frame]")]
	[SerializeField]
    private int shotSpan = 20;
	[SerializeField]
	private GameObject shotObject;
	[SerializeField]
	private GameObject shotSource;
	[SerializeField]
	private GameObject playerSprite = null;
	[Tooltip("被弾時の押し出し[px*kg/sec^2]")]
	[SerializeField]
	private Vector2 pushOnShoot = new Vector2(0, -7000);
	[Tooltip("敵と接触時の押し出し[px*kg/sec^2]")]
	[SerializeField]
	private Vector2 pushOnCollide = new Vector2(0, -10000);
	[Tooltip("モブと接触時の押し出し[px*kg/sec^2]")]
	[SerializeField]
	private Vector2 pushOnHitMob = new Vector2(0, -8000);
    [SerializeField]
    private GameObject smokePrefab;
    [SerializeField]
    private GameObject playerHitPrefab;

	public Rigidbody2D Rigidbody { get; private set; }

    private StateMachine stateMachine;
    private CompositeDisposable disposable;
    private PlayerStateContext context;

	// Use this for initialization
	void Start ()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<StateMachine>();
        disposable = new CompositeDisposable();

        context = new PlayerStateContext
        {
            Player = this,
            ShotObject = shotObject,
            ShotSource = shotSource,
            MoveSpeed = speed,
            PushOnCollideEnemy = pushOnCollide,
            PushOnShoot = pushOnShoot,
            PushOnHitMob = pushOnHitMob,
            Sprite = playerSprite,
            ShotSpan = shotSpan,
			DamageAsset = Resources.Load<PlayerDamagedCameraAsset>
						 ("ScriptableAsset/PlayerDamagedCameraAsset"),
		};
        context.ChangeState(StateNameOpening);

        var smoke = Instantiate(smokePrefab);
        smoke.transform.SetParent(transform);
        smoke.transform.localPosition = Vector3.zero;

        GameManager.I.GameEvents.OnHitEnemyShot
                                .Subscribe(c =>
                                {
                                    ShowHitEffect();
                                    ShakeCamera();
                                })
                   .AddTo(disposable);

        GameManager.I.GameEvents.OnHitEnemyShot
                   .Subscribe(c => SoundManager.I.PlaySe(SeKind.PlayerDamaged))
                   .AddTo(disposable);
	}

    private void ShowHitEffect()
    {
        var effect = Instantiate(playerHitPrefab);
        effect.transform.SetParent(transform);
        effect.transform.localPosition = Vector3.zero;
        Observable.TimerFrame(60)
                  .Subscribe(t => Destroy(effect));
    }

    private void OnDestroy()
    {
        disposable.Dispose();
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

	private void ShakeCamera()
	{
		var shakeTime = TimeSpan.FromSeconds(context.DamageAsset.ShakeTime);
		var size = context.DamageAsset.ShakeSize;
		Observable.EveryUpdate()
				  .TakeUntil(Observable.Timer(shakeTime))
				  .Select(t => new
				  {
					  X = Mathf.Sin(t * 8) * size * Def.UnitPerPixel,
					  Y = Mathf.Cos(t * 8) * size * Def.UnitPerPixel,
				  })
				  .Subscribe(p => SetCameraPos(new Vector3(p.X, p.Y, 0)),
							 () => SetCameraPos(Vector3.zero))
				  .AddTo(disposable);
	}

    private void SetCameraPos(Vector3 pos)
    {
        SpriteStudioManager.I.MainCamera.transform.position = pos;
    }
}
