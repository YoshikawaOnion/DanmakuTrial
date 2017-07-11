using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Text;

public static class AssetHelper
{
    public static T LoadBehaviorAsset<T>(string behaviorName)
        where T : UnityEngine.Object
    {
        var builder = new StringBuilder();
        builder.Append("ScriptableAsset/");
        builder.Append(behaviorName);
        builder.Append("Asset");
        return Resources.Load<T>(builder.ToString());
    }

    public static void Write<TJson, TAsset>(TJson src, TAsset dst)
    {
        var fields = src.GetType().GetFields();
        var dstType = dst.GetType();
        foreach (var field in fields)
        {
            var dstfield = dstType.GetField(field.Name);
            var value = field.GetValue(src);
            dstfield.SetValue(dst, value);
        }
    }
}
