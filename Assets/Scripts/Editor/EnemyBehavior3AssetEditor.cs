using UnityEngine;
using System.Collections;
using UnityEditor;

public class EnemyBehavior3AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior3 Asset Instance")]
	public static void CreateEnemyBehavior3AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior3Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior3Asset.asset");
		AssetDatabase.Refresh();
	}
}
