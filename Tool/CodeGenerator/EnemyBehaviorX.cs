using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEditor;

public class EnemyBehavior$name$ : EnemyBehavior
{
    private EnemyBehavior$name$Asset asset;

    public EnemyBehavior$name$(EnemyApi api) : base(api)
    {
    }

    protected override IObservable<Unit> GetAction()
    {
        throw new NotImplementedException();
    }

    public override IObservable<Unit> LoadAsset()
    {
        const string Name = "EnemyBehavior$name$";
        asset = AssetHelper.LoadBehaviorAsset<EnemyBehavior$name$Asset>(Name);
        return DebugManager.I.LoadAssetFromServer<EnemyBehavior$name$AssetForJson, EnemyBehavior$name$Asset>(asset, Name);
    }
}
