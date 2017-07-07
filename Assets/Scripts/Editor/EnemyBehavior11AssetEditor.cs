﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior11AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior11 Asset Instance")]
	public static void CreateEnemyBehavior11AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior11Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior11Asset.asset");
		AssetDatabase.Refresh();
	}
}
