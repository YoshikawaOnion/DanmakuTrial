﻿using System;
using UnityEngine;

public static class Vector3Extensions {

	public static Vector3 CreateFromArray (float[] array) {
		if (array.Length == 3) {
			return new Vector3 (array [0], array [1], array [2]);
		} else {
			throw new InvalidOperationException ();
		}
	}

	public static float[] ToArray (this Vector3 vector) {
		return new[] { vector.x, vector.y, vector.z };
	}

	public static Vector3 XReplacedBy (this Vector3 vector, float x) {
		return new Vector3 (x, vector.y, vector.z);
	}

	public static Vector3 YReplacedBy (this Vector3 vector, float y) {
		return new Vector3 (vector.x, y, vector.z);
	}

	public static Vector3 ZReplacedBy (this Vector3 vector, float z) {
		return new Vector3 (vector.x, vector.y, z);
	}

	static public Vector3 MergeX (this Vector3 v, float x) {
		return new Vector3 (x, v.y, v.z);
	}

	static public Vector3 MergeY (this Vector3 v, float y) {
		return new Vector3 (v.x, y, v.z);
	}

	static public Vector3 MergeZ (this Vector3 v, float z) {
		return new Vector3 (v.x, v.y, z);
	}

	static public Vector3 MergeX (this Vector3 v, Vector3 v2) {
		return new Vector3 (v2.x, v.y, v.z);
	}

	static public Vector3 MergeY (this Vector3 v, Vector3 v2) {
		return new Vector3 (v.x, v2.y, v.z);
	}

	static public Vector3 MergeZ (this Vector3 v, Vector3 v2) {
		return new Vector3 (v.x, v.y, v2.z);
	}

	static public Vector3 AddX (this Vector3 v, float x) {
		return new Vector3 (v.x + x, v.y, v.z);
	}

	static public Vector3 AddY (this Vector3 v, float y) {
		return new Vector3 (v.x, v.y + y, v.z);
	}

	static public Vector3 AddZ (this Vector3 v, float z) {
		return new Vector3 (v.x, v.y, v.z + z);
	}

	static public Vector3 AddX (this Vector3 v, Vector3 v2) {
		return new Vector3 (v.x + v2.x, v.y, v.z);
	}

	static public Vector3 AddY (this Vector3 v, Vector3 v2) {
		return new Vector3 (v.x, v.y + v2.y, v.z);
	}

	static public Vector3 AddZ (this Vector3 v, Vector3 v2) {
		return new Vector3 (v.x, v.y, v.z + v2.z);
	}

	static public Vector3 Add (this Vector3 v, Vector3 v2) {
		return new Vector3 (v.x + v2.x, v.y + v2.y, v.z + v2.z);
	}

	public static Vector2 ToVector2 (this Vector3 v) {
		return new Vector2 (v.x, v.y);
	}

    public static Vector3 Mul(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public static Vector3 Div(this Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
    }

	public static bool IsNaN (this Vector3 v) {
		if (float.IsNaN (v.x) || float.IsNaN (v.y) || float.IsNaN (v.z)) {
			return true;
		}
		return false;
	}
}

public static class Vector2Extensions {
	
	public static Vector2 InvertX (this Vector2 v) {
		return new Vector2 (-v.x, v.y);
	}

	public static Vector2 InvertY (this Vector2 v) {
		return new Vector2 (v.x, -v.y);
	}

	public static Vector2 MergeX (this Vector2 v, float x) {
		return new Vector2 (x, v.y);
	}

	public static Vector2 MergeY (this Vector2 v, float y) {
		return new Vector2 (v.x, y);
	}

	public static Vector3 ToVector3 (this Vector2 v, float z = 0) {
		return new Vector3 (v.x, v.y, z);
	}

	public static bool IsNaN (this Vector2 v) {
		if (float.IsNaN (v.x) || float.IsNaN (v.y)) {
			return true;
		}
		return false;
	}
}