using UnityEngine;
using System.Collections;
using UnityEditor;

public class EnemyBehavior2Asset : ScriptableObject
{
    public int Way = 8;
    public int AimShotColumns = 1;
    public int AimShotRows = 2;
    public float FlowerSlowestSpeed = 200;
    public float ShotTimeSpan = 1.2f;
    public float ShotSpan = 33;
    public float FlowerShotRows = 8;
}
