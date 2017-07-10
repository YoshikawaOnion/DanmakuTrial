using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
}
