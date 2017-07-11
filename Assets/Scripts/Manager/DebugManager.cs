using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using UniRx;
using System.Text;

public class DebugManager : Singleton<DebugManager>
{
    protected override void Init()
    {
    }

    public IObservable<Unit> LoadAssetFromServer<TJson, TAsset>(TAsset asset, string behaviorName)
    {
        var url = new StringBuilder();
        url.Append("http://oniongames.jp/test/api/danmaku_trial/");
        url.Append(behaviorName);
        url.Append(".json");
        return DownloadText(url.ToString())
            .ForEachAsync(text =>
        {
            if (text != null)
			{
                Debug.Log("Download Succeeded");
				var jsonAsset = JsonUtility.FromJson<TJson>(text);
				AssetHelper.Write(jsonAsset, asset);
            }
        });
    }

    private IObservable<string> DownloadText(string url)
    {
        Debug.Log("[DebugManager]Connecting Server... : " + url);
        var request = new UnityWebRequest();
        request.downloadHandler = new DownloadHandlerBuffer();
        request.url = url;
        request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
        request.method = UnityWebRequest.kHttpVerbGET;

        // リクエストに対して応答が来たら処理する
        return request.Send().AsObservable()
               .Select(op =>
        {
			if (request.isError)
			{
				Debug.LogWarning(request.error);
                return null;
			}
			if (request.responseCode == 200)
			{
				// UTF8文字列として取得する
				string text = request.downloadHandler.text;
				Debug.Log("[DebugManager]Load Settings Success.");
                return text;
			}
			// なにかしら変
			Debug.LogWarning("request.responseCode : " + request.responseCode.ToString());
            return null;
        });
    }
}
