using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// タイトル画面のUIを制御するクラス。
/// </summary>
public class TitleUiManager : Singleton<TitleUiManager>
{
    [SerializeField]
    private Button buttonKomusubi;
    [SerializeField]
    private Button buttonSekiwaki;

    private System.Action m_onPressKoimusubiCallback;
    public System.Action onPressKoimusubiCallback { get { return m_onPressKoimusubiCallback; } set { m_onPressKoimusubiCallback = value; } }

    protected override void Init()
    {
        buttonKomusubi.OnClickAsObservable()
                .Subscribe(x =>
        {
            GameManager.I.EnemyStrategy = new EnemyStrategy小結();
            AppManager.I.ChangeState(AppManager.GameStateName);

            if (m_onPressKoimusubiCallback != null)
            {
                m_onPressKoimusubiCallback();
            }
        });
        buttonSekiwaki.OnClickAsObservable()
                .Subscribe(x =>
        {
            GameManager.I.EnemyStrategy = new EnemyStrategy関脇();
            AppManager.I.ChangeState(AppManager.GameStateName);
        });
    }
}
