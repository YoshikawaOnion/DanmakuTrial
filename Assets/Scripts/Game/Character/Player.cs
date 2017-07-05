#pragma warning disable CS0649
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour {
    [Tooltip("最高速度[px/frame]")]
    [SerializeField]
    private float speed = 0.1f;
	[SerializeField]
	private int shotSpan = 20;
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
    public IObservable<Unit> OnExitFightingAreaObservable
    {
        get { return onExitFightingArea; }
    }

	public Rigidbody2D Rigidbody { get; private set; }
    private int shotTime = 0;
	private bool isEnabled;
    private IDisposable mouseSubscription;
    private IDisposable defeatedSubscription;
    private Subject<Unit> onExitFightingArea;

	// Use this for initialization
	void Start ()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        onExitFightingArea = new Subject<Unit>();
        isEnabled = false;
        damageArea.Owner = this;
        IsDefeated = false;

        defeatedSubscription = GameManager.I.GameEvents.OnPlayerExitsFightArea
                                          .Subscribe(u =>
        {
            IsDefeated = true;
            SoundManager.I.PlaySe(SeKind.PlayerDamaged);
        });
	}

	// Update is called once per frame
	void Update()
	{
        if (isEnabled && !IsDefeated)
		{
			Move();
			Shot();
        }
        else
        {
            Rigidbody.velocity = Vector3.zero;
        }
    }

    private void SetMouseControlUp()
    {
        var drag = Observable.EveryUpdate()
                             .Where(t => isEnabled && !IsDefeated)
			                 .SkipWhile(t => !Input.GetMouseButton(0))
			                 .Select(t => Input.mousePosition)
                             .Select(p => SpriteStudioManager.I.MainCamera.ScreenToWorldPoint(p))
			                 .TakeWhile(t => !Input.GetMouseButtonUp(0));
        mouseSubscription = drag.Zip(drag.Skip(1), (arg1, arg2) => arg2 - arg1)
            .Repeat()
            .Subscribe(delta =>
		{
            transform.position += delta;
        });
    }

    private void Shot()
    {
        if (shotTime >= shotSpan)
		{
			var obj = Instantiate(shotObject);
            obj.transform.position = shotSource.transform.position;
            obj.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
            SoundManager.I.PlaySe(SeKind.PlayerShot, 0.1f);
            shotTime = 0;
		}
		++shotTime;
    }

    private void Move()
    {
        Vector3 velocity = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
			velocity.x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity.x += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity.y -= 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity.y += 1;
        }

        var length = velocity.magnitude;
        if (length != 0)
        {
            velocity /= length;
        }
        transform.position += velocity * speed * Def.UnitPerPixel;
    }

    /// <summary>
    /// プレイヤーの動作を開始します。
    /// </summary>
    public void StartAction()
    {
        isEnabled = true;
        SetMouseControlUp();
    }

    /// <summary>
    /// プレイヤーの動作を停止します。
    /// </summary>
    public void StopAction()
    {
        isEnabled = false;
        if (mouseSubscription != null)
		{
			mouseSubscription.Dispose();
			mouseSubscription = null;
        }
    }

    private void OnDestroy()
    {
        if (mouseSubscription != null)
        {
            mouseSubscription.Dispose();
        }
        defeatedSubscription.Dispose();
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
