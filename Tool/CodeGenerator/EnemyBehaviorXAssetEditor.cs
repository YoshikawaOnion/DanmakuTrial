﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyBehavior$name$AssetEditor : ScriptableObject
{
	[MenuItem("Assets/Create/EnemyBehavior/Create EnemyBehavior$name$ Asset Instance")]
	public static void CreateEnemyBehavior$name$AssetInstance()
	{
		var asset = CreateInstance<EnemyBehavior$name$Asset>();
		AssetDatabase.CreateAsset(asset, "Assets/Editor/EnemyBehavior$name$Asset.asset");
		AssetDatabase.Refresh();
	}
}
