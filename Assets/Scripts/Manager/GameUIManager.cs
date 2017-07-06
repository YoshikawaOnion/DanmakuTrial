using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

/// <summary>
/// ゲームのUIを制御するクラス。
/// </summary>
public class GameUiManager : Singleton<GameUiManager>
{
    public enum GameOverOption
    {
        Retry, Title
    }

    [SerializeField]
    private Image imageHakkeyoiPrefab;
    [SerializeField]
    private Image imageNokottaPrefab;
    [SerializeField]
    private Image imageGamePrefab;
    [SerializeField]
    private Image imageWinPrefab;
    [SerializeField]
    private Image imageLosePrefab;
    [SerializeField]
    private Image scrollPaper;
    [SerializeField]
    private Button retryButton;
    [SerializeField]
    private Button titleButton;
    [SerializeField]
    private Text fpsText;

    public GameUiManager()
    {
        imageHakkeyoiPrefab = null;
        imageNokottaPrefab = null;
        imageGamePrefab = null;
        imageWinPrefab = null;
        imageLosePrefab = null;
        scrollPaper = null;
        retryButton = null;
        titleButton = null;
    }

    protected override void Init()
    {
        scrollPaper.fillAmount = 0;
        var fps = Instantiate(fpsText).GetComponent<Text>();
        fps.transform.SetParent(transform);
        Observable.IntervalFrame(10)
                  .Subscribe(t => fps.text = (1.0f / Time.deltaTime).ToString("##"));
    }

    private ObservableYieldInstruction<float> StartScrollPaperAnimation(float start, float goal)
    {
        int duration = 30;
        scrollPaper.fillAmount = start;

        // durationだけ時間をかけて、fillAmountをstartからgoalまで変化させる
        // 二次曲線によるイージングを用いる
        return Observable.EveryUpdate()
                               .Take(duration)
                               .Select(t => (float)t / duration)
                               .Select(t => -(t - 1) * (t - 1) + 1)
                               .Select(v => start * (1 - v) + goal * v)
                               .Do(v => scrollPaper.fillAmount = v,
                                   () => scrollPaper.fillAmount = goal)
                               .ToYieldInstruction();
    }

    private IEnumerator StartMessageAnimation(Image objectPrefub)
    {
        var str = Instantiate(objectPrefub);
        var aura = Instantiate(objectPrefub);
        yield return StartMessageAnimateIn(str, aura);
        yield return new WaitForSeconds(0.8f);
        Destroy(str.gameObject);
        Destroy(aura.gameObject);
    }

    private IEnumerator StartMessageAnimateIn(Image str, Image aura)
    {
        str.transform.SetParent(transform, false);
        aura.transform.SetParent(transform, false);

        str.transform.localPosition = Vector3.zero;
        aura.transform.localPosition = Vector3.zero;

        var scale = aura.transform.localScale.x;

        // 拡大しながら不透明度を薄くする。二次曲線によるイージングを用いる
        yield return Observable.EveryUpdate()
                               .Take(20)
                               .Select(t => (float)t / 20)
                               .Select(t => -(t - 1) * (t - 1) + 1)
                               .Do(v =>
        {
            aura.transform.localScale = Vector3.one * (v * 0.3f + 1) * scale;
            aura.color = new Color(1, 1, 1, 1 - v);
        })
                               .ToYieldInstruction();
    }

    public IEnumerator AnimateGameStart()
    {
        yield return StartScrollPaperAnimation(0, 1);
        SoundManager.I.PlaySe(SeKind.Start);
        yield return StartMessageAnimation(imageHakkeyoiPrefab);
        yield return StartMessageAnimation(imageNokottaPrefab);
        yield return StartScrollPaperAnimation(1, 0);
    }

    public IEnumerator AnimateWin()
    {
        yield return StartScrollPaperAnimation(0, 1);
        yield return StartMessageAnimation(imageGamePrefab);
        SoundManager.I.PlaySe(SeKind.Win);
        yield return StartMessageAnimation(imageWinPrefab);
        yield return StartScrollPaperAnimation(1, 0);
    }

    public IEnumerator InputGameOverMenu(Action<GameOverOption> returnCallback)
    {
        yield return StartScrollPaperAnimation(0, 1);
        yield return StartMessageAnimation(imageGamePrefab);

        var str = Instantiate(imageLosePrefab);
        var aura = Instantiate(imageLosePrefab);
        yield return StartMessageAnimateIn(str, aura);

        yield return InputGameOverMenu().Do(x => returnCallback(x))
                                        .ToYieldInstruction();

        Destroy(str.gameObject);
        Destroy(aura.gameObject);
        yield return StartScrollPaperAnimation(1, 0);
    }

    public IObservable<GameOverOption> InputGameOverMenu()
    {
        retryButton.gameObject.SetActive(true);
        titleButton.gameObject.SetActive(true);

        var retry = retryButton.OnClickAsObservable()
                               .Select(t => GameOverOption.Retry);
        var title = titleButton.OnClickAsObservable()
                               .Select(t => GameOverOption.Title);

        return retry.Merge(title).First()
                    .Do(t => { }, () =>
          {
              retryButton.gameObject.SetActive(false);
              titleButton.gameObject.SetActive(false);
          });
    }
}
