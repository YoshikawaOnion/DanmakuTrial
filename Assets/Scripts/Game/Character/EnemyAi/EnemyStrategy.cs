using UnityEngine;
using System.Collections;

public abstract class EnemyStrategy
{
    public bool isDefeated;
    protected Enemy Owner;

    public EnemyStrategy(Enemy owner)
    {
        Owner = owner;
        isDefeated = false;
    }

    public virtual void Initialize()
    {
    }

    public virtual void OnDestroy()
    {
    }

    public abstract IEnumerator Act();
}
