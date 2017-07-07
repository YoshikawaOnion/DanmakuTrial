using UnityEngine;
using System.Collections;
using System;
using UniRx;

public static class AnimationExtentions
{
    public static IDisposable Move(this MonoBehaviour obj, Vector3 goal, int durationFrame)
    {
		var prevPos = obj.transform.position;
		return Observable.EveryUpdate()
				  .Take(durationFrame)
				  .Subscribe(t =>
		{
			var pos = (prevPos * (durationFrame - t) + goal * t) / durationFrame;
			obj.transform.position = pos;
		});
    }
}
