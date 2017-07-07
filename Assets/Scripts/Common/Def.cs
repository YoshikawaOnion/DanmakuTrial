using UnityEngine;
using System.Collections;

public static class Def
{
    public static readonly float ScreenWidth = 360;
    public static readonly float ScreenHeight = 640;
	public static readonly float UnitPerPixel = 80.0f * 2 / ScreenHeight;
    public static readonly float PixelPerFrame = UnitPerPixel * 60;

    public static readonly string EnemyTag = "Enemy";
    public static readonly string EnemyShotTag = "EnemyShot";
    public static readonly string PlayerShotTag = "PlayerShot";
    public static readonly string MobTag = "Mob";
}
