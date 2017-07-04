using UnityEngine;
using System.Collections;

public class EnemyBehavior1Asset : ScriptableObject
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
