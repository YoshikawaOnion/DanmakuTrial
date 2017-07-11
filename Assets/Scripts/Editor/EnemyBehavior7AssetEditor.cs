﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior7AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior7 Asset Instance")]
	public static void CreateEnemyBehavior7AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior7Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior7Asset.asset");
		AssetDatabase.Refresh();
	}
}
