﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior13AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior13 Asset Instance")]
	public static void CreateEnemyBehavior13AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior13Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior13Asset.asset");
		AssetDatabase.Refresh();
	}
}
