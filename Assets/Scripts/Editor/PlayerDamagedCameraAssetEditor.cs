using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerDamagedCameraAssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/PlayerState/Create PlayerDamagedCamera Asset Instance")]
    public static void CreatePlayerDamagedCameraAssetInstance()
	{
		var asset = CreateInstance<PlayerDamagedCameraAsset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/PlayerDamagedCameraAsset.asset");
		AssetDatabase.Refresh();
	}
}
