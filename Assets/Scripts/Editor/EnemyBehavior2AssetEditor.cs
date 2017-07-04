using UnityEngine;
using System.Collections;
using UnityEditor;

public class EnemyBehavior2AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior2 Asset Instance")]
	public static void CreateEnemyBehavior2AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior2Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior2Asset.asset");
		AssetDatabase.Refresh();
	}
}
