using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace LunaGames.Editor
{
    internal static class ReflectionExtensions
    {
        internal static object FetchField(this Type type, string field)
        {
            return type.GetFieldRecursive(field, true).GetValue(null);
        }

        internal static object FetchField(this object obj, string field)
        {
            return obj.GetType().GetFieldRecursive(field, false).GetValue(obj);
        }

        internal static object FetchProperty(this Type type, string property)
        {
            return type.GetPropertyRecursive(property, true).GetValue(null, null);
        }

        internal static object FetchProperty(this object obj, string property)
        {
            return obj.GetType().GetPropertyRecursive(property, false).GetValue(obj, null);
        }

        internal static object CallMethod(this Type type, string method, params object[] parameters)
        {
            return type.GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, parameters);
        }

        internal static object CallMethod(this object obj, string method, params object[] parameters)
        {
            return obj.GetType().GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, parameters);
        }

        internal static object CreateInstance(this Type type, params object[] parameters)
        {
            Type[] parameterTypes;
            if (parameters == null)
                parameterTypes = null;
            else
            {
                parameterTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    parameterTypes[i] = parameters[i].GetType();
            }

            return CreateInstance(type, parameterTypes, parameters);
        }

        internal static object CreateInstance(this Type type, Type[] parameterTypes, object[] parameters)
        {
            return type.GetConstructor(parameterTypes).Invoke(parameters);
        }

        private static FieldInfo GetFieldRecursive(this Type type, string field, bool isStatic)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | (isStatic ? BindingFlags.Static : BindingFlags.Instance);
            do
            {
                FieldInfo fieldInfo = type.GetField(field, flags);
                if (fieldInfo != null)
                    return fieldInfo;

                type = type.BaseType;
            } while (type != null);

            return null;
        }

        private static PropertyInfo GetPropertyRecursive(this Type type, string property, bool isStatic)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | (isStatic ? BindingFlags.Static : BindingFlags.Instance);
            do
            {
                PropertyInfo propertyInfo = type.GetProperty(property, flags);
                if (propertyInfo != null)
                    return propertyInfo;

                type = type.BaseType;
            } while (type != null);

            return null;
        }
    }

    public class MultiScreenshotCapture : OdinEditorWindow
    {
        private static string settingsPath = "Assets/-SETTINGS/Screenshot Setting.asset";
        private static MultiScreenshotCaptureSetting Settings;

        private class CustomResolution
        {
            public readonly int width, height;
            private int originalIndex, newIndex;

            private bool m_isActive;
            public bool IsActive
            {
                get { return m_isActive; }
                set
                {
                    if (m_isActive != value)
                    {
                        m_isActive = value;

                        int resolutionIndex;
                        if (m_isActive)
                        {
                            originalIndex = (int)GameView.FetchProperty("selectedSizeIndex");

                            object customSize = GetFixedResolution(width, height);
                            SizeHolder.CallMethod("AddCustomSize", customSize);
                            newIndex = (int)SizeHolder.CallMethod("IndexOf", customSize) + (int)SizeHolder.CallMethod("GetBuiltinCount");
                            resolutionIndex = newIndex;
                        }
                        else
                        {
                            SizeHolder.CallMethod("RemoveCustomSize", newIndex);
                            resolutionIndex = originalIndex;
                        }

                        GameView.CallMethod("SizeSelectionCallback", resolutionIndex, null);
                        GameView.Repaint();
                    }
                }
            }

            public CustomResolution(int width, int height)
            {
                this.width = width;
                this.height = height;
            }
        }
        private const string TEMPORARY_RESOLUTION_LABEL = "MSC_temp";
        private static object SizeHolder { get { return GetType("GameViewSizes").FetchProperty("instance").FetchProperty("currentGroup"); } }
        private static EditorWindow GameView { get { return GetWindow(GetType("GameView")); } }

        [BoxGroup("Current Resolution"), HideLabel] public ResolutionDatum CurrentResolution;
        [ListDrawerSettings(Expanded = true, DraggableItems = false)] public List<ResolutionDatum> ResolutionData;

        [BoxGroup("Settings")][EnumToggleButtons] public TargetCamera targetCamera = TargetCamera.GameView;
        [BoxGroup("Settings")] public float ResolutionMultiplier = 1f;
        [BoxGroup("Settings")] public bool CaptureOverlayUI = false;
        [BoxGroup("Settings")] public bool SetTimeScaleToZero = true;
        [BoxGroup("Settings")] public bool SaveAsPNG = true;
        [BoxGroup("Settings")][ShowIf("SaveAsPNG")] public bool AllowTransparentBackground = false;
        [BoxGroup("Settings")][FolderPath(AbsolutePath = true)] public string SaveDirectory;
        private float prevTimeScale;


        private readonly List<CustomResolution> queuedScreenshots = new List<CustomResolution>();

        //[MenuItem("Luna Games/Screenshot Capture &s", priority = 1)]
        public static void Init()
        {
            MultiScreenshotCapture window = GetWindow<MultiScreenshotCapture>();
            window.titleContent = new GUIContent("Screenshot");
            window.minSize = new Vector2(325f, 150f);
            window.Show();
        }

        private void GetSettings()
        {
            if (AssetDatabase.LoadAssetAtPath(settingsPath, typeof(MultiScreenshotCaptureSetting)) as MultiScreenshotCaptureSetting == null)
            {
                MultiScreenshotCaptureSetting NewSetting = ScriptableObject.CreateInstance<MultiScreenshotCaptureSetting>();
                AssetDatabase.CreateAsset(NewSetting, settingsPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
                Settings = AssetDatabase.LoadAssetAtPath(settingsPath, typeof(MultiScreenshotCaptureSetting)) as MultiScreenshotCaptureSetting;
        }

        private void Awake()
        {
            GetSettings();
            CurrentResolution = Settings.CurrentResolution;
            ResolutionData = Settings.ResolutionData;
            ResolutionMultiplier = Settings.resolutionMultiplier > 0f ? Settings.resolutionMultiplier : 1f;
            targetCamera = Settings.targetCamera;
            CaptureOverlayUI = Settings.captureOverlayUI;
            SetTimeScaleToZero = Settings.setTimeScaleToZero;
            SaveAsPNG = Settings.saveAsPNG;
            AllowTransparentBackground = Settings.allowTransparentBackground;
            SaveDirectory = Settings.saveDirectory;
        }

        protected override void OnDestroy()
        {
            Settings.CurrentResolution = CurrentResolution;
            Settings.ResolutionData = ResolutionData;
            Settings.resolutionMultiplier = ResolutionMultiplier;
            Settings.targetCamera = targetCamera;
            Settings.captureOverlayUI = CaptureOverlayUI;
            Settings.setTimeScaleToZero = SetTimeScaleToZero;
            Settings.saveAsPNG = SaveAsPNG;
            Settings.allowTransparentBackground = AllowTransparentBackground;
            Settings.saveDirectory = SaveDirectory;
            base.OnDestroy();
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            CurrentResolution.resolution = (targetCamera == TargetCamera.GameView ? Camera.main : SceneView.lastActiveSceneView.camera).pixelRect.size;

        }
        [Button(ButtonSizes.Gigantic), GUIColor(0,1,0)]
        public void CaptureScreenShots()
        {
            if (string.IsNullOrEmpty(SaveDirectory))
                SaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            if (CurrentResolution.resolutionsEnabled)
                CaptureScreenshot(CurrentResolution.resolution);

            for (int i = 0; i < ResolutionData.Count; i++)
            {
                if (ResolutionData[i].resolutionsEnabled)
                    CaptureScreenshot(ResolutionData[i].resolution);
            }

            if (!CaptureOverlayUI || targetCamera == TargetCamera.SceneView)
                Debug.Log("<b>Saved screenshots:</b> " + SaveDirectory);
            else
            {
                if (EditorApplication.isPlaying && SetTimeScaleToZero)
                {
                    prevTimeScale = Time.timeScale;
                    Time.timeScale = 0f;
                }

                EditorApplication.update -= CaptureQueuedScreenshots;
                EditorApplication.update += CaptureQueuedScreenshots;
            }
        }

        private void CaptureScreenshot(Vector2 resolution)
        {
            int width = Mathf.RoundToInt(resolution.x * ResolutionMultiplier);
            int height = Mathf.RoundToInt(resolution.y * ResolutionMultiplier);

            if (width <= 0 || height <= 0)
                Debug.LogWarning("Skipped resolution: " + resolution);
            else if (!CaptureOverlayUI || targetCamera == TargetCamera.SceneView)
                CaptureScreenshotWithoutUI(width, height);
            else
                queuedScreenshots.Add(new CustomResolution(width, height));
        }

        private void CaptureQueuedScreenshots()
        {
            if (queuedScreenshots.Count == 0)
            {
                EditorApplication.update -= CaptureQueuedScreenshots;
                return;
            }

            CustomResolution resolution = queuedScreenshots[0];
            if (!resolution.IsActive)
            {
                resolution.IsActive = true;

                if (EditorApplication.isPlaying && EditorApplication.isPaused)
                    EditorApplication.Step(); // Necessary to refresh overlay UI
            }
            else
            {
                try
                {
                    CaptureScreenshotWithUI();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                resolution.IsActive = false;

                queuedScreenshots.RemoveAt(0);
                if (queuedScreenshots.Count == 0)
                {
                    if (EditorApplication.isPlaying && EditorApplication.isPaused)
                        EditorApplication.Step(); // Necessary to restore overlay UI

                    if (EditorApplication.isPlaying && SetTimeScaleToZero)
                        Time.timeScale = prevTimeScale;

                    Debug.Log("<b>Saved screenshots:</b> " + SaveDirectory);
                    Repaint();
                }
                else
                {
                    // Activate the next resolution immediately
                    CaptureQueuedScreenshots();
                }
            }
        }

        private void CaptureScreenshotWithoutUI(int width, int height)
        {
            Camera camera = targetCamera == TargetCamera.GameView ? Camera.main : SceneView.lastActiveSceneView.camera;

            RenderTexture temp = RenderTexture.active;
            RenderTexture temp2 = camera.targetTexture;

            RenderTexture renderTex = RenderTexture.GetTemporary(width, height, 24);
            Texture2D screenshot = null;

            bool allowHDR = camera.allowHDR;
            if (SaveAsPNG && AllowTransparentBackground)
                camera.allowHDR = false;

            try
            {
                RenderTexture.active = renderTex;

                camera.targetTexture = renderTex;
                camera.Render();

                screenshot = new Texture2D(renderTex.width, renderTex.height, SaveAsPNG && AllowTransparentBackground ? TextureFormat.RGBA32 : TextureFormat.RGB24, false);
                screenshot.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0, false);
                screenshot.Apply(false, false);

                File.WriteAllBytes(GetUniqueFilePath(renderTex.width, renderTex.height), SaveAsPNG ? screenshot.EncodeToPNG() : screenshot.EncodeToJPG(100));
            }
            finally
            {
                camera.targetTexture = temp2;
                if (SaveAsPNG && AllowTransparentBackground)
                    camera.allowHDR = allowHDR;

                RenderTexture.active = temp;
                RenderTexture.ReleaseTemporary(renderTex);

                if (screenshot != null)
                    DestroyImmediate(screenshot);
            }
        }

        private void CaptureScreenshotWithUI()
        {
            RenderTexture temp = RenderTexture.active;

            RenderTexture renderTex = (RenderTexture)GameView.FetchField("m_TargetTexture");
            Texture2D screenshot = null;

            int width = renderTex.width;
            int height = renderTex.height;

            try
            {
                RenderTexture.active = renderTex;

                screenshot = new Texture2D(width, height, SaveAsPNG && AllowTransparentBackground ? TextureFormat.RGBA32 : TextureFormat.RGB24, false);
                screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);

                if (SystemInfo.graphicsUVStartsAtTop)
                {
                    Color32[] pixels = screenshot.GetPixels32();
                    for (int i = 0; i < height / 2; i++)
                    {
                        int startIndex0 = i * width;
                        int startIndex1 = (height - i - 1) * width;
                        for (int x = 0; x < width; x++)
                        {
                            Color32 color = pixels[startIndex0 + x];
                            pixels[startIndex0 + x] = pixels[startIndex1 + x];
                            pixels[startIndex1 + x] = color;
                        }
                    }

                    screenshot.SetPixels32(pixels);
                }

                screenshot.Apply(false, false);

                File.WriteAllBytes(GetUniqueFilePath(width, height), SaveAsPNG ? screenshot.EncodeToPNG() : screenshot.EncodeToJPG(100));
            }
            finally
            {
                RenderTexture.active = temp;

                if (screenshot != null)
                    DestroyImmediate(screenshot);
            }
        }


        private string GetUniqueFilePath(int width, int height)
        {
            string filename = string.Concat(width, "x", height, " {0}", SaveAsPNG ? ".png" : ".jpeg");
            int fileIndex = 0;
            string path;
            do
            {
                path = Path.Combine(SaveDirectory, string.Format(filename, ++fileIndex));
            } while (File.Exists(path));

            return path;
        }

        private static object GetFixedResolution(int width, int height)
        {
            object sizeType = Enum.Parse(GetType("GameViewSizeType"), "FixedResolution");
            return GetType("GameViewSize").CreateInstance(sizeType, width, height, TEMPORARY_RESOLUTION_LABEL);
        }

        private static Type GetType(string type)
        {
            return typeof(EditorWindow).Assembly.GetType("UnityEditor." + type);
        }
    }
}
#endif