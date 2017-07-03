using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class TitleUiManager : Singleton<TitleUiManager>
{
    public Button Button小結;
    public Button Button関脇;
    private EnemyApi api;

    protected override void Init()
    {
        Button小結.OnClickAsObservable()
                .Subscribe(x =>
        {
			GameManager.I.EnemyStrategy = new EnemyStrategy小結();
			AppManager.I.ChangeState(AppManager.GameStateName);
        });
        Button関脇.OnClickAsObservable()
                .Subscribe(x =>
        {
            GameManager.I.EnemyStrategy = new EnemyStrategy関脇();
            AppManager.I.ChangeState(AppManager.GameStateName);
        });
    }
}
