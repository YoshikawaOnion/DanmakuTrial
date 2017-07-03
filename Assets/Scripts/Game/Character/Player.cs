using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour {
    [Tooltip("最高速度[px/frame]")]
	public float Speed = 0.1f;
	[Tooltip("マウス移動速度[px/frame]")]
    public float MouseSpeed = 0.1f;
	public int shotSpan = 20;
    public bool isDefeated;

    public GameObject ShotObject;
    public GameObject ShotSource;
    public PlayerDamageArea DamageArea;
    public AudioClip ShootSound;
    public AudioClip DamageSound;

    public bool IsEnabled
    {
        get { return isEnabled; }
    }

    internal Rigidbody2D rigidBody;
    private Vector3 shotPosition;
    private int shotTime = 0;
    private Script_SpriteStudio_Root sprite;
	private bool isEnabled;
    private IDisposable mouseSubscription;
    internal AudioSource AudioSource;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();
        shotPosition = ShotObject.transform.localPosition;
        SetSpriteUp();
        isEnabled = false;
        DamageArea.Owner = this;
        isDefeated = false;
	}

	// Update is called once per frame
	void Update()
	{
        if (isEnabled && !isDefeated)
		{
			Move();
			Shot();
        }
        else
        {
            rigidBody.velocity = Vector3.zero;
        }
    }

    private void SetSpriteUp()
    {
        var spriteControl = GetComponent<Script_SpriteStudio_ControlPrefab>();
        var prefab = spriteControl.PrefabUnderControl as GameObject;
        sprite = prefab.GetComponent<Script_SpriteStudio_Root>();
    }

    private void SetMouseControlUp()
    {
        var drag = Observable.EveryUpdate()
                             .Where(t => isEnabled && !isDefeated)
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
			var obj = Instantiate(ShotObject);
            obj.transform.position = ShotSource.transform.position;
            obj.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
            AudioSource.PlayOneShot(ShootSound, 0.1f);
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
        transform.position += velocity * Speed * Def.UnitPerPixel;
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

    /// <summary>
    /// プレイヤーに移動を強制します。
    /// </summary>
    /// <param name="goal">移動の目的地点。</param>
    /// <param name="durationFrame">移動にかかる時間。</param>
    public void ForceToMove(Vector3 goal, int durationFrame)
    {
        var initial = transform.position;
        if (rigidBody != null)
		{
			rigidBody.velocity = Vector3.zero;
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
