using UnityEngine;
using UnityEditor;

public class EnemyBehavior4AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior4 Asset Instance")]
	public static void CreateEnemyBehavior4AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior4Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior4Asset.asset");
		AssetDatabase.Refresh();
	}
}
