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
    private CompositeDisposable disposable;
    private Script_SpriteStudio_Root sprite;

    protected new void EvStateEnter(PlayerStateContext context)
    {
        base.EvStateEnter(context);

        this.context = context;
        sprite = context.Sprite.GetComponent<Script_SpriteStudio_Root>();

        disposable = new CompositeDisposable();
        SetAnimation("Damage");

        ShakeCamera();
        WaitForTransitToNeutral(context);
    }

    private void WaitForTransitToNeutral(PlayerStateContext context)
    {
        // 一定時間被弾しなかったら Neutral 状態へ戻る
        GameManager.I.GameEvents.OnHitEnemyShot
                   .Throttle(TimeSpan.FromSeconds(1))
                   .Subscribe(u =>
                   {
                       SetAnimation("Player");
                       context.ChangeState(Player.StateNameFighting);
                   })
                   .AddTo(disposable);
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
                  .AddTo(disposable);
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
