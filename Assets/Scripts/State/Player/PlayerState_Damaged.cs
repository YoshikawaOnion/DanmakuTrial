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
        WaitForTransitToNeutral(context);
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

    private void SetAnimation(string name)
    {
        var index = sprite.IndexGetAnimation(name);
        sprite.AnimationPlay(index);
    }
}
