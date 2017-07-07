﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior12AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior12 Asset Instance")]
	public static void CreateEnemyBehavior12AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior12Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior12Asset.asset");
		AssetDatabase.Refresh();
	}
}
