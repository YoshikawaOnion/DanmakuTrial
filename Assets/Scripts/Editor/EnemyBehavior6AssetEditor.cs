using UnityEngine;
using UnityEditor;

public class EnemyBehavior6AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior6 Asset Instance")]
	public static void CreateEnemyBehavior6AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior6Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior6Asset.asset");
		AssetDatabase.Refresh();
	}
}
