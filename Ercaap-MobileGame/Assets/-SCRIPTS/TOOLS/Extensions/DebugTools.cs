using UnityEngine;

namespace LunaGames.Extentions
{
    public static class DebugTools
    {
        public static T Debug<T>(this T message, Object? context = null) {
            if(UnityEngine.Debug.isDebugBuild)
                UnityEngine.Debug.Log(message, context);
            return (T)message;
        }
        public static T DebugWarning<T>(this T message, Object? context = null) {
            if(UnityEngine.Debug.isDebugBuild)
                UnityEngine.Debug.LogWarning(message, context);
            return (T)message;
        }
        public static T DebugError<T>(this T message, Object? context = null) {
            if(UnityEngine.Debug.isDebugBuild)
                UnityEngine.Debug.LogError(message, context);
            return (T)message;
        }
    }
}
