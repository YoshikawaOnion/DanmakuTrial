using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// ゲームプレイ中に発生する様々なイベントを購読できる IObservable を提供するインターフェース。
/// </summary>
public interface IGameEventObservables
{
    /// <summary>
    /// 次の弾幕パターンへ移ることが決定した瞬間に値が発行されるストリームを取得します。
    /// </summary>
	IObservable<Unit> OnNextRound { get; }
    /// <summary>
    /// 敵がプレイヤーの弾に衝突した瞬間に Collider2D の値が発行されるストリームを取得します。
    /// </summary>
	IObservable<Collider2D> OnHitPlayerShot { get; }
    /// <summary>
    /// 敵が土俵を飛び出した瞬間に値が発行されるストリームを取得します。
    /// </summary>
	IObservable<Unit> OnEnemyExitsFightArea { get; }
    /// <summary>
    /// プレイヤーが土俵を飛び出した瞬間に値が発行されるストリームを取得します。
    /// </summary>
	IObservable<Unit> OnPlayerExitsFightArea { get; }
    /// <summary>
    /// 新たな弾幕パターンが開始した瞬間に値が発行されるストリームを取得します。
    /// </summary>
	IObservable<Unit> OnRoundStart { get; }
    /// <summary>
    /// 敵が安全圏を飛び出した瞬間に値が発行されるストリームを取得します。
    /// </summary>
    IObservable<Unit> OnEnemyExitsSafeArea { get; }
    /// <summary>
    /// 敵が安全圏に戻ってきた瞬間に値が発行されるストリームを取得します。
    /// </summary>
    IObservable<Unit> OnEnemyEntersSafeArea { get; }
    /// <summary>
    /// 敵の全ての弾幕に勝利した瞬間に値が発行されるストリームを取得します。
    /// </summary>
    IObservable<Unit> OnEnemyDefeated { get; }
    /// <summary>
    /// プレイヤーが敵弾に衝突した瞬間に Collider2D の値が発行されるストリームを取得します。
    /// </summary>
    /// <value>The on hit enemy shot.</value>
    IObservable<Collider2D> OnHitEnemyShot { get; }
}
