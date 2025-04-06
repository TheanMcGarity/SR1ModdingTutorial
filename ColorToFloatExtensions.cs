using UnityEngine;

namespace TestModForVideoSR1
{
    public static class ColorToFloatExtensions
    {
        public static Color GetFloatVersion(this Color32 color) =>
            new Color(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
    }

}