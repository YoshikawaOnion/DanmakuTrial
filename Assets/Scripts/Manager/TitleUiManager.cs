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
    private Button button小結;
    [SerializeField]
    private Button button関脇;

    private System.Action m_onPressKoimusubiCallback;
    public System.Action onPressKoimusubiCallback { get { return m_onPressKoimusubiCallback; } set { m_onPressKoimusubiCallback = value; } }

    protected override void Init()
    {
        button小結.OnClickAsObservable()
                .Subscribe(x =>
        {
            GameManager.I.EnemyStrategy = new EnemyStrategy小結();
            AppManager.I.ChangeState(AppManager.GameStateName);

            if (m_onPressKoimusubiCallback != null)
            {
                m_onPressKoimusubiCallback();
            }
        });
        button関脇.OnClickAsObservable()
                .Subscribe(x =>
        {
            GameManager.I.EnemyStrategy = new EnemyStrategy関脇();
            AppManager.I.ChangeState(AppManager.GameStateName);
        });
    }
}
