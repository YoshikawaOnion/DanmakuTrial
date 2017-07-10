#pragma warning disable CS0649

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// タイトル画面のUIを制御するクラス。
/// </summary>
public class TitleUiManager : Singleton<TitleUiManager>
{
    public IObservable<EnemyStrategy> LevelSelectedObservable { get; set; }

    [SerializeField]
    private Button buttonKomusubi;
    [SerializeField]
	private Button buttonSekiwaki;
	[SerializeField]
	private Button buttonOozeki;

    protected override void Init()
    {
        var komusubi = buttonKomusubi.OnClickAsObservable()
                                     .Select(x => (EnemyStrategy)new EnemyStrategyKomusubi());
        var sekiwaki = buttonSekiwaki.OnClickAsObservable()
                                     .Select(x => (EnemyStrategy)new EnemyStrategySekiwaki());
        var oozeki = buttonOozeki.OnClickAsObservable()
                                 .Select(x => (EnemyStrategy)new EnemyStrategyOozeki());
        LevelSelectedObservable = komusubi.Merge(sekiwaki)
                                          .Merge(oozeki);
    }

    protected override void OnDestroy()
    {
        LevelSelectedObservable = null;
    }
}
