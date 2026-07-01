using UnityEditor.UIElements;
using UnityEngine.UIElements;
using LunaGames.Editor;
using UnityEngine;
using UnityEditor;

namespace ToolbarToolkit
{
    [InitializeOnLoad]
    public static class LunaToolbar
    {
        private static VisualElement LeftUI;
        private static VisualElement RightUI;
        private static bool isToolbarInitiated;

        static LunaToolbar()
        {
            Initialize(true);
        }
        private static void Initialize(bool enabled)
        {
            if (!enabled)
            {
                EditorApplication.update -= Init;
            }
            else if (!isToolbarInitiated)
            {
                EditorApplication.update += Init;
            }
            isToolbarInitiated = enabled;
        }

        static void Init()
        {
            RightUI = ScriptableObject.CreateInstance<ToolbarRight>().Init();
            if (RightUI == null) return;

            LeftUI = ScriptableObject.CreateInstance<ToolbarLeft>().Init();
            if (LeftUI == null) return;

            Toolbar.RightUI = RightUI;
            Toolbar.LeftUI = LeftUI;
            CustomLevelLogic();
            Initialize(false);
        }

        public static void CustomLevelLogic()
        {
            VisualElement CustomLevelArea = LeftUI.Q<VisualElement>(nameof(CustomLevelArea));

            ToolbarToggle CustomLevelToggle = LeftUI.Q<ToolbarToggle>(nameof(CustomLevelToggle));

            PropertyField CustomLevelNo = LeftUI.Q<PropertyField>(nameof(CustomLevelNo));
            GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            LunaGames.Main.LevelManager LM = null;
            foreach (GameObject root in rootObjects)
            {
                if(root.GetComponentInChildren<LunaGames.Main.LevelManager>() != null){
                    LM = root.GetComponentInChildren<LunaGames.Main.LevelManager>();
                    break;
                }
            }
            if (LM != null)
            {
                CustomLevelArea.style.display = DisplayStyle.Flex;
                SerializedObject LMO = new SerializedObject(LM);
                CustomLevelNo.BindProperty(LMO.FindProperty("customLevelNo"));
                CustomLevelNo.label = "";
                CustomLevelToggle.BindProperty(LMO.FindProperty("customLevel"));
            }
            else
            {
                CustomLevelArea.style.display = DisplayStyle.None;
                CustomLevelNo.Unbind();
                CustomLevelToggle.Unbind();
            }
        }
    }
}