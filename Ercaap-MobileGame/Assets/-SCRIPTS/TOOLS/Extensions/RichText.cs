using UnityEngine;

namespace LunaGames.Extentions
{
    public static class RichText
    {
        public static string Size(this string ScaleString, float muliplier) {
            return $"<size={100 * muliplier}%>{ScaleString}</size>";
        }
        public static string Color(this string ColorString, Color color)
        {
            return $"<color={color.ToHex()}>{ColorString}</color>";
        }

        public static string ToHex(this Color c)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(c)}";
        }

        public static string Color(this string ColorString, string colorCode)
        {
            return $"<color={colorCode}>{ColorString}</color>";
        }
        public static string Material(this string MaterialString, string attribute)
        {
            string fontName = TMPro.TMP_Settings.defaultFontAsset.name;
            return $"<font=\"{fontName}\" material=\"{fontName} {attribute}\">{MaterialString}</font>";
        }
        public static string Emote(string emojiName)
        {
            return $"<sprite name={emojiName}>";
        }
        public static string Rotate(this string RotateString, float angle)
        {
            return $"<rotate=\"{angle}\">{RotateString}</rotate>";
        }
    }
}
