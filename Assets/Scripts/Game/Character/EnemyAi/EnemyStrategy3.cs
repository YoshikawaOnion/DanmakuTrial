using UnityEngine;
using System.Collections;
using System;

public class EnemyStrategy3 : EnemyStrategy
{
    public EnemyStrategy3(Enemy owner) : base(owner)
    {
    }

    public override IEnumerator Act()
    {
        int holeAngle = 72 / 2;
        for (int i = 0; i < 72; i++)
        {
            if (Mathf.Abs(i - holeAngle) > 2)
            {
                
            }
        }
    }
}
