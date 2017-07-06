using UnityEngine;
using System.Collections;
using UniRx;
using System;

/// <summary>
/// プレイヤーが被弾しているステート。
/// </summary>
public class PlayerState_Damaged : PlayerState_Fight
{
    private PlayerStateContext context;
    private Script_SpriteStudio_Root sprite;
    private CompositeDisposable itsOwnDisposable;

    protected new void EvStateEnter(PlayerStateContext context)
    {
        base.EvStateEnter(context);
        this.context = context;
        itsOwnDisposable = new CompositeDisposable();

        sprite = context.Sprite.GetComponent<Script_SpriteStudio_Root>();
        SetAnimation("Damage");
        ShakeCamera();
        WaitForTransitToNeutral(context);

		GameManager.I.GameEvents.OnHitEnemyShot
				   .Subscribe(collider => OnHit(collider))
				   .AddTo(itsOwnDisposable);
	}

	private void OnHit(Collider2D collider)
	{
		if (collider.tag == EnemyShotTag)
		{
			Destroy(collider.gameObject);
			Context.Player.Rigidbody.AddForce(
				Context.PushOnShoot * Def.UnitPerPixel);
		}
		if (collider.tag == EnemyTag)
		{
			Context.Player.Rigidbody.AddForce(
				Context.PushOnCollide * Def.UnitPerPixel);
		}
	}

    protected override void EvStateExit()
    {
        base.EvStateExit();
        SetAnimation("Player");
        itsOwnDisposable.Dispose();
    }

    private void WaitForTransitToNeutral(PlayerStateContext context)
    {
        var source = GameManager.I.GameEvents.OnHitEnemyShot
                                .Select(c => Unit.Default)
                                .Merge(Observable.Return(Unit.Default));

        // 一定時間被弾しなかったら Neutral 状態へ戻る
        source.Throttle(TimeSpan.FromSeconds(1))
              .Subscribe(u =>
              {
                  SetAnimation("Player");
                  context.ChangeState(Player.StateNameNeutral);
              })
              .AddTo(itsOwnDisposable);
    }

    private void ShakeCamera()
    {
        Observable.EveryUpdate()
                  .TakeUntil(Observable.Timer(TimeSpan.FromSeconds(0.3)))
                  .Select(t => new
                  {
                      X = Mathf.Sin(t * 8) * 2 * Def.UnitPerPixel,
                      Y = Mathf.Cos(t * 8) * 2 * Def.UnitPerPixel,
                  })
                  .Subscribe(p => SetCameraPos(new Vector3(p.X, p.Y, 0)),
                             () => SetCameraPos(Vector3.zero))
                  .AddTo(itsOwnDisposable);
    }

    private void SetCameraPos(Vector3 pos)
    {
        SpriteStudioManager.I.MainCamera.transform.position = pos;
    }

    private void SetAnimation(string name)
    {
        var index = sprite.IndexGetAnimation(name);
        sprite.AnimationPlay(index);
    }
}
