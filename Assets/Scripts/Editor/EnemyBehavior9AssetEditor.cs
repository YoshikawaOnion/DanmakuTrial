﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior9AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior9 Asset Instance")]
	public static void CreateEnemyBehavior9AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior9Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior9Asset.asset");
		AssetDatabase.Refresh();
	}
}
