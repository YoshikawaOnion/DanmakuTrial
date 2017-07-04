using UnityEngine;
using UnityEditor;

public class EnemyBehavior5AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior5 Asset Instance")]
	public static void CreateEnemyBehavior5AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior5Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior5Asset.asset");
		AssetDatabase.Refresh();
	}
}
