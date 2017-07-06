﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior8AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior8 Asset Instance")]
	public static void CreateEnemyBehavior8AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior8Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior8Asset.asset");
		AssetDatabase.Refresh();
	}
}
