using UnityEngine;
using System.Collections;

public static class Def
{
    public static float ScreenWidth = 360;
    public static float ScreenHeight = 640;
	public static float UnitPerPixel = 80.0f * 2 / ScreenHeight;
    public static float PixelPerFrame = UnitPerPixel * 60;
}
