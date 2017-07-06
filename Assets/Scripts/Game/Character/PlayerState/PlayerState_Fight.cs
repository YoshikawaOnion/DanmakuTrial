using UnityEngine;
using System.Collections;
using UniRx;
using System;

/// <summary>
/// プレイヤーが操作を受け付けて戦っているステート。
/// </summary>
public class PlayerState_Fight : StateMachine
{
    private static readonly string EnemyTag = "Enemy";
    private static readonly string EnemyShotTag = "EnemyShot";

	private CompositeDisposable disposable { get; set; }
	private PlayerStateContext context { get; set; }

    protected void EvStateEnter(PlayerStateContext context)
    {
        this.context = context;
        disposable = new CompositeDisposable();

        // 一定時間ごとにショットを撃つ
        Observable.IntervalFrame(context.Player.ShotSpan)
                  .Subscribe(t => Shot())
                  .AddTo(disposable);
        
        Observable.EveryUpdate()
                  .Subscribe(t => Move())
                  .AddTo(disposable);
        SetMouseControlUp();

        GameManager.I.GameEvents.OnPlayerExitsFightArea
                   .Subscribe(u => context.ChangeState(Player.StateNameLose))
                   .AddTo(disposable);
        GameManager.I.GameEvents.OnEnemyDefeated
                   .Subscribe(u => context.ChangeState(Player.StateNameWin))
                   .AddTo(disposable);
        GameManager.I.GameEvents.OnNextRound
                   .Subscribe(u => context.ChangeState(Player.StateNameOpening))
                   .AddTo(disposable);

        GameManager.I.GameEvents.OnHitEnemyShot
                   .Subscribe(collider => OnHit(collider))
                   .AddTo(disposable);

        // MissingReferenceException対策
        Observable.EveryUpdate()
                  .SkipWhile(t => context.Player != null)
                  .Take(1)
                  .Subscribe(t => disposable.Dispose())
                  .AddTo(disposable);
    }

    private void OnHit(Collider2D collider)
	{
		if (collider.tag == EnemyShotTag)
		{
			Destroy(collider.gameObject);
			context.Player.Rigidbody.AddForce(
                context.PushOnShoot * Def.UnitPerPixel);
            context.ChangeState(Player.StateNameDamaged);
		}
		if (collider.tag == EnemyTag)
		{
			context.Player.Rigidbody.AddForce(
				context.PushOnCollide * Def.UnitPerPixel);
			context.ChangeState(Player.StateNameDamaged);
		}
    }


    protected override void EvStateExit()
    {
        disposable.Dispose();
    }

    private void SetMouseControlUp()
    {
        var drag = Observable.EveryUpdate()
                        .SkipWhile(t => !Input.GetMouseButton(0))
                        .Select(t => Input.mousePosition)
                        .Select(p => SpriteStudioManager.I.MainCamera.ScreenToWorldPoint(p))
                        .TakeWhile(t => !Input.GetMouseButtonUp(0));
        drag.Zip(drag.Skip(1), (arg1, arg2) => arg2 - arg1)
            .Repeat()
            .Subscribe(delta => context.Player.transform.position += delta)
            .AddTo(disposable);
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
        context.Player.transform.position += velocity
            * context.MoveSpeed * Def.UnitPerPixel;
    }

    private void Shot()
	{
		var obj = Instantiate(context.ShotObject);
		obj.transform.position = context.ShotSource.transform.position;
		obj.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
		SoundManager.I.PlaySe(SeKind.PlayerShot, 0.1f);
    }
}
