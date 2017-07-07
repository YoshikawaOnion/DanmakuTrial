using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerDamageCameraAssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/PlayerState/Create PlayerDamageCamera Asset Instance")]
	public static void CreatePlayerDamageCameraAssetInstance()
	{
		var asset = CreateInstance<PlayerDamagedCameraAsset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/PlayerDamageCameraAsset.asset");
		AssetDatabase.Refresh();
	}
}
