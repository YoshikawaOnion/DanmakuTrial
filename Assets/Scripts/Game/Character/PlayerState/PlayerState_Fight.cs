using UnityEngine;
using System.Collections;
using UniRx;
using System;

/// <summary>
/// プレイヤーが操作を受け付けて戦っているステート。
/// </summary>
public class PlayerState_Fight : StateMachine
{
    protected static readonly string EnemyTag = "Enemy";
    protected static readonly string EnemyShotTag = "EnemyShot";

	protected CompositeDisposable Disposable { get; private set; }
	protected PlayerStateContext Context { get; private set; }

    protected void EvStateEnter(PlayerStateContext context)
    {
        Context = context;
        Disposable = new CompositeDisposable();

        // 一定時間ごとにショットを撃つ
        Observable.IntervalFrame(context.ShotSpan)
                  .Subscribe(t => Shot())
                  .AddTo(Disposable);
        
        Observable.EveryUpdate()
                  .Subscribe(t => Move())
                  .AddTo(Disposable);
        SetMouseControlUp();


		GameManager.I.GameEvents.OnPlayerExitsFightArea
				   .Subscribe(u =>
		{
			SoundManager.I.PlaySe(SeKind.PlayerDamaged);
			context.ChangeState(Player.StateNameLose);
		})
				   .AddTo(Disposable);
        GameManager.I.GameEvents.OnEnemyDefeated
                   .Subscribe(u => context.ChangeState(Player.StateNameWin))
                   .AddTo(Disposable);
        GameManager.I.GameEvents.OnNextRound
                   .Subscribe(u => context.ChangeState(Player.StateNameOpening))
                   .AddTo(Disposable);

        // MissingReferenceException対策
        Observable.EveryUpdate()
                  .SkipWhile(t => context.Player != null)
                  .Take(1)
                  .Subscribe(t => Disposable.Dispose())
                  .AddTo(Disposable);
    }


    protected override void EvStateExit()
    {
        Disposable.Dispose();
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
            .Subscribe(delta => Context.Player.transform.position += delta)
            .AddTo(Disposable);
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
        Context.Player.transform.position += velocity
            * Context.MoveSpeed * Def.UnitPerPixel;
    }

    private void Shot()
	{
		var obj = Instantiate(Context.ShotObject);
		obj.transform.position = Context.ShotSource.transform.position;
		obj.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
		SoundManager.I.PlaySe(SeKind.PlayerShot, 0.1f);
    }
}
