using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using System;

namespace LunaGames.Editor
{
    public class ToolbarLeft : EditorWindow
    {
        [SerializeField] private VisualTreeAsset LeftUI = default;
        private ToolbarToggle CustomLevelToggle = default;
        private PropertyField CustomLevelNo = default;
        private bool isToolbarInitiated;
        private Label Stats;

        public VisualElement Init()
        {
            Initialize(true);
            return InitGUI();
        }
        private void OnDestroy()
        {
            Initialize(false);
        }
        private void Initialize(bool enabled)
        {
            if (!enabled)
            {
                EditorApplication.update -= Update;
            }
            else if (!isToolbarInitiated)
            {
                EditorApplication.update += Update;
            }

            isToolbarInitiated = enabled;
        }

        private void Update()
        {
            GetStats(Stats);
            CustomLevelNo.SetEnabled(CustomLevelToggle.value);
        }

        public VisualElement InitGUI()
        {
            VisualElement Root = LeftUI.Instantiate();
            Root.StretchToParentSize();

            Button AssetManager = Root.Q<Button>(nameof(AssetManager));
            AssetManager.clicked += () => LunaGames.Editor.AssetManager.OpenWindow();

            Button PrefabPainter = Root.Q<Button>(nameof(PrefabPainter));
            PrefabPainter.clicked += () => LunaGames.Tools.LevelEditor.PrefabPainter.OpenWindow();

            Button PlayerPrefs = Root.Q<Button>(nameof(PlayerPrefs));
            PlayerPrefs.clicked += () => LunaGames.Editor.PlayerPrefsEditor.Init();

            Button ScreenShot = Root.Q<Button>(nameof(ScreenShot));
            ScreenShot.clicked += () => LunaGames.Editor.MultiScreenshotCapture.Init();

            Button Replacer = Root.Q<Button>(nameof(Replacer));
            Replacer.clicked += () => LunaGames.Editor.Replacer.OpenWindow();

            Button ScriptManager = Root.Q<Button>(nameof(ScriptManager));
            ScriptManager.clicked += () => LunaGames.Editor.ScriptMonitor.Init();

            Button SMBuilder = Root.Q<Button>(nameof(SMBuilder));
            SMBuilder.clicked += () => LunaGames.Editor.StateMachineBuilder.OpenWindow();

            CustomLevelNo = Root.Q<PropertyField>(nameof(CustomLevelNo));

            CustomLevelToggle = Root.Q<ToolbarToggle>(nameof(CustomLevelToggle));

            Stats = Root.Q<Label>(nameof(Stats));

            return Root;

        }



        float deltaTime = 0.0f;
        private void GetStats(Label L)
        {
            bool canShow = EditorApplication.isPlaying;
            L.parent.style.visibility = canShow ? Visibility.Visible : Visibility.Hidden;
            if (!canShow || EditorApplication.isPaused) return;
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            float lerp = (fps - 30) / 30;
            Color C = Color.Lerp(Color.red, Color.green, lerp);
            L.text = $"{msec.ToString("0")}ms - <color=#{ColorUtility.ToHtmlStringRGB(C)}>{fps.ToString("0.0")} FPS</color> - B: {UnityStats.drawCalls}";
        }
    }
}
