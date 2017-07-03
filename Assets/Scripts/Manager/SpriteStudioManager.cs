using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SpriteStudioの描画に関する情報を扱うクラス。
/// </summary>
public class SpriteStudioManager : Singleton<SpriteStudioManager>
{
    public Script_SpriteStudio_ManagerDraw ManagerDraw;
    //TODO: カメラはSpriteStudioに関係なく使うことがあるので移動したい
    public Camera MainCamera;

    protected override void Init()
    {
    }
}
