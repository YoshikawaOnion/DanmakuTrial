using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class TitleUiManager : Singleton<TitleUiManager>
{
    [SerializeField]
    private Button button小結;
    [SerializeField]
    private Button button関脇;
    private EnemyApi api;

    protected override void Init()
    {
        button小結.OnClickAsObservable()
                .Subscribe(x =>
        {
			GameManager.I.EnemyStrategy = new EnemyStrategy小結();
			AppManager.I.ChangeState(AppManager.GameStateName);
        });
        button関脇.OnClickAsObservable()
                .Subscribe(x =>
        {
            GameManager.I.EnemyStrategy = new EnemyStrategy関脇();
            AppManager.I.ChangeState(AppManager.GameStateName);
        });
    }
}
