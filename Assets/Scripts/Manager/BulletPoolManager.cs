using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class BulletPoolManager : MonoBehaviour
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

    public GameObject GetInstance(Kind kind)
    {
        List<GameObject> list;
        bool result = objectPool.TryGetValue(kind, out list);
        if (!result)
        {
            list = new List<GameObject>();
            objectPool.Add(kind, list);
        }

        foreach (var item in list)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                return item;
            }
        }

        var obj = GetNewInstance(kind);
        list.Add(obj);
        return obj;
    }

    public void SleepInstance(GameObject obj)
	{
		obj.SetActive(false);
		obj.transform.position = Vector2.zero;
        obj.GetComponent<Collider2D>().enabled = false;
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

    public int GetActiveCount(Kind kind)
    {
        return objectPool.Where(x => x.Key == kind)
                         .SelectMany(x => x.Value)
                         .Where(x => x.activeInHierarchy)
                         .Count();
    }
}
