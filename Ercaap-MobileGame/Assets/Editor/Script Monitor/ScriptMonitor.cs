using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace LunaGames.Editor
{
    public class ScriptMonitor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset UI = default;
        [SerializeField] private VisualTreeAsset ComponentUI = default;

        public List<ComponentData> ComponentList = new List<ComponentData>();
        public List<ComponentData> FilteredComponentList = new List<ComponentData>();

        private ListView ScriptList;


        //[MenuItem("Window/UI Toolkit/ScriptMonitor")]
        public static void Init()
        {
            ScriptMonitor wnd = GetWindow<ScriptMonitor>();
            wnd.titleContent = new GUIContent("ScriptMonitor");
        }

        public void CreateGUI()
        {
            UpdateList();
            FilteredComponentList = ComponentList;
            VisualElement UI = this.UI.Instantiate();

            ToolbarSearchField Search = UI.Q<ToolbarSearchField>("Search");
            Search.RegisterValueChangedCallback(UpdateSearch);

            void UpdateSearch(ChangeEvent<string> evt)
            {
                FilteredComponentList = ComponentList
                    .Where((asset) => asset.ComponentName.ToLower().Contains(evt.newValue.ToLower())).OrderBy(o => o.ComponentName)
                    .ToList();
                ScriptList.itemsSource = FilteredComponentList;
            }

            ScriptList = UI.Q<ListView>("ScriptList");
            ScriptList.itemsSource = FilteredComponentList;
            ScriptList.makeItem = ComponentUI.Instantiate;
            ScriptList.bindItem = BindComponentData;
            void BindComponentData(VisualElement root, int index)
            {
                ComponentData data = ComponentList[index];
                Button btn_list = root.Q<Button>(nameof(btn_list));
                btn_list.clicked += () => data.SelectAll();
                btn_list.text = data.ComponentName;

                Button btn_edit = root.Q<Button>(nameof(btn_edit));
                btn_edit.clicked += () => data.EditScript();
            }

            rootVisualElement.Add(UI);
        }

        #region System

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

        private void UpdateList()
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
        #endregion
    }
}