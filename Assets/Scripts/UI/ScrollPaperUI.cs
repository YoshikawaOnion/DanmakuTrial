using UnityEngine;
using System.Collections;
using UniRx;

public class ScrollPaperUI : MonoBehaviour
{
    public string ImageFilePath;

	// Use this for initialization
	void Start()
	{
        var image = Resources.Load("SpriteStudioFiles/Font/" + ImageFilePath);
        transform.position = SpriteStudioManager.I.MainCamera
            .ViewportToWorldPoint(new Vector3(0, 1, 0));
        
        Observable.EveryUpdate();
	}

	// Update is called once per frame
	void Update()
	{
			
	}
}
