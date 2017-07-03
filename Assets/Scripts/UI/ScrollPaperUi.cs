using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;
using System.Collections.Generic;
#pragma warning disable CS0649
using System;

[Obsolete]
public class ScrollPaperUi : MonoBehaviour
{
    [SerializeField]
    private Image paperImage;
    [SerializeField]
    private float startingFillState;
    [SerializeField]
    private float endingFillState;
    [SerializeField]
    private int animationDurationFrame;

	// Use this for initialization
	void Start()
	{
		paperImage.fillAmount = startingFillState;
		Observable.EveryUpdate()
			      .Take(animationDurationFrame)
                  .Select(t =>
        {
            var time = (float)t / animationDurationFrame;
            var easing = -(time - 1) * (time - 1) + 1;
            return startingFillState * (1 - easing)
                + endingFillState * easing;
        })
			      .Subscribe(v => paperImage.fillAmount = v,
				            () => paperImage.fillAmount = endingFillState);
	}
}
