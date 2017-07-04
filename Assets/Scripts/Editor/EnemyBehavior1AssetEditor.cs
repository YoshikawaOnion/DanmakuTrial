﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior1AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior1 Asset Instance")]
	public static void CreateEnemyBehavior1AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior1Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior1Asset.asset");
		AssetDatabase.Refresh();
	}
}
