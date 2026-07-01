using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LunaGames.Editor
{
    public class ScriptsManager : EditorWindow
    {
        public List<ComponentData> ComponentList = new List<ComponentData>();
        public class ComponentData
        {
            public ComponentData(Component component)
            {
                this.component = component;
                GetGameObjects();
            }
            public string ComponentName => component.GetType().Name;
            private Component component;
            public GameObject[] GetGameObjects()
            {
                List<GameObject> gos = new List<GameObject>();
                Object[] Os = FindObjectsOfType(component.GetType());
                foreach (Object o in Os)
                {
                    gos.Add(o.FetchProperty("gameObject") as GameObject);
                }
                return gos.ToArray();
            }
            public void SelectAll()
            {
                Selection.objects = GetGameObjects();
            }
            public void EditScript()
            {
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(GetFile(ComponentName), 0);
            }
        }
        public static void Init()
        {
            EditorWindow window = GetWindow(typeof(ScriptsManager));
            window.minSize = new Vector2(300, 250);
            window.maxSize = new Vector2(300, 350);
            window.titleContent = new GUIContent("Scripts Manager");
        }

        public void UpdateList()
        {
            ComponentList.Clear();
            List<string> ComponentsNames = new List<string>();
            foreach (Component component in FindObjectsOfType(typeof(Component)))
            {
                bool isMonoBehaviour = component.GetType().BaseType == typeof(UnityEngine.MonoBehaviour);

                if (isMonoBehaviour)
                {
                    bool isDuplicate = ComponentsNames.Contains(component.GetType().Name);
                    if (!isDuplicate)
                    {
                        bool isFileAvailable = GetFile(component.GetType().Name) != "";
                        if (isFileAvailable)
                        {
                            ComponentsNames.Add(component.GetType().Name);
                            ComponentList.Add(new ComponentData(component));
                        }
                    }
                }
            }
        }
        static string GetFile(string fileName)
        {
            foreach (var guid in AssetDatabase.FindAssets("t:Script"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.Contains(fileName)) return path;
            }
            return "";
        }
        protected void OnGUI() => FallBackGUI();

        private Vector2 Scroll = new Vector2(0, 0);
        private static Texture2D EditIcon => EditorGUIUtility.Load("Assets/-TEXTURES/Editor/preview.png") as Texture2D;
        private string search = "";

        public List<ComponentData> FilteredComponentList
        {
            get => ComponentList.Where((asset) => asset.ComponentName.ToLower().Contains(search.ToLower())).OrderBy(o => o.ComponentName).ToList();
            set => ComponentList = value;
        }
        private void FallBackGUI()
        {
            search = EditorGUILayout.TextField(search, EditorStyles.toolbarSearchField);

            Scroll = GUILayout.BeginScrollView(Scroll);
            foreach (ComponentData COM in FilteredComponentList)
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                if (GUILayout.Button(new GUIContent(COM.ComponentName), EditorStyles.toolbarButton, GUILayout.MaxWidth(250))) COM.SelectAll();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(new GUIContent(EditIcon, "Edit Script"), EditorStyles.toolbarButton, GUILayout.Width(50))) COM.EditScript();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private void OnFocus() => UpdateList();
    }
}