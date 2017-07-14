using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum EnemyShotKind
{
	Null, Flower, Circle, Lockon, Shooting,
    Ring
}

public class ObjectPoolManager : MonoBehaviour
{
    public enum Kind
    {
        Normal, Mob
    }

    [SerializeField]
	private GameObject normalShotPrefab;
	[SerializeField]
    private GameObject mobPrefab;

    private Dictionary<Kind, List<GameObject>> objectPool = new Dictionary<Kind, List<GameObject>>();
    private Dictionary<EnemyShotKind, List<EnemyShotBehavior>> shotBehaviorPool = new Dictionary<EnemyShotKind, List<EnemyShotBehavior>>();

    private void Awake()
    {
        var bulletList = new List<GameObject>();
        for (int i = 0; i < 24; i++)
        {
            var obj = Instantiate(normalShotPrefab);
            obj.SetActive(false);
            bulletList.Add(obj);
        }

        var mobList = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            var mob = Instantiate(mobPrefab);
            mob.SetActive(false);
            mobList.Add(mob);
        }

        objectPool.Add(Kind.Normal, bulletList);
        objectPool.Add(Kind.Mob, mobList);

        // TODO: EnemyShotBehavior系もたくさん生成するのでプールした方がいいかも
        var nullList = new List<EnemyShotBehavior>();
        for (int i = 0; i < 24; i++)
        {
            var obj = new NullEnemyShotBehavior();
            obj.IsActive = false;
            nullList.Add(obj);
        }
        shotBehaviorPool.Add(EnemyShotKind.Null, nullList);
    }

    public GameObject GetInstance(Kind kind)
    {
        List<GameObject> list;
        bool result = objectPool.TryGetValue(kind, out list);
        if (!result)
        {
            list = new List<GameObject>();
            objectPool.Add(kind, list);
        }

		//*
		foreach (var item in list)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                return item;
            }
        }
        //*/

        var obj = GetNewInstance(kind);
        list.Add(obj);
        return obj;
    }

    public void SleepInstance(GameObject obj)
	{
		obj.GetComponent<Collider2D>().enabled = false;
		obj.SetActive(false);
		obj.transform.position = Vector2.zero;
	}

	private GameObject GetNewInstance(Kind kind)
	{
		switch (kind)
		{
			case Kind.Normal:
				return Instantiate(normalShotPrefab);
			case Kind.Mob:
				return Instantiate(mobPrefab);
			default:
				throw new Exception("弾の生成処理で不明なKindが指定されました。");
		}
	}

	public TBehavior GetInstance<TBehavior>(EnemyShotKind kind)
        where TBehavior : EnemyShotBehavior
	{
		List<EnemyShotBehavior> list;
		bool result = shotBehaviorPool.TryGetValue(kind, out list);
		if (!result)
		{
			list = new List<EnemyShotBehavior>();
			shotBehaviorPool.Add(kind, list);
		}

		foreach (var item in list)
		{
			if (!item.IsActive)
			{
                item.IsActive = true;
				return item as TBehavior;
			}
		}

		var obj = GetNewInstance(kind);
        obj.IsActive = true;
		list.Add(obj);
		return obj as TBehavior;
	}

	public void SleepInstance(EnemyShotBehavior obj)
	{
        obj.IsActive = false;
	}

    private EnemyShotBehavior GetNewInstance(EnemyShotKind kind)
    {
        switch (kind)
        {
            case EnemyShotKind.Null: return new NullEnemyShotBehavior();
            case EnemyShotKind.Flower: return new FlowerEnemyShotBehavior();
            case EnemyShotKind.Circle: return new CircleEnemyShotBehavior();
            case EnemyShotKind.Lockon: return new LockOnEnemyShotBehavior();
            case EnemyShotKind.Shooting: return new ShootingEnemyShotBehavior();
            case EnemyShotKind.Ring: return new RingEnemyShotBehavior();
            default: throw new Exception("EnemyShotBehaviorの生成処理で不明なKindが指定されました。");
        }
    }

    public int GetActiveCount(Kind kind)
    {
        return objectPool.Where(x => x.Key == kind)
                         .SelectMany(x => x.Value)
                         .Where(x => x.activeInHierarchy)
                         .Count();
    }
}
