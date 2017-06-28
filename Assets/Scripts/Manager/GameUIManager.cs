using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class GameUIManager : Singleton<GameUIManager>
{
    public enum GameOverOption
    {
        Retry, Title
    }

    public Image ImageはっけよいPrefub;
    public Image ImageのこったPrefub;
    public Image Image勝負ありPrefub;
    public Image Image勝ちPrefub;
    public Image Image負けPrefub;
    public Image ScrollPaper;
    public Button RetryButton;
	public Button TitleButton;

	public AudioClip StartSound;
	public AudioClip WinSound;
	internal AudioSource AudioSource;

    private Vector3 ScrollPaperSize;

    protected override void Init()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private ObservableYieldInstruction<float> StartScrollPaperAnimation(float start, float goal)
    {
        int duration = 30;
        ScrollPaper.fillAmount = start;
        return Observable.EveryUpdate()
                               .Take(duration)
                               .Select(t => (float)t / duration)
                               .Select(t => -(t - 1) * (t - 1) + 1)
                               .Select(v => start * (1 - v) + goal * v)
                               .Do(v => ScrollPaper.fillAmount = v,
                                   () => ScrollPaper.fillAmount = goal)
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
        AudioSource.PlayOneShot(StartSound);
        yield return StartMessageAnimation(ImageはっけよいPrefub);
        yield return StartMessageAnimation(ImageのこったPrefub);
        yield return StartScrollPaperAnimation(1, 0);
    }

    public IEnumerator AnimateWin()
    {
        yield return StartScrollPaperAnimation(0, 1);
        yield return StartMessageAnimation(Image勝負ありPrefub);
        AudioSource.PlayOneShot(WinSound);
        yield return StartMessageAnimation(Image勝ちPrefub);
        yield return StartScrollPaperAnimation(1, 0);
    }

    public IEnumerator InputGameOverMenu(Action<GameOverOption> returnCallback)
	{
		yield return StartScrollPaperAnimation(0, 1);
		yield return StartMessageAnimation(Image勝負ありPrefub);

        var str = Instantiate(Image負けPrefub);
        var aura = Instantiate(Image負けPrefub);
		yield return StartMessageAnimateIn(str, aura);

        yield return InputGameOverMenu().Do(x => returnCallback(x))
                                        .ToYieldInstruction();

		Destroy(str.gameObject);
		Destroy(aura.gameObject);
		yield return StartScrollPaperAnimation(1, 0);
    }

    public IObservable<GameOverOption> InputGameOverMenu()
    {
        RetryButton.gameObject.SetActive(true);
        TitleButton.gameObject.SetActive(true);

        var retry = RetryButton.OnClickAsObservable()
                               .Select(t => GameOverOption.Retry);
        var title = TitleButton.OnClickAsObservable()
                               .Select(t => GameOverOption.Title);

        return retry.Merge(title).First()
                    .Do(t => { }, () =>
          {
              RetryButton.gameObject.SetActive(false);
              TitleButton.gameObject.SetActive(false);
          });
    }
}
