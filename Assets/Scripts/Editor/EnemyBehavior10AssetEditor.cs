﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior10AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior10 Asset Instance")]
	public static void CreateEnemyBehavior10AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior10Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior10Asset.asset");
		AssetDatabase.Refresh();
	}
}
