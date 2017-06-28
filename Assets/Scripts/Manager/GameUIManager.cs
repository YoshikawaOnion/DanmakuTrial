using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameUIManager : Singleton<GameUIManager>
{
    public enum Message
    {
        はっけよい, のこった, 勝負あり, 勝ち, 負け
    }

	public Image ImageはっけよいPrefub;
	public Image ImageのこったPrefub;
    public Image Image勝負ありPrefub;
    public Image Image勝ちPrefub;
    public Image Image負けPrefub;
    public Image ScrollPaper;

    private Vector3 ScrollPaperSize;

    protected override void Init()
    {
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
							   .Do(v => ScrollPaper.fillAmount = v)
							   .ToYieldInstruction();
    }

    private IEnumerator StartMessageAnimation(Image objectPrefub)
	{
		var str = Instantiate(objectPrefub);
		var aura = Instantiate(objectPrefub);
		str.transform.parent = transform;
		aura.transform.parent = transform;
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

		yield return new WaitForSeconds(0.8f);
		Destroy(str.gameObject);
		Destroy(aura.gameObject);
    }

    public IEnumerator AnimationGameStart()
    {
        yield return StartScrollPaperAnimation(0, 1);
        yield return StartMessageAnimation(ImageはっけよいPrefub);
        yield return StartMessageAnimation(ImageのこったPrefub);
		yield return StartScrollPaperAnimation(1, 0);
    }

    public IEnumerator AnimationWin()
	{
		yield return StartScrollPaperAnimation(0, 1);
		yield return StartMessageAnimation(Image勝負ありPrefub);
		yield return StartMessageAnimation(Image勝ちPrefub);
		yield return StartScrollPaperAnimation(1, 0);
    }

	public IEnumerator AnimationGameOver()
	{
		yield return StartScrollPaperAnimation(0, 1);
		yield return StartMessageAnimation(Image勝負ありPrefub);
        yield return StartMessageAnimation(Image負けPrefub);
		yield return StartScrollPaperAnimation(1, 0);
	}
}
