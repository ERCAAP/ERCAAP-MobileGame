using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using ToolbarToolkit;
using UnityEditor;
using UnityEngine;
using System;

namespace LunaGames.Editor
{
    public class ToolbarRight : EditorWindow
    {
        [SerializeField] private VisualTreeAsset RightUI = default;
        [SerializeField] private LunaToolbarSetting Settings = default;

        private bool isToolbarInitiated;

        private float GameSpeed
        {
            get { return Time.timeScale; }
            set
            {
                Time.timeScale = value;
                Time.fixedDeltaTime = 0.02f * value;
            }
        }
        Slider GameSpeedSlider;
        private bool DynamicSpeedToggle;
        float scrollDelta = 0;
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
            if (!enabled) EditorApplication.update -= Update;
            else if (!isToolbarInitiated) EditorApplication.update += Update;

            isToolbarInitiated = enabled;
        }

        private void Update()
        {
            if (DynamicSpeedToggle)
            {
                scrollDelta += Input.GetAxis("Mouse ScrollWheel");
                //Debug.Log("Scroll Delta: " + scrollDelta);

                if (Input.GetMouseButtonDown(2))
                    GameSpeed = 1;
                if (scrollDelta != 0)
                {
                    float newSpeed = Mathf.Clamp(GameSpeed + (scrollDelta > 0 ? 0.25f : -0.25f), 0, 5);
                    GameSpeed = newSpeed;
                    scrollDelta = 0;
                }
            }
            GameSpeedSlider.value = GameSpeed;
        }

        public VisualElement InitGUI()
        {
            VisualElement Result = RightUI.Instantiate();
            Result.StretchToParentSize();

            Label CurrentGameSpeed = Result.Q<Label>(nameof(CurrentGameSpeed));

            GameSpeedSlider = Result.Q<Slider>(nameof(GameSpeedSlider));
            GameSpeedSlider.RegisterValueChangedCallback(e =>
            {
                GameSpeed = e.newValue;
                CurrentGameSpeed.text = GameSpeed.ToString("0.0");
            });
            GameSpeedSlider.highValue = Settings.maxGameSpeed;

            CurrentGameSpeed.text = 1.ToString("0.0");

            Button SpeedReset = Result.Q<Button>(nameof(SpeedReset));
            SpeedReset.clicked += () =>
            {
                GameSpeed = 1;
                GameSpeedSlider.value = 1;
            };

            Toggle ToggleMouse = Result.Q<Toggle>(nameof(ToggleMouse));
            ToggleMouse.RegisterValueChangedCallback(DynamicSpeed);

            void SetScene(DropdownMenuAction action)
            {
                string targetPath = Settings.SceneList.Find((e) => e.DisplayName == action.name).ScenePath;
                EditorSceneManager.OpenScene(targetPath);
                LunaToolbar.CustomLevelLogic();
            }

            ToolbarMenu SceneSelector = Result.Q<ToolbarMenu>(nameof(SceneSelector));
            foreach (SceneData scene in Settings.SceneList)
            {
                SceneSelector.menu.AppendAction(scene.DisplayName, SetScene);
            }


            Button ToolbarSettings = Result.Q<Button>(nameof(ToolbarSettings));
            ToolbarSettings.clicked += GotToSettings;

            Button FrameDebugger = Result.Q<Button>(nameof(FrameDebugger));
            FrameDebugger.clicked += OpenFrameDebugger;

            Button Profiler = Result.Q<Button>(nameof(Profiler));
            Profiler.clicked += OpenProfiler;

            Button PackageManager = Result.Q<Button>(nameof(PackageManager));
            PackageManager.clicked += OpenPackageManager;

            return Result;
        }

        private void DynamicSpeed(ChangeEvent<bool> evt)
        {
            DynamicSpeedToggle = evt.newValue;
        }

        private void GotToSettings()
        {
            Selection.activeObject = Settings;
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        private void OpenPackageManager()
        {
            UnityEditor.PackageManager.UI.Window.Open(string.Empty);
        }
        private void OpenFrameDebugger()
        {
            System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
            Type type = assembly.GetType("UnityEditor.FrameDebuggerWindow");
            var window = EditorWindow.GetWindow(type);
            window.Show();
        }
        private void OpenProfiler()
        {
            System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
            Type type = assembly.GetType("UnityEditor.ProfilerWindow");
            var window = EditorWindow.GetWindow(type);
            window.Show();
        }
    }
}
