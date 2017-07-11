using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyBehavior1AssetForJson
{
	[SerializeField]
	public float ShotTimeSpan;
	[SerializeField]
	public int ColumnsInChunk;
	[SerializeField]
	public int RowsInChunk;
	[SerializeField]
	[Tooltip("速度[px/sec]")]
	public float Speed;
	[SerializeField]
	public float OffsetSize;
	[SerializeField]
	public float RandomOffsetSize;
}
